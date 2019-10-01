using InovaTrackApi_SBB.Model;
using InovaTrackApi_SBB.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ManageController : ControllerBase
    {
        private ApplicationDbContext _db;

        public ManageController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("projects")]
        [HttpPost]
        public ActionResult CreateProject(ProjectModel data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.Email)?.Value);

            try
            {
                var model = new SAPShipment
                {
                    ProjectName = data.ProjectName,
                    ShipToName = data.ShipToName,
                    ShipmentNumber = GenerateShipmentNumber(),
                    ShipAddress = data.ShipAddress,
                    IsMixerAllowed = data.IsMixerAllowed,
                    BatchingPlantId = data.BatchingPlantId,
                    TimeSlotId = data.TimeSlotId,
                    StructureTypeCode = data.StructureTypeCode,
                    GradeCode = data.GradeCode,
                    SlumpCode = data.SlumpCode,
                    OrderQuantity = data.Volume,
                    EstimatedCost = data.EstimatedCost,
                    ShipmentDate = data.ShipmentDate,
                    ShipmentInterval = data.ShipmentInterval,
                    Status = data.Status ? SAPShipment.OPEN : SAPShipment.DRAFT,
                    CreatedTime = DateTime.Now,
                    CreatedBy = userId
                };
                _db.SAPShipments.Add(model);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok("Project created");
        }

        [Route("projects/{id}")]
        [HttpPost]
        public ActionResult UpdateProject(ProjectModel data, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.Email)?.Value);

            var model = _db.SAPShipments.Where(m => m.ShipmentId == id).FirstOrDefault();
            if (model == null)
                return BadRequest("Project not found");

            try
            {
                model.ProjectName = data.ProjectName;
                model.ShipToName = data.ShipToName;
                model.ShipAddress = data.ShipAddress;
                model.IsMixerAllowed = data.IsMixerAllowed;
                model.BatchingPlantId = data.BatchingPlantId;
                model.TimeSlotId = data.TimeSlotId;
                model.StructureTypeCode = data.StructureTypeCode;
                model.GradeCode = data.GradeCode;
                model.SlumpCode = data.SlumpCode;
                model.OrderQuantity = data.Volume;
                model.EstimatedCost = data.EstimatedCost;
                model.ShipmentDate = data.ShipmentDate;
                model.ShipmentInterval = data.ShipmentInterval;
                model.ModifiedTime = DateTime.Now;
                model.ModifiedBy = userId;
                if (model.Status == SAPShipment.DRAFT)
                {
                    model.Status = data.Status ? SAPShipment.OPEN : SAPShipment.DRAFT;
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

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