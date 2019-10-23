using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using static InovaTrackApi_SBB.DataModel.AuthModel;
using static InovaTrackApi_SBB.DataModel.VisitModel;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private ApplicationDbContext _db;
        private VisitModel _visitModel;

        public VisitController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _visitModel = new VisitModel(db, config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult get(int? visitId = null)
        {
            try
            {
                var data = _visitModel.get(visitId: visitId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("create")]
        [HttpPost]
        public ActionResult create(Visitparam data)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (aktor)
                {
                    case Actor.sales:
                        string salesId = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        var visit = _visitModel.createVisit(data);
                        return Ok(visit);

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("confirm")]
        [HttpGet]
        public ActionResult confirm(long visitId, bool byVisit, double lat, double lon)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (aktor)
                {
                    case Actor.sales:
                        string salesId = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        var visit = _visitModel.confirmVisit(visitId, byVisit, salesId, lat, lon);
                        return Ok(visit);


                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("confirm/data")]
        [HttpPost]
        public ActionResult confirmdata(ConfirmVisitParam data)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (aktor)
                {
                    case Actor.sales:
                        string salesId = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        var visit = _visitModel.confirmVisitData(data, salesId);
                        return Ok(visit);

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
