using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("positions")]
    public class Position
    {
      [Key]
      public string position_id {get; set;}
      public string vehicle_id {get; set;}
      public string date_time {get; set;}
      public string x {get; set;}
      public string y {get; set;}
      public string speed {get; set;}
      public string course {get; set;}
      public string engine {get; set;}
    }
}
