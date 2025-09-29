using System.Security.Claims;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFlex.Controllers
{
    [ApiController]
    [Route("api/selection/[controller]")]
    public class SelectionController : ControllerBase
    {

        private readonly IUserSubscription _iuser;
        public SelectionController(IUserSubscription iuser)
        {
            _iuser = iuser;
        }

        [HttpPost]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> SelectionTrainer([FromBody] TrainerSelectingDto dto)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            //            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);


            if (userId == 0) Unauthorized();

            var trainerselect = await _iuser.TrainerSelcetion(userId, dto.TrainerId);
            if (trainerselect is null) return NotFound(trainerselect);
  
            

            return Ok(trainerselect);
        }
        [HttpPost("SelectSubscription")]
        public async Task<IActionResult> SubscriptionSelection([FromBody] SubscriptionSelectionDto dto)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            
            var result = await _iuser.SubscriptionSelection(dto, userId);

            if (result == null || result.Data == null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("UserByid")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> SubscriptionById()
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var result = await _iuser.GetUserSubscriptionByUserId(userId);

            if (result == null || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var result = await _iuser.AllUserSubscriptions();

            if (result == null || result.Data == null || !result.Data.Any())
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("TrainerByid/")]
        [Authorize(Roles = "Trainer")]

        public async Task<IActionResult> GetSubscriptionsByTrainerId()
        {
            int TrainerId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var result = await _iuser.GetSubscriptionsByTrainerId(TrainerId);

            if (result == null || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPut("block/")]
        public async Task<IActionResult> BlockSubscription(int UserID)
        {
            var result = await _iuser.BlockSubscriptionAsync(UserID);

            if (result == null || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("unblock/")]
        public async Task<IActionResult> UnblockSubscription(int UserID)
        {
            var result = await _iuser.UnblockSubscriptionAsync(UserID);

            if (result == null || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

       


    }
}
