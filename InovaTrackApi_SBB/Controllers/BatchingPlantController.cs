using GeoCoordinatePortable;
using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BatchingPlantController : ControllerBase
    {
        private ApplicationDbContext _db;

        public BatchingPlantController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("get")]
        [HttpGet]
        public ActionResult BatchingPlants()
        {
            var data = _db.BatchingPlants.OrderBy(m => m.BatchingPlantName).ToList();
            return Ok(data);
        }

        [Route("get/{id}")]
        [HttpGet]
        public ActionResult BatchingPlants(int id)
        {
            var data = _db.BatchingPlants.FirstOrDefault(m => m.BatchingPlantId == id);
            if (data == null)
                return BadRequest(new { message = "Batching Plant not found" });
            return Ok(data);
        }

        [Route("get-nearest")]
        [HttpGet]
        public ActionResult GetNearestBatchingPlant(double lat, double lon)
        {
            var coord = new GeoCoordinate(lat, lon);
            var data = _db.BatchingPlants
                .Where(m => m.Lat != null && m.Lon != null)
                .Select(m => new BatchingPlantModel
                {
                    BatchingPlantId = m.BatchingPlantId,
                    BatchingPlantName = m.BatchingPlantName,
                    DistanceKm = Geo.Distance(lat, lon, m.Lat.Value, m.Lon.Value),
                    Lat = m.Lat.Value,
                    Lon = m.Lon.Value
                })
                .OrderBy(x => new GeoCoordinate(x.Lat, x.Lon).GetDistanceTo(coord)).Take(5);

            return Ok(data);
        }
    }
}
