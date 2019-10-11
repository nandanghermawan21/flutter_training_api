using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("vehicle_drivers")]
    public class VehicleDriver
    {
     [Key]
     public int vehicle_id {get; set;}
     public int driver_id {get; set;}
     public DateTime? start_time {get; set;}
     public DateTime? end_time {get; set;}
    }
}
