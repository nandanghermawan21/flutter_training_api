using GeoCoordinatePortable;
using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private ApplicationDbContext _db;

        public QueryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("get-nearest-batching-plant")]
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