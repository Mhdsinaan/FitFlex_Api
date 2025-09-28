using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitFlex.Application.services
{
    public class AdditionalSubscriptionService : IAdditionalSubscriptionService
    {
        private readonly IRepository<UserSubscriptionAddOn> _Addon;
        private readonly IRepository<User> _Userrepo;
        private readonly IRepository<UserSubscription> _userSubscriptionRepo;
        private readonly IRepository<SubscriptionPlan> _SubscriptionRepo;

        public AdditionalSubscriptionService(
            IRepository<UserSubscriptionAddOn> Addon,
            IRepository<UserSubscription> userSubscriptionRepo,
            IRepository<SubscriptionPlan> SubscriptionRepo,
            IRepository<User> Userrepo)
        {
            _Addon = Addon;
            _userSubscriptionRepo = userSubscriptionRepo;
            _SubscriptionRepo = SubscriptionRepo;
            _Userrepo = Userrepo;
        }

        public async Task<APiResponds<AdditionalFeatureResponseDto>> AddAdditionalFeatureAsync(AddAdditionalFeatureRequestDto request,int userId)
        {
            var mainSub = await _userSubscriptionRepo.GetByIdAsync(userId);
           
          
            if (mainSub == null)
                return new APiResponds<AdditionalFeatureResponseDto>("404", "Subscription not found", null);

            var addon = new UserSubscriptionAddOn
            {
                UserSubscriptionId = mainSub.SubscriptionId,
                CreatedOn = mainSub.CreatedOn,
                EndDate = mainSub.EndDate,
                PaymentStatus = PaymentStatus.Processing
            };

            await _Addon.AddAsync(addon);
            await _Addon.SaveChangesAsync();

            return new APiResponds<AdditionalFeatureResponseDto>(
                "200",
                "Additional subscription added successfully",
                new AdditionalFeatureResponseDto
                {
                  
                    Price = addon.Price,
                    StartDate = addon.CreatedOn,
                    EndDate = addon.EndDate,
                    PaymentStatus = addon.PaymentStatus.ToString()
                });
        }

        public async Task<APiResponds<AdditionalFeatureResponseDto>> Additonalsubscription(int UserID, AddAdditionalFeatureRequestDto dto)
        {
            try
            {

                var all = await _Userrepo.GetByIdAsync(UserID);
                if (all is null) return new APiResponds<AdditionalFeatureResponseDto>("404", "user not found", null);

                

                var userplan = await _userSubscriptionRepo.GetByIdAsync(dto.UserSubID);
                if (userplan is null ) return new APiResponds<AdditionalFeatureResponseDto>("404", "user have  not plan", null);

                var plan = await _SubscriptionRepo.GetByIdAsync(dto.PlanID);
                if (plan is null || !plan.IsAdditional) return new APiResponds<AdditionalFeatureResponseDto>("404", "user  plan not found", null);

                var additonal = new UserSubscriptionAddOn
                {
                    PlanID = dto.PlanID,
                    UserId = UserID,
                    UserSubscriptionId = userplan.Id,
                    CreatedOn = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    PaymentStatus = PaymentStatus.Pending,
                    Status = subscriptionStatus.pending,
                    CreatedBy = UserID,



                };

                await _Addon.AddAsync(additonal);
                await _Addon.SaveChangesAsync();

                var response = new  AdditionalFeatureResponseDto
                {
                    UserID = UserID,
                    PaymentStatus = additonal.PaymentStatus.ToString(),
                    Status = additonal.Status.ToString(),
                    Price = additonal.Price,
                    StartDate = additonal.CreatedOn,
                    EndDate = additonal.EndDate,
                    


                };

                return new APiResponds<AdditionalFeatureResponseDto>("200", "Additional subscription added successfully", response);
            }
            catch (Exception ex)
            {
                return new APiResponds<AdditionalFeatureResponseDto>("500", $"Error: {ex.Message}", null);


            }
        }
        public async Task<APiResponds<SubscriptionPlansResponseDto>> AdditonalsubscriptionByID(int planId)
        {
            try
            {
                var byid = await _SubscriptionRepo.GetByIdAsync(planId);
                if (byid is null)
                    return new APiResponds<SubscriptionPlansResponseDto>("404", "Plan not found", null);

                var res = new SubscriptionPlansResponseDto
                {
                    Id = byid.Id,
                    Name = byid.Name,
                    Description = byid.Description,
                    Price = byid.Price,
                    DurationInMonth= byid.DurationInMonth
                };

                return new APiResponds<SubscriptionPlansResponseDto>("200", "Plan fetched successfully", res);
            }
            catch (Exception ex)
            {
                return new APiResponds<SubscriptionPlansResponseDto>("500", $"Internal Server Error: {ex.Message}", null);
            }
        }

      

        public async Task<APiResponds<List<SubscriptionPlansResponseDto>>> AllAdditionalSubscription()
        {
            try
            {
                var plans = await _SubscriptionRepo.GetAllAsync();

                if (plans == null || !plans.Any())
                    return new APiResponds<List<SubscriptionPlansResponseDto>>("404", "No subscription plans found", null);

                var res = plans.Select(p => new SubscriptionPlansResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DurationInMonth = p.DurationInMonth
                }).ToList();

                return new APiResponds<List<SubscriptionPlansResponseDto>>("200", "Plans fetched successfully", res);
            }
            catch (Exception ex)
            {
                return new APiResponds<List<SubscriptionPlansResponseDto>>("500", $"Internal Server Error: {ex.Message}", null);
            }
        }
        

        public async Task<APiResponds<SubscriptionPlansResponseDto>> CreatePlanAsync(AdditionalSubscriptionPlanDto plan)
        {
            var create = await _SubscriptionRepo.GetAllAsync();
            var exist = create.FirstOrDefault(p => p.Name == plan.Name);
            if (exist != null) return new APiResponds<SubscriptionPlansResponseDto>("409", "Plan Already exist", null);
            var newplan = new SubscriptionPlan
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationInMonth = plan.DurationInMonth,
                Price = plan.Price,
                IsAdditional = true

            };
            await _SubscriptionRepo.AddAsync(newplan);
            await _SubscriptionRepo.SaveChangesAsync();

            var response = new SubscriptionPlansResponseDto
            {
                Id = newplan.Id,
                Name = newplan.Name,
                Description = newplan.Description,
                DurationInMonth = newplan.DurationInMonth,
                Price = newplan.Price,
            };
            return new APiResponds<SubscriptionPlansResponseDto>("200", "Subscription plan created successfully", response);



   
        }

        

        public async Task<APiResponds<bool>> RemoveAdditionalFeatureAsync(int additionalSubscriptionId)
        {
            var addon = await _Addon.GetByIdAsync(additionalSubscriptionId);
            if (addon == null) return new APiResponds<bool>("404", "removed sucessfully", false);


            _Addon.Delete(addon);
            await _Addon.SaveChangesAsync();

            return new APiResponds<bool>("200", "sucessfully deleted", true);

        }

    }
}