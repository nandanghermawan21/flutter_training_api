using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;

        public AccountController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("customer-forgot-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerForgotPasswordAsync(string email)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(m => m.Email == email && m.IsDeleted != true);
            if (customer == null)
                return BadRequest("Customer not found");

            try
            {
                var len = 6;
                var totalMinutes = _config.ResetPasswordExpiredTime;
                var code = Guid.NewGuid().ToString("N").Substring(0, len);

                while (await _db.Customers.Where(m => m.ResetPasswordCode == code).FirstOrDefaultAsync() != null)
                    code = Guid.NewGuid().ToString("N").Substring(0, len);

                customer.ResetPasswordCode = code;
                customer.ResetPasswordExpiredTime = DateTime.Now.AddMinutes(totalMinutes);

                string body = $@"<p>Hi <strong>{customer.CustomerName}</strong>,</p>
                    <p>Somebody asked to reset your InovaTrack password. If it's not you, please ignore. 
                    Otherwise, use this code: <strong>{customer.ResetPasswordCode}</strong> to reset your password.
                    (<em>Reset code is active for the next {totalMinutes / 60} hours</em>)</p>
                    <p>Regards,<br/>Administrator</p>";

                await _db.SaveChangesAsync();

                if (await Mail.SendAsync(subject: "Reset Password",
                    body: body,
                    toAddress: customer.Email,
                    emailHost: _config.EmailHost,
                    emailAccount: _config.EmailAccount,
                    emailPassword: _config.EmailPassword,
                    emailPort: _config.EmailPort))
                    return Ok("Email sent");

                return StatusCode(StatusCodes.Status500InternalServerError, "Fail to send email");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("customer-reset-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerResetPasswordAsync(ResetPasswordModel data)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(m => m.ResetPasswordCode == data.ResetCode
                && m.ResetPasswordExpiredTime > DateTime.Now
                && m.IsDeleted != true);
            if (customer == null)
                return BadRequest("Customer not found");

            if (data.NewPassword.Length < 8)
                ModelState.AddModelError("NewPassword", "Password has to be at least 8 characters in length");
            else if (data.NewPassword != data.ConfirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Password confirmation does not match");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var psw = PasswordHasher.Hash(customer.Email, data.NewPassword);
                customer.Password = psw;
                await _db.SaveChangesAsync();

                return Ok("Password updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("customer-register")]
        [HttpPost]
        public async Task<ActionResult> CustomerRegisterAsync(CustomerRegisterModel data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (data.Password.Length < 8)
                ModelState.AddModelError("NewPassword", "Password has to be at least 8 characters in length");
            else if (data.Password != data.ConfirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Password confirmation does not match");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _db.Customers.FirstOrDefaultAsync(m => m.Email == data.Email);
            if (customer != null)
                return BadRequest("Customer already exists");

            try
            {
                var model = new Customer
                {
                    Email = data.Email,
                    Password = PasswordHasher.Hash(data.Email, data.Password),
                    MobileNumber = data.MobileNumber,
                    CustomerName = data.FirstName + (!string.IsNullOrEmpty(data.LastName) ? " " + data.LastName : ""),
                    CreatedDate = DateTime.Now
                };
                _db.Customers.Add(model);
                await _db.SaveChangesAsync();
                return Ok("Customer created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}