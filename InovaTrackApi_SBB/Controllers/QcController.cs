using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcController : ControllerBase
    {
        private QcLabModel _qcLabModel;

        public QcController(ApplicationDbContext db)
        {
            _qcLabModel = new QcLabModel(db);
        }

        [Route("lab")]
        [HttpGet]
        public ActionResult Lab(string labCode)
        {
            try
            {
                var data = _qcLabModel.get(labCode: labCode);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("lab/{lat}/{lon}")]
        [HttpGet]
        public ActionResult labNeares(double? lat = null, double? lon = null, int? count = null)
        {
            try
            {
                var data = _qcLabModel.get(lat: lat, lon: lon, count: count);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.StackTrace);
            }
        }
    }
}
