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
        public int driverId { get; set; }
        [Column("driver_name")]
        public string driverName { get; set; }
        [Column("driver_code")]
        public string driverCode { get; set; }
        [Column("member_id")]
        public int? memberId { get; set; }
        [Column("sim_number")]
        public string simNumber { get; set; }
        [Column("gsm_number")]
        public string gsmNumber { get; set; }
        [Column("phone_number")]
        public string phoneNumber { get; set; }
        public bool phone_number_confirmed { get; set; }
        public int? status { get; set; }
        public DateTime? last_login_time { get; set; }
        public string udid { get; set; }
        public string registration_code { get; set; }
        public string pin_hash { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string reset_password_code { get; set; }
        public DateTime? reset_password_expired_time { get; set; }
        public DateTime? created_date { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public DateTime TokenExpiredTime { get; set; }
    }
}
