using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeSlotController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;

        public TimeSlotController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("get")]
        [HttpGet]
        public ActionResult TimeSlots()
        {
            var data = _db.SAPTimeSlots.OrderBy(m => m.Time).ToList();
            return Ok(data);
        }

        [Route("get/{id}")]
        [HttpGet]
        public ActionResult TimeSlots(int id)
        {
            var data = _db.SAPTimeSlots.FirstOrDefault(m => m.SlotId == id);
            if (data == null)
                return BadRequest(new { message = "Time Slot not found" });
            return Ok(data);
        }
    }
}
