using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("sales_master")]
    public class Sales
    {
        [Key]
        [Column("sales_nik")]
        public string salesId { get; set; }
        [Column("sales_name")]
        public string salesName { get; set; }
        [Column("join_date")]
        public DateTime? joinDate { get; set; }
        [Column("address")]
        public string address { get; set; }
        [Column("postal_code")]
        public string postalCode { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("password")]
        public string password { get; set; }
        [Column("phone")]
        public string phone { get; set; }
        [Column("remarks")]
        public string remarks { get; set; }
        [Column("status")]
        public string status { get; set; }
        public bool? isdeleted { get; set; }
        [Column("reset_code")]
        public string resetCode { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public DateTime TokenExpiredTime { get; set; }
    }
}
