using InovaTrackApi_SBB.DataModel;
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
        private TimeSlotModel _timeSlot;

        public TimeSlotController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _timeSlot = new TimeSlotModel(db, config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult TimeSlots(int? batchingPlantId = null, int? timeSlotId = null)
        {
            var data = _timeSlot.get(batchingPlantId: batchingPlantId, timeSlotId: timeSlotId);
            return Ok(data);
        }


    }
}
