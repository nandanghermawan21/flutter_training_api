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

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;

        public ProjectController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("get")]
        [HttpGet]
        public ActionResult Projects(string status = null)
        {
            return Ok("");
        }

        [Route("get/{id}")]
        [HttpGet]
        public ActionResult Projects(int id)
        {
            return Ok("data");
        }

        [Route("create")]
        [HttpPost]
        public ActionResult CreateProject(ProjectModel data)
        {
            return Ok("Project created");
        }

        [Route("update/{id}")]
        [HttpPost]
        public ActionResult UpdateProject(ProjectModel data, int id)
        {
            return Ok("Project updated");
        }

        private string GenerateShipmentNumber()
        {
            int index = 1;
            string lft = "SBI-" + DateTime.Today.ToString("yyyyMM");
            string rgt = index.ToString().PadLeft(5, '0');
            string shipmentNumber = lft + rgt;
            while (_db.SAPShipments.Where(m => m.ShipmentNumber == shipmentNumber).FirstOrDefault() != null)
            {
                index++;
                rgt = index.ToString().PadLeft(5, '0');
                shipmentNumber = lft + rgt;
            }
            return shipmentNumber;
        }
    }
}
