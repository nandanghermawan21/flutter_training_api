using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Controllers
{
    public class ShipmentController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProjectModel _projectModel;

        public ShipmentController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _projectModel = new ProjectModel(db, config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult Shipment(string projectId = null, string shipmentNo = null, string driverId = null)
        {
            return (Ok());
        }

        [Route("confirm")]
        [HttpGet]
        public ActionResult Confirm(String shipmentId)
        {
            //hanya driver yang boleh lakukan confirm jika yang lain masuk sini return badrequest authentication error
            return (Ok());
        }


        [Route("emergency")]
        [HttpGet]
        public ActionResult Emergency(ShipmentModel.EmergencyModel data)
        {
            //hanya driver yang boleh lakukan confirm jika yang lain masuk sini return badrequest authentication error
            return (Ok());
        }


        [Route("pod")]
        [HttpGet]
        public ActionResult Pod(ShipmentModel.PodModel data)
        {
            //hanya driver yang boleh lakukan confirm jika yang lain masuk sini return badrequest authentication error
            return (Ok());
        }

    }
}
