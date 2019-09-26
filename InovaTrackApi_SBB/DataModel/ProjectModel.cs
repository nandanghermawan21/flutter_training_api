using System;
using System.ComponentModel.DataAnnotations;

namespace InovaTrackApi_SBB.DataModel
{
    public class ProjectModel
    {
        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ShipToName { get; set; }

        [Required]
        public string ShipAddress { get; set; }

        public bool? IsMixerAllowed { get; set; }

        [Required]
        public int BatchingPlantId { get; set; }

        [Required]
        public int TimeSlotId { get; set; }

        [Required]
        public string StructureTypeCode { get; set; }

        [Required]
        public string GradeCode { get; set; }

        [Required]
        public string SlumpCode { get; set; }

        [Required]
        public int Volume { get; set; }

        [Required]
        public decimal EstimatedCost { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        [Required]
        public Int16 ShipmentInterval { get; set; }
        public bool Status { get; set; }
    }
}
