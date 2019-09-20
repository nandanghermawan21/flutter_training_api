using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InovaTrackApi_SBB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InovaTrackApi_SBB.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users.Take(10).Select(p => new
            {
                p.Id,
                p.UserName,
                p.UserFullName
            }).ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var user = db.Users.Where(m => m.Id == id).FirstOrDefault();
            if (user == null)
                return BadRequest(new { message = "User not found" });

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.UserFullName
            });
        }
    }
}