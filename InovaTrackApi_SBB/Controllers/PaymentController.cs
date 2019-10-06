using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly AppSettings _config;

        public PaymentController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("get-all")]
        [HttpGet]
        public ActionResult Payments()
        {
            var data = _db.SalesPayments.OrderByDescending(m => m.PaymentDate).ToList();
            return Ok(data);
        }

        [Route("get/{id}")]
        [HttpGet]
        public ActionResult Payments(int id)
        {
            var data = _db.SalesPayments.FirstOrDefault(m => m.PaymentId == id);
            if (data == null)
                return BadRequest(new { message = "Payment not found" });
            return Ok(data);
        }

        [Route("get-by-shipment/{shipmentNumber}")]
        [HttpGet]
        public ActionResult Payments(string shipmentNumber)
        {
            var data = _db.SalesPayments.Where(m => m.ShipmentNumber == shipmentNumber)
                .OrderByDescending(m => m.PaymentDate).ToList();
            return Ok(data);
        }
    }
}
