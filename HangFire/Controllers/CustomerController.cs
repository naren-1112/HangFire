using Microsoft.AspNetCore.Mvc;
using HangFire.Models;
using HangFire.Services;
using Hangfire.Logging;
using Hangfire;

namespace HangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        public static List<Details> details = new List<Details>();
        private readonly ICustomerServices _customerservices;
        public CustomerController(ICustomerServices customerservices)
        {
            _customerservices = customerservices;
        }
        [HttpPost]
        public IActionResult AddDetails(Details detail)
        {
            if (ModelState.IsValid)
            {
                details.Add(detail);
                _customerservices.AddCustReview(detail);
                return CreatedAtAction("Add Review", new { detail.ID }, detail);
            }
            return BadRequest();
        }
    

    
        [HttpGet]
        public IActionResult GetDetails(int ID) 
        {
            var Details = _customerservices.GetReviews();

            var detail = details.FirstOrDefault(x => x.ID == ID);
            if (detail == null)
                return NotFound();
            BackgroundJob.Enqueue<ICustomerServices>(x => x.SyncData());
            return Ok(detail);
        }
    }
}
