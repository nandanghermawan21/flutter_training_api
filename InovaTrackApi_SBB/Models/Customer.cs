using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        [Column("cust_id")]
        public long CustomerId { get; set; }

        [Column("customer_email")]
        public string Email { get; set; }

        [Column("customer_password")]
        public string Password { get; set; }

        [Column("customer_number")]
        public string CustomerNumber { get; set; }

        [Column("customer_name1")]
        public string CustomerName { get; set; }

        [Column("customer_name3")]
        public string Address1 { get; set; }

        [Column("customer_name4")]
        public string Address2 { get; set; }

        [Column("customer_mobile")]
        public string MobileNumber { get; set; }

        [Column("is_verified_account")]
        public bool? isVerifiedAccount { get; set; }

        [Column("customer_avatar")]
        public string customerAvatar { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [Column("reset_password_code")]
        public string ResetPasswordCode { get; set; }

        [Column("reset_password_expired_time")]
        public DateTime? ResetPasswordExpiredTime { get; set; }

        [Column("create_date")]
        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public DateTime TokenExpiredTime { get; set; }
    }
}
