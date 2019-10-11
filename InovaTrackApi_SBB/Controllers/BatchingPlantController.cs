﻿using InovaTrackApi_SBB.DataModel;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InovaTrackApi_SBB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BatchingPlantController : ControllerBase
    {
        private BatchingPlantModel _bachingPlantModel;

        public BatchingPlantController(ApplicationDbContext db)
        {
            _bachingPlantModel = new BatchingPlantModel(db);
        }

        [Route("get")]
        [HttpGet]
        public ActionResult BatchingPlants(int? bachingPlantId)
        {
            try
            {
                var data = _bachingPlantModel.get(BatchingPlantId: bachingPlantId);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Route("get/{lat}/{lon}")]
        [HttpGet]
        public ActionResult getNearest(double lat, double lon, int? count = null)
        {
            try
            {
                var data = _bachingPlantModel.get(lat: lat, lon: lon, count: count);

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.StackTrace);
            }
        }
    }
}
