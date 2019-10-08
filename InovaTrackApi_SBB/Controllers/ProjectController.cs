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
            _projectModel = new ProjectModel(db, config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult Projects(int? customerid = null, string id = null)
        {
            try
            {
                //claim user to get actor
                var source = User.FindFirst(ClaimTypes.Actor)?.Value;
                string salesid = null;

                switch (source)
                {
                    case Projectsource.customer:
                        customerid = int.Parse(User.FindFirst(ClaimTypes.Email)?.Value);
                        break;

                    case Projectsource.sales:
                        salesid = User.FindFirst(ClaimTypes.Email)?.Value;
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
        public ActionResult CreateProject(ProjectModel.CreateModel data, int? customerId = null, short? status = null)
        {
            try
            {
                //claim user to get actor
                var project = new ProjectModel();
                project.readParamFromObj(data);
                project.param.source = User.FindFirst(ClaimTypes.Actor)?.Value;
                switch (project.param.source)
                {
                    case Projectsource.customer:
                        project.param.customerId = int.Parse(User.FindFirst(ClaimTypes.Email)?.Value);
                        break;

                    case Projectsource.sales:
                        project.param.salesId = User.FindFirst(ClaimTypes.Email)?.Value;
                        project.param.customerId = customerId;
                        break;
                }

                if (status.HasValue)
                {
                    project.param.projectStatus = status.Value;
                }

                project.setDb(_db).create();

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

                //claim user to get actor
                var project = new ProjectModel();
                project.readParamFromObj(data).setDb(_db).update(actor);

                if (status.HasValue)
                {
                    project.param.projectStatus = status.Value;
                }

                return Ok(project.param);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
