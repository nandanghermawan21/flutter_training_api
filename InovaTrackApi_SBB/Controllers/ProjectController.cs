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
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static InovaTrackApi_SBB.DataModel.AuthModel;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProjectModel _projectModel;

        public ProjectController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _projectModel = new ProjectModel(db, _config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult Projects(int? customerid = null, string id = null)
        {
            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;
                string salesid = null;

                switch (aktor)
                {
                    case Actor.customer:
                        customerid = int.Parse(User.FindFirst(ClaimTypes.Sid)?.Value);
                        break;

                    case Actor.sales:
                        salesid = User.FindFirst(ClaimTypes.Sid)?.Value;
                        break;
                }

                var data = _projectModel.get(customerId: customerid, salesId: salesid, id: id);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [Route("create")]
        [HttpPost]
        public ActionResult CreateProject(ProjectModel.CreateModel data, short? status = null)
        {
            try
            {
                //claim user to get actor
                var project = new ProjectModel(_db, _config);
                project.readParamFromObj(data);
                project.param.source = User.FindFirst(ClaimTypes.Actor)?.Value;
                switch (project.param.source)
                {
                    case Actor.customer:
                        string salesId = "12121212"; //change this variable to function get related sales
                        project.param.salesId = salesId;
                        project.param.customerId = int.Parse(User.FindFirst(ClaimTypes.Sid)?.Value);
                        break;

                    case Actor.sales:
                        project.param.salesId = User.FindFirst(ClaimTypes.Sid)?.Value;
                        break;

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);
                }

                if (status.HasValue)
                {
                    project.param.projectStatus = status.Value;
                }

                project.create();

                return Ok(project.param);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("update")]
        [HttpPost]
        public ActionResult UpdateProject(ProjectModel.UpdateModel data, short? status = null)
        {
            try
            {
                //claim user to get actor
                string actor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (actor)
                {
                    case Actor.customer:

                        break;

                    case Actor.sales:

                        break;

                    default:
                        return BadRequest(GlobalData.get.resource.thisUserIsNotPermitted);
                }


                //claim user to get actor
                var project = new ProjectModel(_db, _config);
                project.readParamFromObj(data);


                if (status.HasValue)
                {
                    project.param.projectStatus = status.Value;
                }

                project.update(actor);

                return Ok(project.param);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
