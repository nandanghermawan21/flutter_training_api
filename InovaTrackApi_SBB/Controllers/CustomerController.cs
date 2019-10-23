using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.DataModel;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static InovaTrackApi_SBB.DataModel.AuthModel;
using Microsoft.AspNetCore.Authorization;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private CustomerModel _customer;


        public CustomerController(ApplicationDbContext db, IOptions<AppSettings> config)
        {

            _db = db;
            _config = config.Value;
            _customer = new CustomerModel(db);
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerForgotPasswordAsync(string phoneNumber)
        {

            var customer = _customer.CheckPhoneExist(phoneNumber);
            if (customer == null)
                return BadRequest($"{GlobalData.get.resource.phoneNumberNotRegistered}");
            try
            {
                var totalMinutes = _config.ResetPasswordExpiredTime;
                var code = new Random().Next(100000, 999999);

                while (await _db.Customers.Where(m => m.ResetPasswordCode == code.ToString()).FirstOrDefaultAsync() != null)
                    code = new Random().Next(100000, 999999);

                customer.ResetPasswordCode = code.ToString();
                customer.ResetPasswordExpiredTime = DateTime.Now.AddMinutes(totalMinutes);

                await _db.SaveChangesAsync();

                var a = await new Ginota(_config.GinotaApiKey, _config.GinotaApiSecreet).Send(new Ginota.Message()
                {
                    phoneNumber = phoneNumber,
                    flash = false,
                    senderName = _config.GinotaSender,
                    content = $"{GlobalData.get.resource.thereIsTheCodeToResetYourPassword} {code} {GlobalData.get.resource.pleaseDoNotShareThisCodeWithAnyone}",
                });

                if (a != null)
                {
                    return Ok(new
                    {
                        email = customer.Email,
                        code = code,
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"{GlobalData.get.resource.smsFailedToSend}");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("reset-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerResetPasswordAsync(ResetPasswordModel data)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(m => m.ResetPasswordCode == data.ResetCode
                && m.ResetPasswordExpiredTime > DateTime.Now
                && m.IsDeleted != true);

            if (customer == null)
                return BadRequest($"{GlobalData.get.resource.customerNotFound}");

            if (data.NewPassword.Length < 8)
                ModelState.AddModelError("Password", $"{GlobalData.get.resource.newPasswordMustBeAtLeast8CharacterLong}");

            else if (data.NewPassword != data.ConfirmPassword)
                ModelState.AddModelError("Confirm Password", $"{GlobalData.get.resource.confirmPaswordNotMatch}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var psw = PasswordHasher.Hash(customer.Email, data.NewPassword);
                customer.Password = psw;
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    email = customer.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> CustomerRegisterAsync(CustomerRegisterModel data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (data.Password.Length < 8)
                ModelState.AddModelError("NewPassword", $"{GlobalData.get.resource.newPasswordMustBeAtLeast8CharacterLong}");
            else if (data.Password != data.ConfirmPassword)
                ModelState.AddModelError("ConfirmPassword", $"{GlobalData.get.resource.confirmPaswordNotMatch}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //chek phone exist
            var customer = _customer.CheckPhoneExist(data.MobileNumber);
            if (customer != null)
                return BadRequest($"{GlobalData.get.resource.phoneNumberRegistered}");

            //chek email exist
            customer = await _db.Customers.FirstOrDefaultAsync(m => m.Email == data.Email);
            if (customer != null)
                return BadRequest($"{GlobalData.get.resource.emailAlreadyRegistered}");

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
                return Ok($"{GlobalData.get.resource.registerSuccess}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("update-profile/{id}")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CustomerUpdateProfile(int id, ProfileModel profile)
        {
            try
            {
                Customer customer = new Customer
                {
                    CustomerId = id,
                    Email = profile.email,
                    CustomerName = profile.name,
                    Address1 = profile.addrss,
                    customerAvatar = profile.avatar,
                };

                customer = _customer.update(customer);

                if (customer == null)
                    return BadRequest(GlobalData.get.resource.customerNotFound);

                return Ok(customer);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("get")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(int? id, string salesId = null)
        {
            try
            {
                var data = _customer.get(userId: id, salesId: salesId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("checkphoneexist")]
        [HttpGet]
        public ActionResult CheckPhoneNUmberExist(string phoneNumber)
        {
            try
            {
                var customers = _customer.CheckPhoneExist(phoneNumber);

                return Ok(new
                {
                    customerId = customers != null ? customers.CustomerId : 0,
                    phoneNumber = phoneNumber,
                    statusString = customers == null ? GlobalData.get.resource.phoneNotFound : $"{GlobalData.get.resource.phoneNumberRegistered}",
                    statusCode = customers != null ? true : false
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
