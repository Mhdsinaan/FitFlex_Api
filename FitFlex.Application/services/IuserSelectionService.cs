using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Db_context;
using FitFlex.Infrastructure.Interfaces;
using FitFlex.Infrastructure.Repository_service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FitFlex.Application.services
{
    public class IuserSelectionService : IUserSubscription
    {
        private readonly IRepository<UserSubscription> _usersub;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Trainer> _TrainerRepo;
        private readonly IRepository<SubscriptionPlan> _subscription;
        private readonly IRepository<UserTrainer> _userTraniner;
        public IuserSelectionService(IRepository<UserSubscription> usersub, IRepository<User> userRepo, IRepository<Trainer> TrainerRepo, IRepository<SubscriptionPlan> subscription,
            IRepository<UserTrainer> userTraniner)
        {
            _usersub = usersub;
            _userRepo = userRepo;
            _TrainerRepo = TrainerRepo;
            _subscription = subscription;
            _userTraniner = userTraniner;
        }

        public async Task<APiResponds<List<UserSubscriptionResponseDto>>> AllUserSubscriptions()
        {
            var allplans = await _usersub.GetAllQueryable()
                .Include(u => u.User)
                .Include(t => t.Trainer)
                .Include(s => s.Subscription)
                .ToListAsync();

            if (!allplans.Any())
                return new APiResponds<List<UserSubscriptionResponseDto>>("404", "notfound", null);

            var allDtos = allplans.Select(plan => new UserSubscriptionResponseDto
            {
                UserId = plan.UserId,
                UserName = plan.User?.UserName,
                PlanId = plan.SubscriptionId,
                PlanName = plan.Subscription?.Name,
                TrainerId = plan.TrainerID,
                TrainerName = plan.Trainer?.FullName,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                SubscriptionStatus = plan.SubscriptionStatus
            }).ToList();

            return new APiResponds<List<UserSubscriptionResponseDto>>("200", "success", allDtos);
        }


        public async Task<APiResponds<List<UserSubscriptionResponseDto>>> GetSubscriptionsByTrainerId(int trainerId)
        {

            var subscriptions = await _usersub.GetAllQueryable()
                .Include(u => u.User)
                .Include(s => s.Subscription)
                .Include(t => t.Trainer)
                .Where(s => s.TrainerID == trainerId)
                .ToListAsync();

            if (!subscriptions.Any())
                return new APiResponds<List<UserSubscriptionResponseDto>>("404", "No subscriptions for this trainer", null);


            var result = subscriptions.Select(s => new UserSubscriptionResponseDto
            {
                UserId = s.UserId,
                UserName = s.User?.UserName,
                PlanId = s.SubscriptionId,
                PlanName = s.Subscription?.Name,
                TrainerId = s.TrainerID,
                TrainerName = s.Trainer?.FullName,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                SubscriptionStatus = s.SubscriptionStatus
            }).ToList();

            return new APiResponds<List<UserSubscriptionResponseDto>>("200", "success", result);
        }


        public async Task<APiResponds<UserSubscriptionResponseDto>> GetUserSubscriptionByUserId(int userId)
        {
            try
            {
                var usersub = await _usersub.GetAllQueryable()
                    .Include(u => u.User)
                    .Include(t => t.Trainer)
                    .Include(s => s.Subscription)
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (usersub == null)
                    return new APiResponds<UserSubscriptionResponseDto>("404", "Subscription not found for this user", null);
                if (usersub.User.Role == UserRole.Admin)
                {
                    return new APiResponds<UserSubscriptionResponseDto>(
                        "403",
                        "Admin users do not have subscriptions",
                        null
                    );
                }


                var response = new UserSubscriptionResponseDto
                {
                    UserId = usersub.UserId,
                    UserName = usersub.User?.UserName,
                    PlanId = usersub.SubscriptionId,
                    PlanName = usersub.Subscription?.Name,
                    TrainerId = usersub.TrainerID,
                    TrainerName = usersub.Trainer?.FullName,
                    StartDate = usersub.StartDate,
                    EndDate = usersub.EndDate,
                    SubscriptionStatus = usersub.SubscriptionStatus
                };

                return new APiResponds<UserSubscriptionResponseDto>("200", "Subscription fetched successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<UserSubscriptionResponseDto>(
                    "500",
                    $"Error: {ex.Message}",
                    null
                );
            }
        }


        public async Task<APiResponds<List<UserSubscriptionResponseDto>>> AllSubscriptions()
        {
            var allplans = await _usersub.GetAllAsync();
            if (allplans == null || !allplans.Any())
                return new APiResponds<List<UserSubscriptionResponseDto>>("404", "notfound", null);

            var allDtos = allplans.Select(plan => new UserSubscriptionResponseDto
            {
                UserName = plan.User?.UserName, 
                PlanName = plan.Subscription?.Name, 
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                SubscriptionStatus = plan.SubscriptionStatus,
                PlanId=plan.SubscriptionId,
                UserId=plan.UserId,
                TrainerId=plan.TrainerID
                
            }).ToList();

            return new APiResponds<List<UserSubscriptionResponseDto>>("200", "success", allDtos);
        }




        public async Task<APiResponds<UserSubscriptionResponseDto>> SubscriptionSelection(int PlanID,int UserID)
        {
            try
            {

                
                var user = await _userRepo.GetByIdAsync(UserID);
                if (user is null)
                    return new APiResponds<UserSubscriptionResponseDto>("404", "User not found", null);
                if (user.Role == UserRole.Admin)
                {
                    return new APiResponds<UserSubscriptionResponseDto>(
                        "403",
                        "Admin users do not have subscriptions",
                        null
                    );
                }


                
                var userTrainer = await _userTraniner.GetAllQueryable()
                      .Include(ut => ut.Trainer)
                      .FirstOrDefaultAsync(ut => ut.UserId == UserID);


                if (userTrainer is null)
                    return new APiResponds<UserSubscriptionResponseDto>("404", "Trainer not assigned to this user", null);

                var trainerId = userTrainer.TrainerId;


               
                var plan = await _subscription.GetByIdAsync(PlanID);
                if (plan ==null || plan.IsDelete)
                    return new APiResponds<UserSubscriptionResponseDto>("404", "Plan not found", null);

                var existing = (await _usersub.GetAllAsync())
                                .FirstOrDefault(s => s.UserId == UserID && s.EndDate > DateTime.UtcNow);
                if (existing != null)
                    return new APiResponds<UserSubscriptionResponseDto>("400", "User already has an active subscription", null);

                var newSub = new UserSubscription
                {
                    UserId = user.ID,
                    SubscriptionId = plan.Id,
                    TrainerID=trainerId,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(plan.DurationInMonth),
                    SubscriptionStatus = subscriptionStatus.pending,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedBy = user.ID
                };

                await _usersub.AddAsync(newSub);
                await _usersub.SaveChangesAsync();

                var response = new UserSubscriptionResponseDto
                {
                    UserId = newSub.UserId,
                    PlanId = newSub.SubscriptionId,
                    UserName=user.UserName,
                    PlanName=plan.Name,
                    TrainerName= userTrainer.Trainer.FullName,
                    TrainerId=trainerId,

                    StartDate = newSub.StartDate,
                    EndDate = newSub.EndDate,
                    SubscriptionStatus = newSub.SubscriptionStatus
                };

                return new APiResponds<UserSubscriptionResponseDto>("200", "Subscription selected successfully", response);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner: " + ex.InnerException.Message;
                }

                return new APiResponds<UserSubscriptionResponseDto>(
                    "500",
                    $"Error: {errorMessage}",
                    null
                );
            }
        }
        public async Task<APiResponds<UserTrainerResponseDto>> TrainerSelcetion(int userid, int TrainerID)
        {
            try
            {


                var userById = await _userRepo.GetByIdAsync(userid);
                if (userById is null) return new APiResponds<UserTrainerResponseDto>("401", "user  not found", null);

                var trainer = await _TrainerRepo.GetByIdAsync(TrainerID);
                if (trainer ==null || (trainer.status != TrainerStatus.Accept)) return new APiResponds<UserTrainerResponseDto>("404", "Trainer not found", null);
                var usertrainers = await _userTraniner.GetAllAsync();
                var exist = usertrainers.FirstOrDefault(p => p.UserId == userid && p.IsDelete == false);
                if (exist != null)
                {
                    var res = new UserTrainerResponseDto
                    {
                        UserId = userid,
                        TrainerId = trainer.Id,
                        AssignedDate = exist.CreatedOn,
                        TrainerName = trainer.FullName,
                        UserName = userById.UserName,
                        shift=trainer.Shift.ToString()
                        
                    };
                    return new APiResponds<UserTrainerResponseDto>("200", "User already has a trainer", res);
                }
                    
                //var existingPlan = await _usersub.GetByIdAsync(dto.UserSubscriptionID);
                //if (existingPlan != null) return new APiResponds<UserTrainerResponseDto>("404", "USer have a plan", null);
                var userplan = new UserTrainer
                {
                    UserId = userid,
                    TrainerId = trainer.Id,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = userid

                };
                await _userTraniner.AddAsync(userplan);
                await _userTraniner.SaveChangesAsync();

                var responseDto = new UserTrainerResponseDto
                {
                    UserId = userplan.UserId,
                    TrainerId = userplan.TrainerId,
                    AssignedDate = userplan.CreatedOn,
                    TrainerName = trainer.FullName,
                    shift = trainer.Shift.ToString(),


                    UserName = userById.UserName,


                };

                return new APiResponds<UserTrainerResponseDto>("200", "Trainer assigned successfully.", responseDto);

            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"{ex.Message} | Inner: {ex.InnerException.Message}"
                    : ex.Message;

                return new APiResponds<UserTrainerResponseDto>("500", $"An error occurred: {errorMessage}", null);
            }



        }
        public async Task<APiResponds<bool>> BlockSubscriptionAsync(int userId)
        {
            var sub = await _usersub.GetAllQueryable()
                .FirstOrDefaultAsync(s =>  s.UserId == userId);

            if (sub == null)
                return new APiResponds<bool>("404", "Subscription not found for this user", false);

            if(sub.SubscriptionStatus==subscriptionStatus.Blocked)
            {
                return new APiResponds<bool>("404", "its already blocked user", false);
            }
            sub.SubscriptionStatus = subscriptionStatus.Blocked;
            sub.BlockedAt = DateTime.UtcNow;

            _usersub.Update(sub);
            await _usersub.SaveChangesAsync();

            return new APiResponds<bool>("200", "Subscription blocked successfully", true);
        }


        public async Task<APiResponds<bool>> UnblockSubscriptionAsync(int userId)
        {
            var sub = await _usersub.GetAllQueryable()
                .FirstOrDefaultAsync(s =>  s.UserId == userId && s.SubscriptionStatus==subscriptionStatus.Blocked);

            if (sub == null)
                return new APiResponds<bool>("404", "there is no blocked user", false);

            if (sub.BlockedAt.HasValue && DateTime.UtcNow - sub.BlockedAt.Value > TimeSpan.FromDays(150))
            {
                sub.SubscriptionStatus = subscriptionStatus.Expired;

                _usersub.Update(sub);
                await _usersub.SaveChangesAsync();

                return new APiResponds<bool>("410", "Subscription expired after block period", false);
            }

            
            sub.SubscriptionStatus = subscriptionStatus.Active;
            sub.BlockedAt = null;

            _usersub.Update(sub);
            await _usersub.SaveChangesAsync();

            return new APiResponds<bool>("200", "Subscription unblocked successfully", true);
        }

        
    }


}

