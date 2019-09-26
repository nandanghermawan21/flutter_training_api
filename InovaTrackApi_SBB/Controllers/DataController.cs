using InovaTrackApi_SBB.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DataController : ControllerBase
    {
        private ApplicationDbContext _db;

        public DataController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("batching-plants")]
        [HttpGet]
        public ActionResult BatchingPlants()
        {
            var data = _db.BatchingPlants.OrderBy(m => m.BatchingPlantName).ToList();
            return Ok(data);
        }

        [Route("batching-plants/{id}")]
        [HttpGet]
        public ActionResult BatchingPlants(int id)
        {
            var data = _db.BatchingPlants.FirstOrDefault(m => m.BatchingPlantId == id);
            if (data == null)
                return BadRequest(new { message = "Batching Plant not found" });
            return Ok(data);
        }

        [Route("product-slumps")]
        [HttpGet]
        public ActionResult ProductSlumps()
        {
            var data = _db.ProductSlumps.OrderBy(m => m.SlumpCode).ToList();
            return Ok(data);
        }

        [Route("product-slumps/{code}")]
        [HttpGet]
        public ActionResult ProductSlumps(string code)
        {
            var data = _db.ProductSlumps.FirstOrDefault(m => m.SlumpCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Slump not found" });
            return Ok(data);
        }

        [Route("product-grades")]
        [HttpGet]
        public ActionResult ProductGrades()
        {
            var data = _db.ProductGrades.OrderBy(m => m.GradeCode).ToList();
            return Ok(data);
        }

        [Route("product-grades/{code}")]
        [HttpGet]
        public ActionResult ProductGrades(string code)
        {
            var data = _db.ProductGrades.FirstOrDefault(m => m.GradeCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Grade not found" });
            return Ok(data);
        }

        [Route("project-structure-types")]
        [HttpGet]
        public ActionResult ProjectStructureTypes()
        {
            var data = _db.ProjectStructureTypes.OrderBy(m => m.StructureCode).ToList();
            return Ok(data);
        }

        [Route("project-structure-types/{code}")]
        [HttpGet]
        public ActionResult ProjectStructureTypes(string code)
        {
            var data = _db.ProjectStructureTypes.FirstOrDefault(m => m.StructureCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Structure Type not found" });
            return Ok(data);
        }

        [Route("time-slots")]
        [HttpGet]
        public ActionResult TimeSlots()
        {
            var data = _db.SAPTimeSlots.OrderBy(m => m.Time).ToList();
            return Ok(data);
        }

        [Route("time-slots/{id}")]
        [HttpGet]
        public ActionResult TimeSlots(int id)
        {
            var data = _db.SAPTimeSlots.FirstOrDefault(m => m.SlotId == id);
            if (data == null)
                return BadRequest(new { message = "Time Slot not found" });
            return Ok(data);
        }

        [Route("product-materials")]
        [HttpGet]
        public ActionResult ProductMaterials()
        {
            var data = _db.SAPProductMaterials.OrderBy(m => m.SAPMaterial).ToList();
            return Ok(data);
        }

        [Route("product-materials/{id}")]
        [HttpGet]
        public ActionResult ProductMaterials(int id)
        {
            var data = _db.SAPProductMaterials.FirstOrDefault(m => m.MaterialId == id);
            if (data == null)
                return BadRequest(new { message = "Product Material not found" });
            return Ok(data);
        }

        [Route("projects")]
        [HttpGet]
        public ActionResult Projects(string status = null)
        {
            var data = (from a in _db.SAPShipments
                        where status == null ? true : a.Status == status
                        select new
                        {
                            ShipmentNumber = a.ShipmentNumber,
                            ShipmentDate = a.ShipmentDate,
                            ProjectName = a.ProjectName,
                            ShipToName = a.ShipToName,
                            ShipAddress = a.ShipAddress,
                            Status = a.Status
                        }).OrderByDescending(m => m.ShipmentNumber).ToList();
            return Ok(data);
        }

        [Route("projects/{id}")]
        [HttpGet]
        public ActionResult Projects(int id)
        {
            var data = (from a in _db.SAPShipments
                        where a.ShipmentId == id
                        select new
                        {
                            ShipmentNumber = a.ShipmentNumber,
                            ShipmentDate = a.ShipmentDate,
                            ProjectName = a.ProjectName,
                            ShipToName = a.ShipToName,
                            ShipAddress = a.ShipAddress,
                            Status = a.Status
                        }).FirstOrDefault();

            if (data == null)
                return BadRequest(new { message = "Project not found" });
            return Ok(data);
        }

        [Route("shipment-status")]
        [HttpGet]
        public ActionResult ShipmentStatus()
        {
            return Ok(new
            {
                DRAFT = SAPShipment.DRAFT,
                OPEN = SAPShipment.OPEN,
                PAYMENT = SAPShipment.PAYMENT,
                COMPLETED = SAPShipment.COMPLETED
            });
        }

        [Route("payments")]
        [HttpGet]
        public ActionResult Payments()
        {
            var data = _db.SalesPayments.OrderByDescending(m => m.PaymentDate).ToList();
            return Ok(data);
        }

        [Route("payments/{id}")]
        [HttpGet]
        public ActionResult Payments(int id)
        {
            var data = _db.SalesPayments.FirstOrDefault(m => m.PaymentId == id);
            if (data == null)
                return BadRequest(new { message = "Payment not found" });
            return Ok(data);
        }

        [Route("payments/{shipmentNumber}")]
        [HttpGet]
        public ActionResult Payments(string shipmentNumber)
        {
            var data = _db.SalesPayments.Where(m => m.ShipmentNumber == shipmentNumber)
                .OrderByDescending(m => m.PaymentDate).ToList();
            return Ok(data);
        }
    }
}