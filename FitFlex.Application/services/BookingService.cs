using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Booking_model;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;

namespace FitFlex.Application.services
{
    public class BookingService : Ibooking
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Trainer> _trainerRepo;
      private readonly  IRepository<UserSubscription> _plan;

        public BookingService(
            IRepository<Booking> bookingRepo,
            IRepository<User> userRepo,
             IRepository<UserSubscription> plan,
            IRepository<Trainer> trainerRepo)
        {
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _trainerRepo = trainerRepo;
            _plan = plan;
        }

        public Task<APiResponds<BookingResponseDto>> CancelBooking(int bookingId)
        {
            throw new NotImplementedException();
        }

        public async Task<APiResponds<BookingResponseDto>> CreateBooking(CreateBookingDto dto)
        {

            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null)
                return new APiResponds<BookingResponseDto>("404", "User not found", null);

            var trainer = await _trainerRepo.GetByIdAsync(dto.TrainerId);
            if (trainer == null)
                return new APiResponds<BookingResponseDto>("404", "Trainer not found", null);

            //var plan = await _plan.GetByIdAsync(dto.UserId);
            //if (plan == null)
            //    return new APiResponds<BookingResponseDto>("404", "Plan not found", null);



            var existingBooking = await _bookingRepo.GetAllAsync();
            var result = existingBooking.FirstOrDefault(p =>
                p.UserID == dto.UserId &&
                p.CreatedOn.Date == dto.BookingDate.Date 
               
            );

            if (result != null)
                return new APiResponds<BookingResponseDto>("400", "This shift is already booked", null);


            var booking = new Booking
            {
                UserID = dto.UserId,
                TrainerId = dto.TrainerId,
                CreatedOn = dto.BookingDate,
                Shift = dto.shift,
                



            };


            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();


            var response = new BookingResponseDto
            {

                UserId = booking.UserID,
                //UserName=booking.User.UserName,
                //TrainerName=booking.Trainer.FullName,
               
                TrainerId = booking.TrainerId,
                
                BookingDate = booking.CreatedOn,
                Session = booking.Shift.ToString()
                
            };

            return new APiResponds<BookingResponseDto>("200", "Booking created successfully", response);
        }


        public async Task<APiResponds<List<BookingResponseDto>>> GetAllBookings()
        {
            var allBooking = await _bookingRepo.GetAllAsync();
            
            if (allBooking == null || !allBooking.Any())
                return new APiResponds<List<BookingResponseDto>>("404", "No booking found", null);


            var bookingDtos = allBooking.Select(b => new BookingResponseDto
            {

                UserId = b.UserID,
                BookingDate = b.CreatedOn,
                TrainerId = b.TrainerId,
                Session = b.Shift.ToString()
               


            }).ToList(); ;

            return new APiResponds<List<BookingResponseDto>>("200", "Bookings retrieved successfully", bookingDtos);
        }

        public async Task<APiResponds<List<BookingResponseDto>>> GetUserBookings(int userId)
        {
            var bookings = await _bookingRepo.GetAllAsync();
            var userBookings = bookings.Where(b => b.UserID == userId).ToList();

            if (userBookings == null || !userBookings.Any())
                return new APiResponds<List<BookingResponseDto>>("404", "No bookings found for this user", null);

            var bookingDtos = userBookings.Select(b => new BookingResponseDto
            {
                UserId = b.UserID,
                TrainerId = b.TrainerId,
                BookingDate = b.CreatedOn,
              

                Session = b.Shift.ToString()
               
            }).ToList();

            return new APiResponds<List<BookingResponseDto>>("200", "User bookings retrieved successfully", bookingDtos);
        }



        public async Task<APiResponds<List<BookingResponseDto>>> TrainerBooking(int trainerId)
        {

            var today = DateTime.UtcNow.Date;

            var trainer = await _trainerRepo.GetByIdAsync(trainerId);
            if (trainer == null) return new APiResponds<List<BookingResponseDto>>("404", "trainer not found", null);

            var bookings = await _bookingRepo.GetAllAsync();
            var todayBookings = bookings
                 .Where(p => p.TrainerId == trainerId && p.CreatedOn.Date == today).ToList();



            if (todayBookings == null || !todayBookings.Any())
                return new APiResponds<List<BookingResponseDto>>("404", "No bookings found for today", null);


            var bookingDtos = todayBookings.Select(b => new BookingResponseDto
            {
                UserId = b.UserID,
                TrainerId = b.TrainerId,
                BookingDate = b.CreatedOn,
                Session = b.Shift.ToString()
               
            }).ToList();

            return new APiResponds<List<BookingResponseDto>>("200", "Trainer bookings retrieved successfully", bookingDtos);
        }

    }
}