using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("sales_customer")]
    public class SalesCustomer
    {   
        [Key]
        public string sales_nik { get; set; }
        public long cust_id { get; set; }
    }
}
