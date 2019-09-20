using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InovaTrackApi_SBB.Models;
using InovaTrackApi_SBB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Test()
        {
            return Ok("Hello World!");
        }

        [HttpPost]
        public ActionResult Login([FromBody]LoginParams request)
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

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetUser([FromQuery] LoginParams request)
        {
            User user = new User();
            return Ok(new { user = user });
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Logout()
        {
            return Ok(new { status = "OK" });
        }
    }
}