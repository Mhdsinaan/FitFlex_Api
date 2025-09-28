using System.ComponentModel.DataAnnotations;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;

public class UserSubscription : BaseEntity
{
   
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SubscriptionId { get; set; }
    public int TrainerID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public subscriptionStatus SubscriptionStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

   
    public User User { get; set; }
    public Trainer Trainer { get; set; }
    public SubscriptionPlan Subscription { get; set; }
    public ICollection<UserSubscriptionAddOn> UserSubscriptionAddOns { get; set; }



    public DateTime? BlockedAt { get; set; }
}
