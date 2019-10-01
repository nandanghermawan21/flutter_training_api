﻿using System.ComponentModel.DataAnnotations;

namespace InovaTrackApi_SBB.Model
{
    public class CustomerLoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
