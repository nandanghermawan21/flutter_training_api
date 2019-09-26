using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("geofence")]
    public class Geofence
    {
        [Key]
        public int geofence_id { get; set; }
        public int member_id { get; set; }
        public string geofence_name { get; set; }
        public string geofence_code { get; set; }
        public string geofence_type { get; set; }
        public bool? deleted { get; set; }
    }
}
