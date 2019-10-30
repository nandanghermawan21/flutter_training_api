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
        private ProjectModel _projectModel;

        public VisitController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _visitModel = new VisitModel(db, config);
            _projectModel = new ProjectModel(db, config.Value);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult get(int? visitId = null, string salesId = null)
        {
            try
            {
                var data = _visitModel.get(visitId: visitId, salesId: salesId);
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
                        string salesSource = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        var visit = _visitModel.createVisit(data, salesSource);
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
        [HttpPost]
        public ActionResult confirm(ConfirmVisit data)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (aktor)
                {
                    case Actor.sales:
                        string salesId = (User.FindFirst(ClaimTypes.Sid)?.Value);
                        var visit = _visitModel.confirmVisit(data, salesId);
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
        public ActionResult confirmdata(ConfirmVisiData data)
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
