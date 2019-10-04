using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("sap_time_slot")]
    public class SAPTimeSlot
    {
        [Key]
        [Column("slotid")]
        public int SlotId { get; set; }
        [Column("batching_plant_id")]
        public int? BatchingPlantId { get; set; }
        [Column("timeslot")]
        public DateTime Time { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
