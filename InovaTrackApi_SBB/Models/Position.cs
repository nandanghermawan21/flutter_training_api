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
        public long position_id { get; set; }
        public int? vehicle_id { get; set; }
        public DateTime? date_time { get; set; }
        public int? x { get; set; }
        public int? y { get; set; }
        public short? speed { get; set; }
        public short? course { get; set; }
        public bool? engine { get; set; }
        [NotMapped]
        public double? lat
        {
            get
            {
                return 1e-7 * y;
            }
        }
        [NotMapped]
        public double? lon
        {
            get
            {
                return 1e-7 * x;
            }
        }
    }
}
