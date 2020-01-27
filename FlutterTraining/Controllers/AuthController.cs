using FlutterTraining.DataModel;
using FlutterTraining.Helper;
using FlutterTraining.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlutterTraining.Controllers
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = authService.Authenticate(request.UserName, request.Password);

            if (user == null)
                return BadRequest(GlobalData.get.resource.emailOrPasswordInCorrect);
            user.password = null;
            return Ok(user);
        }

    }
}