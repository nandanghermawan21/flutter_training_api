using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;

        public ProductController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("slumps")]
        [HttpGet]
        public ActionResult ProductSlumps()
        {
            var data = _db.ProductSlumps.OrderBy(m => m.SlumpCode).ToList();
            return Ok(data);
        }

        [Route("slumps/{code}")]
        [HttpGet]
        public ActionResult ProductSlumps(string code)
        {
            var data = _db.ProductSlumps.FirstOrDefault(m => m.SlumpCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Slump not found" });
            return Ok(data);
        }

        [Route("grades")]
        [HttpGet]
        public ActionResult ProductGrades()
        {
            var data = _db.ProductGrades.OrderBy(m => m.GradeCode).ToList();
            return Ok(data);
        }

        [Route("grades/{code}")]
        [HttpGet]
        public ActionResult ProductGrades(string code)
        {
            var data = _db.ProductGrades.FirstOrDefault(m => m.GradeCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Grade not found" });
            return Ok(data);
        }

        [Route("structure-types")]
        [HttpGet]
        public ActionResult ProjectStructureTypes()
        {
            var data = _db.ProjectStructureTypes.OrderBy(m => m.StructureCode).ToList();
            return Ok(data);
        }

        [Route("structure-types/{code}")]
        [HttpGet]
        public ActionResult ProjectStructureTypes(string code)
        {
            var data = _db.ProjectStructureTypes.FirstOrDefault(m => m.StructureCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Structure Type not found" });
            return Ok(data);
        }

        [Route("materials")]
        [HttpGet]
        public ActionResult ProductMaterials()
        {
            var data = _db.SAPProductMaterials.OrderBy(m => m.SAPMaterial).ToList();
            return Ok(data);
        }

        [Route("materials/{id}")]
        [HttpGet]
        public ActionResult ProductMaterials(int id)
        {
            var data = _db.SAPProductMaterials.FirstOrDefault(m => m.MaterialId == id);
            if (data == null)
                return BadRequest(new { message = "Product Material not found" });
            return Ok(data);
        }

    }
}
