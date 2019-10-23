using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private ApplicationDbContext _db;
        private PaymentModel _paymentModel;

        public PaymentController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _paymentModel = new PaymentModel(db, config);
        }


        [Route("get")]
        [HttpGet]
        public ActionResult get(string projectId, int? paymentId = null, bool? includeImmage = null)
        {
            try
            {
                var data = _paymentModel.get(projectId: projectId, includeImmage: includeImmage ?? false, paymentId: paymentId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("account")]
        [HttpGet]
        public ActionResult account(string accountId = "")
        {
            try
            {
                var data = _paymentModel.GetBankAccounts(accoundId: accountId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("create")]
        [HttpPost]
        public ActionResult create(PaymentModel.PaymentParam data)
        {
            try
            {
                var payment = _paymentModel.create(data);
                return Ok(payment);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
