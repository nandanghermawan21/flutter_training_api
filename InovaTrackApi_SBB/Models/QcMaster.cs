using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("qc_master")]
    public class QcMaster
    {

        [Key]
        public string nik { get; set; }

        public string name { get; set; }

        [Column("join_date")]
        public DateTime? joinDate { get; set; }

        public string address { get; set; }

        [Column("postal_code")]
        public string postalCode { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string password { get; set; }

        public string remarks { get; set; }

        public int? status { get; set; }

        [Column("isdeleted")]
        public bool? isDeleted { get; set; }

        [Column("reset_code")]
        public string resetCode { get; set; }

        [Column("created_by")]
        public string createdBy { get; set; }

        [Column("created_date")]
        public DateTime? createdDate { get; set; }

        [Column("modified_by")]
        public string modifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? modifiedDate { get; set; }

        [NotMapped]
        public string Token { get; set; }

        [NotMapped]
        public DateTime TokenExpiredTime { get; set; }
    }
}
