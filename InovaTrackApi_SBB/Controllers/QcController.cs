using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using static InovaTrackApi_SBB.DataModel.AuthModel;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcController : ControllerBase
    {
        private QcLabModel _qcLabModel;
        private QcModel _qcModel;

        public QcController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _qcLabModel = new QcLabModel(db);
            _qcModel = new QcModel(db, config.Value);
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

        [Route("get")]
        [HttpGet]
        public ActionResult get()
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;
                string qcNik = null;

                switch (aktor)
                {
                    case Actor.qc:
                        qcNik = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        break;

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }

                var data = _qcModel.get(qcNik: qcNik).ToList();

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("updateHeader")]
        [HttpPost]
        public ActionResult UpdateHeader(QcHeader qcHeader)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;
                string qcNik = null;

                switch (aktor)
                {
                    case Actor.qc:
                        qcNik = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        break;

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }

                var data = _qcModel.UpdateHeader(qcHeader);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [Route("updateDetail")]
        [HttpPost]
        public ActionResult UpdateDetail(QcDetail qcDetail)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;
                string qcNik = null;

                switch (aktor)
                {
                    case Actor.qc:
                        qcNik = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        break;

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }

                var data = _qcModel.updateDetail(qcDetail);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
