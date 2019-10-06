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
            _customer = new CustomerModel(db, config);
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerForgotPasswordAsync(string phoneNumber)
        {

            var customer = _customer.CheckPhoneExist(phoneNumber);
            if (customer == null)
                return BadRequest(new
                ResponeModel()
                {
                    phoneNumber = phoneNumber,
                    statusString = $"{GlobalData.get.resource.phoneNumberNotRegistered}",
                    email = "",
                    statusCode = 0,
                    data = { }
                });
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
                    ResponeModel
                    {
                        phoneNumber = phoneNumber,
                        statusString = $"{GlobalData.get.resource.success}",
                        statusCode = 1,
                        data = { },
                        email = customer.Email
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, (new
                    ResponeModel
                    {
                        phoneNumber = phoneNumber,
                        statusString = $"{GlobalData.get.resource.smsFailedToSend}",
                        statusCode = 0,
                        data = { },
                        email = customer.Email
                    }));

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponeModel
                {
                    phoneNumber = phoneNumber,
                    statusString = ex.Message,
                    statusCode = 0,
                    data = { },
                    email = customer.Email
                });
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
                return BadRequest(new ResponeModel
                {
                    phoneNumber = customer.MobileNumber,
                    email = customer.Email,
                    statusCode = 0,
                    statusString = ""

                });

            if (data.NewPassword.Length < 8)
                ModelState.AddModelError("message", $"{GlobalData.get.resource.newPasswordMustBeAtLeast8CharacterLong}");

            else if (data.NewPassword != data.ConfirmPassword)
                ModelState.AddModelError("message", $"{GlobalData.get.resource.confirmPaswordNotMatch}");

            if (!ModelState.IsValid)
                return BadRequest(new ResponeModel
                {
                    phoneNumber = customer.MobileNumber,
                    email = customer.Email,
                    statusCode = 0,
                    statusString = $"{GlobalData.get.resource.invalidData}",
                    data = ModelState,
                });

            try
            {
                var psw = PasswordHasher.Hash(customer.Email, data.NewPassword);
                customer.Password = psw;
                await _db.SaveChangesAsync();

                return Ok(new ResponeModel
                {
                    phoneNumber = customer.MobileNumber,
                    email = customer.Email,
                    statusCode = 1,
                    statusString = $"{GlobalData.get.resource.passwordDuccessfullyChanged}",
                    data = { },
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponeModel
                {
                    phoneNumber = customer.MobileNumber,
                    email = customer.Email,
                    statusCode = 0,
                    statusString = ex.Message,
                    data = { },
                });
            }
        }

        [Route("register")]
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

            //chek phone exist
            var customer = _customer.CheckPhoneExist(data.MobileNumber);
            if (customer != null)
                return BadRequest("telepon telah terdaftar");

            //chek email exist
            customer = await _db.Customers.FirstOrDefaultAsync(m => m.Email == data.Email);
            if (customer != null)
                return BadRequest("email sudah terdaftar");

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

        [Route("checkphoneexist")]
        [HttpGet]
        public ActionResult CheckPhoneNUmberExist(string phoneNumber)
        {
            try
            {
                var customers = _db.Customers.Where((c) => c.MobileNumber == phoneNumber
               || (!String.IsNullOrEmpty(c.MobileNumber) ? "62" + c.MobileNumber.Substring(1) : "") == phoneNumber).FirstOrDefault();


                return Ok(new
                {
                    customerId = customers.CustomerId,
                    phoneNumber = phoneNumber,
                    statusString = customers != null ? GlobalData.get.resource.phoneNotFound : $"{GlobalData.get.resource.phoneNumberRegistered}",
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
