using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB
{
    [Table("qc_lab")]
    public class QcLab
    {
        [Key]
        public string lab_code { get; set; }
        public char lab_type { get; set; }
        public string lab_name { get; set; }
        public string lab_address { get; set; }
        public string lab_contact { get; set; }
        public decimal? qc_price { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public double? longitude { get; set; }
        public double? latitude { get; set; }
        public string remarks { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
