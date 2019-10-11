using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("vehicles")]
    public class Vehicle
    {
        [Key]
        public int vehicle_id { get; set; }
        public int member_id { get; set; }
        public string registration_number { get; set; }
        public string vehicle_name { get; set; }
        public string vehicle_number { get; set; }
    }
}
