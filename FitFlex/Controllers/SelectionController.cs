using System.Threading.Tasks;
using FitFlex.Application.DTO_s.subscriptionDto;
using FitFlex.Application.DTO_s.UserTrainerDto;
using FitFlex.Application.Interfaces;
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
        public async Task<IActionResult> SelectionTrainer([FromBody] TrainerSelectingDtoTrainerSelectingDto dto)
        {
            var result = await _iuser.TrainerSelcetion(dto);

            if (result == null || result.Data == null)
                return NotFound(result);

          
            var subscription = await _iuser.TrainerSelcetion(dto);
            if (subscription == null)
                return BadRequest("No active subscription found for this user");

            return Ok(result);
        }
        [HttpPost("SelectSubscription")]
        public async Task<IActionResult> SubscriptionSelection([FromBody] SubscriptionSelectionDto dto)
        {
            var result = await _iuser.SubscriptionSelection(dto);

            if (result == null || result.Data == null)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("UserByid/{id}")]
        public async Task<IActionResult> SubscriptionById(int id)
        {
            var result = await _iuser.GetUserSubscriptionByUserId(id);

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
        public async Task<IActionResult> GetSubscriptionsByTrainerId(int TrainerID)
        {
            var result = await _iuser.GetSubscriptionsByTrainerId(TrainerID);

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
