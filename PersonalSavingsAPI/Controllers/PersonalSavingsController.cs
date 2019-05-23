using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalSavingsAPI.Controllers
{
    [Route("api/[controller]")]
    public class PersonalSavingsController : ControllerBase
    {
        private readonly IPersonalSavings _personalSavings;
        public PersonalSavingsController(IPersonalSavings personalSavings)
        {
            _personalSavings = personalSavings;
        }


        [HttpGet]
        [Route("getuserdetails")]
        public async Task<IActionResult> GetUserDetails()
        {
            return Ok(await _personalSavings.GetUserDetails());
        }

        [HttpGet]
        [Route("getplandetails")]
        public async Task<IActionResult> GetPlanDetailsForUser()
        {
            return Ok(await _personalSavings.GetPlanDetailsForUser());
        }

        [HttpPost]
        [Route("addcarddetails")]
        public async Task<IActionResult> AddCard([FromBody] TransactionDetails transactionDetails)
        {
            return Ok(await _personalSavings.AddCardDetails(transactionDetails));
        }

        [HttpGet]
        [Route("getallcards")]
        public async Task<IActionResult> GetAllCardDetails()
        {
            return Ok(await _personalSavings.GetAllCardDetails());
        }

        [HttpGet]
        [Route("getallplan")]
        public async Task<IActionResult> GetPlan()
        {
            return Ok(await _personalSavings.GetAllPlan());
        }

        [HttpPost]
        [Route("onetimesaving")]
        public async Task<IActionResult> OneTimeSavings([FromBody] OneTimeSavings oneTimeSavings)
        {
            return Ok(await _personalSavings.AddOnetimeSaving(oneTimeSavings));
        }

        [HttpPost]
        [Route("addplan")]
        public async Task<IActionResult> AddPlan([FromBody] PlanDetails planDetails)
        {
            return Ok(await _personalSavings.AddPlan(planDetails));
        }

        [HttpGet]
        [Route("checkDate")]
        public async Task<IActionResult> CheckDate([FromQuery] string planName)
        {
            return Ok(await _personalSavings.CheckDate(planName));
        }

        [HttpPost]
        [Route("withdrawal")]
        public async Task<IActionResult> Withdrawal([FromBody] OneTimeSavings oneTime)
        {
            return Ok(await _personalSavings.Withdrawal(oneTime.PlanName, oneTime.Amount));
        }


    }
}
