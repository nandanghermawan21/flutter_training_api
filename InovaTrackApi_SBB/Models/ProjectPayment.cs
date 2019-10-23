using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{

    [Table("project_payment")]
    public class ProjectPayment
    {
        [Key]
        public int payment_id { get; set; }
        public string project_id { get; set; }
        public string paid_to_guid_account { get; set; }
        public string paid_from_bank { get; set; }
        public string bank_account_no { get; set; }
        public string bank_account_name { get; set; }
        public string bank_account_branch { get; set; }
        public decimal total_price { get; set; }
        public decimal paid_amount { get; set; }
        public byte? status { get; set; }
        public string note { get; set; }
        public string paid_by { get; set; }
        public DateTime? paid_date { get; set; }
        public string paid_file { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
