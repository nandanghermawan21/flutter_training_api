using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("batching_plant")]
    public class BatchingPlant
    {
        [Key]
        [Column("batching_plant_id")]
        public int BatchingPlantId { get; set; }
        [Column("member_id")]
        public int MemberId { get; set; }
        [Column("batching_plant_name")]
        public string BatchingPlantName { get; set; }
        [Column("batching_plant_code")]
        public string BatchingPlantCode { get; set; }
        [Column("lon")]
        public double? Lon { get; set; }
        [Column("lat")]
        public double? Lat { get; set; }
        [Column("radius")]
        public int? radius { get; set; }
        [Column("daily_target_trips")]
        public int? DailyTargetTrips { get; set; }
        [Column("daily_target_volume")]
        public int? DailyTargetVolume { get; set; }
    }
}
