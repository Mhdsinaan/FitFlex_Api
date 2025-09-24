using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Booking_model;

namespace FitFlex.Application.Interfaces
{
    public interface Ibooking
    {
       
        
            Task<APiResponds<List<BookingResponseDto>>> GetAllBookings();
            Task<APiResponds<List<BookingResponseDto>>> GetUserBookings(int userId);
            Task<APiResponds<List<BookingResponseDto>>> TrainerBooking(int trainerId);
            Task<APiResponds<BookingResponseDto>> CreateBooking(CreateBookingDto Dto);
            Task<APiResponds<BookingResponseDto>> CancelBooking(int bookingId);
        


    }
}
