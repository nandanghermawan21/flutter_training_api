﻿using InovaTrackApi_SBB.DataModel;
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
        private CustomerModel _customer;


        public AccountController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _customer = new CustomerModel(db, config);
        }

        [Route("customer-forgot-password")]
        [HttpPost]
        public async Task<ActionResult> CustomerForgotPasswordAsync(string phoneNumber)
        {

            var customer = _customer.CheckPhoneExist(phoneNumber);
            if (customer == null)
                return BadRequest(new
                ResponeModel()
                {
                    phoneNumber = phoneNumber,
                    statusString = "no telepon tidak terdaftar",
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
                    content = $"Berikut kode untuk reset password anda {code} mohon tidak memberitahukan kode ini kepada siapapun",
                });

                if (a != null)
                {
                    return Ok(new
                    ResponeModel
                    {
                        phoneNumber = phoneNumber,
                        statusString = "success",
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
                        statusString = "gagal mengirim sms",
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

        [Route("customer-reset-password")]
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
                    statusString = "pelanggan tidak ditemukan"

                });

            if (data.NewPassword.Length < 8)
                ModelState.AddModelError("message", "Password baru minimal 8 karakter");

            else if (data.NewPassword != data.ConfirmPassword)
                ModelState.AddModelError("message", "Konfirmasi password tidak sama");

            if (!ModelState.IsValid)
                return BadRequest(new ResponeModel
                {
                    phoneNumber = customer.MobileNumber,
                    email = customer.Email,
                    statusCode = 0,
                    statusString = "data tidak valid",
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
                    statusString = "password berhasil diubah",
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

        [Route("customer-checkphoneexist")]
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
                    statusString = customers != null ? "exist" : "not Exist",
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