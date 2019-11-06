using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
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
                return BadRequest("User Name or Password is incorrect");

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
        public ActionResult CustomerLogin([FromBody]UserLoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = authService.AuthenticateCustomer(request.UserName, request.Password);

            if (customer == null)
                return BadRequest(GlobalData.get.resource.emailOrPasswordInCorrect);
            customer.Password = null;
            return Ok(customer);
        }

        [Route("sales-login")]
        [HttpPost]
        public ActionResult SalesLogin([FromBody]UserLoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sales = authService.AuthenticateSales(request.UserName, request.Password);

            if (sales == null)
                return BadRequest(GlobalData.get.resource.emailOrPasswordInCorrect);

            sales.password = null;
            return Ok(sales);
        }

        [Route("driver-login")]
        [HttpPost]
        public ActionResult DriverLogin([FromBody]UserLoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driver = authService.AuthenticateDriver(request.UserName, request.Password);

            if (driver == null)
                return BadRequest(GlobalData.get.resource.emailOrPasswordInCorrect);

            driver.password = null;
            return Ok(driver);
        }

        [Route("qc-login")]
        [HttpPost]
        public ActionResult QcLogin([FromBody]UserLoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driver = authService.AuthenticateQc(request.UserName, request.Password);

            if (driver == null)
                return BadRequest(GlobalData.get.resource.emailOrPasswordInCorrect);

            driver.password = null;
            return Ok(driver);
        }


    }
}