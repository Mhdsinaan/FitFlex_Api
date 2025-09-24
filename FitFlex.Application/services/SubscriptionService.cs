using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.Services
{
    public class SubscriptionService : ISubscription
    {
        private readonly IRepository<SubscriptionPlan> _subscription;
        private readonly IMapper _mapper;

        public SubscriptionService(IRepository<SubscriptionPlan> subscription, IMapper mapper)
        {
            _subscription = subscription;
            _mapper = mapper;
        }

        public async Task<APiResponds<SubscriptionPlansResponseDto>> CreatePlanAsync(SubscriptionPlanDto plan)
        {
            var subscriptions = await _subscription.GetAllAsync();
            var existing = subscriptions.FirstOrDefault(p => p.Name == plan.Name);

            if (existing != null)
                return new APiResponds<SubscriptionPlansResponseDto>("400", "Plan already exists", null);

            var newPlan = new SubscriptionPlan
            {
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInMonth = plan.DurationInMonth
            };

            await _subscription.AddAsync(newPlan);
            await _subscription.SaveChangesAsync();

            var response = _mapper.Map<SubscriptionPlansResponseDto>(newPlan);

            return new APiResponds<SubscriptionPlansResponseDto>("200", "Plan created successfully", response);
        }

        public async Task<APiResponds<bool>> DeletePlanAsync(int id)
        {
            var plans = await _subscription.GetAllAsync();
            var plan = plans.FirstOrDefault(p => p.Id == id && !p.IsDelete);

            if (plan == null)
                return new APiResponds<bool>("404", "Plan not found", false);

            plan.IsDelete = true;
            await _subscription.SaveChangesAsync();

            return new APiResponds<bool>("200", "Plan deleted successfully", true);
        }

        public async Task<APiResponds<IEnumerable<SubscriptionPlansResponseDto>>> GetAllPlansAsync()
        {
            var plans = await _subscription.GetAllAsync();
            if (plans == null || !plans.Any())
                return new APiResponds<IEnumerable<SubscriptionPlansResponseDto>>("200", "No plans available", Enumerable.Empty<SubscriptionPlansResponseDto>());

            var planDtos = _mapper.Map<IEnumerable<SubscriptionPlansResponseDto>>(plans);
            return new APiResponds<IEnumerable<SubscriptionPlansResponseDto>>("200", "Plans retrieved successfully", planDtos);
        }

        public async Task<APiResponds<SubscriptionPlansResponseDto>> GetPlanByIdAsync(int id)
        {
            var plan = await _subscription.GetByIdAsync(id);
            if (plan == null)
                return new APiResponds<SubscriptionPlansResponseDto>("404", "Plan not found", null);

            var response = _mapper.Map<SubscriptionPlansResponseDto>(plan);
            return new APiResponds<SubscriptionPlansResponseDto>("200", "Plan retrieved successfully", response);
        }

        public async Task<APiResponds<SubscriptionPlansResponseDto>> UpdatePlanAsync(int id, SubscriptionPlanDto planDto)
        {
            var existing = await _subscription.GetByIdAsync(id);
            if (existing == null)
                return new APiResponds<SubscriptionPlansResponseDto>("404", "Plan not found", null);

            existing.Name = planDto.Name;
            existing.Description = planDto.Description;
            existing.Price = planDto.Price;
            existing.DurationInMonth = planDto.DurationInMonth;
            existing.ModifiedOn = DateTime.UtcNow;

            _subscription.Update(existing);
            await _subscription.SaveChangesAsync();

            var response = _mapper.Map<SubscriptionPlansResponseDto>(existing);
            return new APiResponds<SubscriptionPlansResponseDto>("200", "Plan updated successfully", response);
        }
    }
}
