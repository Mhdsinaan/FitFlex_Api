using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitFlex.Application.services
{
    public class AdditionalSubscriptionService : IAdditionalSubscriptionService
    {
        private readonly IRepository<UserSubscriptionAddOn> _Addon;
        private readonly IRepository<UserSubscription> _userSubscriptionRepo;

        public AdditionalSubscriptionService(
            IRepository<UserSubscriptionAddOn> Addon,
            IRepository<UserSubscription> userSubscriptionRepo)
        {
            _Addon = Addon;
            _userSubscriptionRepo = userSubscriptionRepo;
        }

        public async Task<APiResponds<AdditionalFeatureResponseDto>> AddAdditionalFeatureAsync(AddAdditionalFeatureRequestDto request)
        {
            var mainSub = await _userSubscriptionRepo.GetAllQueryable()
                .Include(u => u.UserSubscriptionAddOns)
                .FirstOrDefaultAsync(u => u.Id == request.UserSubscriptionId);

            if (mainSub == null)
                return new APiResponds<AdditionalFeatureResponseDto>("404", "Subscription not found", null);

            var addon = new UserSubscriptionAddOn
            {
                UserSubscriptionId = mainSub.Id,
                FeatureName = request.FeatureName,
                Price = request.Price,
                CreatedOn = request.StartDate,
                EndDate = request.EndDate,
                PaymentStatus = PaymentStatus.Processing
            };

            await _Addon.AddAsync(addon);
            await _Addon.SaveChangesAsync();

            return new APiResponds<AdditionalFeatureResponseDto>(
                "200",
                "Additional subscription added successfully",
                new AdditionalFeatureResponseDto
                {
                    Id = addon.Id,
                    FeatureName = addon.FeatureName,
                    Price = addon.Price,
                    StartDate = addon.CreatedOn,
                    EndDate = addon.EndDate,
                    PaymentStatus = addon.PaymentStatus.ToString()
                });
        }


        public async Task<APiResponds<List<AdditionalFeatureResponseDto>>> GetAddOnsByUserSubscriptionAsync(int userSubscriptionId)
        {
            var addons = await _Addon.GetAllQueryable()
                .Where(a => a.UserSubscriptionId == userSubscriptionId)
                .ToListAsync();

            var responseData = addons.Select(a => new AdditionalFeatureResponseDto
            {
                Id = a.Id,
                FeatureName = a.FeatureName,
                Price = a.Price,
                StartDate = a.CreatedOn,
                EndDate = a.EndDate,
                PaymentStatus = a.PaymentStatus.ToString()
            }).ToList();

            return new APiResponds<List<AdditionalFeatureResponseDto>>("200", "Add on subscription", responseData);
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