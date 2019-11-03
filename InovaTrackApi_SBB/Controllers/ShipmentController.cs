using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static InovaTrackApi_SBB.DataModel.AuthModel;
using static InovaTrackApi_SBB.DataModel.ShipmentModel;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ShipmentModel _shipmentModel;

        public ShipmentController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _shipmentModel = new ShipmentModel(db, config);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult Get(string projectId = null, int? shipmentId = null, int? driverId = null)
        {
            try
            {
                var data = _shipmentModel.get(projectId: projectId, shipmentId: shipmentId, driverId: driverId);

                return Ok(data.ToList());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("summary")]
        [HttpGet]
        public ActionResult Summary(string projectId = null, int? shipmentId = null, int? driverId = null)
        {


            try
            {
                //claim user to get actor
                var aktor = User.FindFirst(ClaimTypes.Actor)?.Value;

                switch (aktor)
                {
                    //override driver id if login by driver
                    case Actor.driver:
                        driverId = int.Parse((User.FindFirst(ClaimTypes.Sid)?.Value));
                        break;
                }

                var data = _shipmentModel.getSummary(projectId: projectId, shipmentId: shipmentId, driverId: driverId).OrderByDescending(x => x.datetime);

                return Ok(data);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("status")]
        [HttpGet]
        public ActionResult Status(string projectId = null, int? shipmentId = null)
        {
            try
            {
                var data = _shipmentModel.getStatus(projectId: projectId, shipmentId: shipmentId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("status/update")]
        [HttpPost]
        public ActionResult UpdateStatus(ShipmentStatus shipmentStatus, int? mode = 2)
        {
            try
            {
                var data = _shipmentModel.updateStatus(shipmentStatus, mode: mode);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("status/confirm")]
        [HttpPost]
        public ActionResult UpdateConfirm(ShipmentConfirm ShipmentConfirm)
        {
            try
            {
                var data = _shipmentModel.updateStatus(JsonConvert.DeserializeObject<ShipmentStatus>(JsonConvert.SerializeObject(ShipmentConfirm)), mode: 2, imageInclude: true, returnMode: ReturnMode.Summary);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("pod")]
        [HttpPost]
        public ActionResult Pod(Pod podData)
        {
            try
            {
                var data = _shipmentModel.updateStatus(JsonConvert.DeserializeObject<ShipmentStatus>(JsonConvert.SerializeObject(podData)),
                    mode: 2,
                    imageInclude: true);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("emergency/get")]
        [HttpGet]
        public ActionResult GetEmergency(int? shipmentId = null, string emergencyId = null)
        {
            try
            {
                var data = _shipmentModel.getEmergency(shipmentId, emergencyId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("emergency/add")]
        [HttpPost]
        public ActionResult AddEmergency(EmergencyInput emergency)
        {
            try
            {
                var data = _shipmentModel.addEmmergency(emergency);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("position/current")]
        [HttpGet]
        public ActionResult getCurrentPosition(int shipmentId)
        {
            try
            {
                var data = _shipmentModel.getCurrent(shipmentId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("position/history")]
        [HttpGet]
        public ActionResult getHistoryPosition(int shipmentId)
        {
            try
            {
                var data = _shipmentModel.getHistory(shipmentId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [Route("driver")]
        [HttpGet]
        public ActionResult getDriverShipment(int driverId)
        {
            try
            {
                IQueryable<ShipmentDetail> qDetail = _shipmentModel.get();

                var data = (from a in qDetail where a.driverId == driverId select a.shipmentSummary).ToList();

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
