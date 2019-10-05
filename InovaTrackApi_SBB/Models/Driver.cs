using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("drivers")]
    public class Driver
    {
        [Key]
        [Column("driver_id")]
        public int DriverId { get; set; }
        [Column("driver_name")]
        public string DriverName { get; set; }
        [Column("driver_code")]
        public string DriverCode { get; set; }
        [Column("sim_number")]
        public string SimNumber { get; set; }
        [Column("gsm_number")]
        public string GSMNumber{ get; set; }
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("reset_password_code")]
        public string ResetPasswordCode { get; set; }
        [Column("reset_password_expired_time")]
        public DateTime? ResetPasswordExpiredTime { get; set; }
        [Column("create_date")]
        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string TokenExpiredTime { get; set; }
    }
}
