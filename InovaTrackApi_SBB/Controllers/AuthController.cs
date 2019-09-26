using InovaTrackApi_SBB.Model;
using InovaTrackApi_SBB.Context;
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

        //[Route("test")]
        //[HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public IActionResult Test()
        //{
        //    return Ok("Hello World!");
        //}

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

        //[Route("get-user")]
        //[HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public IActionResult GetUser([FromQuery]UserLoginModel request)
        //{
        //    User user = new User();
        //    return Ok(new { user = user });
        //}

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

            return Ok(customer);
        }
    }
}