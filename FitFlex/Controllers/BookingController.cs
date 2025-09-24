using FitFlex.Application.DTO_s;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.Users_Model;
using Microsoft.AspNetCore.Mvc;

namespace FitFlex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BookingController : ControllerBase
    {
        private readonly Ibooking _bookingService;

        public BookingController(Ibooking bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllBookings();
            if (result is null) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings()
        {
          
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _bookingService.GetUserBookings(userId);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetTrainerBookings(int trainerId)
        {
            var trainer = await _bookingService.TrainerBooking(trainerId);

            //if (trainer is null) return NotFound(trainer);

            var result = await _bookingService.TrainerBooking(trainerId);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var result = await _bookingService.CreateBooking(dto);
            if (result is null) return NotFound(result);
            return Ok(result);
        }

       
       
    }
}
