using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("delivery_emergency")]
    public class ShipmentEmergency
    {
        [Key]
        public string emergency_file_guid { get; set; }
        public int? delivery_id { get; set; }
        public string ticket_number { get; set; }
        public string truck_number { get; set; }
        public DateTime emergency_time { get; set; }
        public string emergency_note { get; set; }
        public string emergency_file { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
