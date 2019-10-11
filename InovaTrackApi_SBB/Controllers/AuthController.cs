using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Models;
using InovaTrackApi_SBB.Services;
using Microsoft.AspNetCore.Mvc;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [Route("user-login")]
        [HttpPost]
        public ActionResult Login([FromBody]UserLoginModel request)
        {
            var user = authService.Authenticate(request.UserName, request.Password);

            if (user == null)
                return BadRequest(new { message = "User Name or Password is incorrect" });

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.UserFullName,
                user.Token
            });
        }

        [Route("logout")]
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Logout()
        {
            return Ok(new { status = "OK" });
        }

        [Route("customer-login")]
        [HttpPost]
        public ActionResult CustomerLogin([FromBody]CustomerLoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = authService.AuthenticateCustomer(request.Email, request.Password);

            if (customer == null)
                return BadRequest(new { message = "Email or Password is incorrect" });

            customer.Password = null;
            return Ok(customer);
        }

        //[Route("driver-login")]
        //[HttpPost]
        //public ActionResult DriverLogin([FromBody]CustomerLoginModel request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var driver = authService.AuthenticateDriver(request.Email, request.Password);

        //    if (driver == null)
        //        return BadRequest(new { message = "Email or Password is incorrect" });

        //    driver.Password = null;
        //    return Ok(driver);
        //}
    }
}