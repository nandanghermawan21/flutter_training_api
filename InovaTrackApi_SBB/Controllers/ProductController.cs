using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProductModel _product;

        public ProductController(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _product = new ProductModel(db, config);
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
            var data = _db.ProductStructureType.OrderBy(m => m.StructureCode).ToList();
            return Ok(data);
        }

        [Route("structure-types/{code}")]
        [HttpGet]
        public ActionResult ProjectStructureTypes(string code)
        {
            var data = _db.ProductStructureType.FirstOrDefault(m => m.StructureCode == code);
            if (data == null)
                return BadRequest(new { message = "Product Structure Type not found" });
            return Ok(data);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult ProductMaterials(long? id = null)
        {
            try
            {
                var data = _product.get(id: id, imageIncluded: true);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("get/by-structure/{structureCode}")]
        [HttpGet]
        public ActionResult ProductByStructurType(string structureCode)
        {
            try
            {
                var data = _product.get(structureCode: structureCode, imageIncluded: false);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}
