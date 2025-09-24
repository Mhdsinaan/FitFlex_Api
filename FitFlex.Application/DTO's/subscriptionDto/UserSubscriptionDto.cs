using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class UserSubscriptionDto
    {
        public int Id { get; set; }                      
        public int UserId { get; set; }                
        public string UserName { get; set; }            
        public int SubscriptionPlanId { get; set; }      
        public string PlanName { get; set; }             
        public decimal Price { get; set; }             
        public DateTime StartDate { get; set; }          
        public DateTime EndDate { get; set; }           
        public subscriptionStatus Status { get; set; }              
        public bool IsPaid { get; set; }                
        public DateTime? PaidOn { get; set; }            
        public DateTime CreatedOn { get; set; }         
        public DateTime? ModifiedOn { get; set; }
    }
}
