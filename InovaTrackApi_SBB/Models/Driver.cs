using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("drivers")]
    public class Driver
    {
        [Key]
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_code { get; set; }
        public int? member_id { get; set; }
        public string sim_number { get; set; }
        public string gsm_number { get; set; }
        public string phone_number { get; set; }
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
        public string driver_guid { get; set; }
        public long update_seq { get; set; }
    }
}
