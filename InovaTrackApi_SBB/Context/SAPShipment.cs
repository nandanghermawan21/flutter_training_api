using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Context
{
    [Table("sap_shipment_P")]
    public class SAPShipment
    {
        public const string DRAFT = "-1";
        public const string OPEN = "0";
        public const string PAYMENT = "1";
        public const string COMPLETED = "2";

        [Key]
        [Column("id")]
        public int ShipmentId { get; set; }

        [Column("shipment_no")]
        public string ShipmentNumber { get; set; }

        [Column("request_date")]
        public DateTime? ShipmentDate { get; set; }

        [Column("project_name")]
        public string ProjectName { get; set; }

        [Column("shipto_name")]
        public string ShipToName { get; set; }

        [Column("ship_to")]
        public string ShipAddress { get; set; }

        [Column("ismixerallowed")]
        public bool? IsMixerAllowed { get; set; }

        [Column("shipment_geofenceid")]
        public int? ShipGeofenceId { get; set; }

        [Column("batching_plant_id")]
        public int? BatchingPlantId { get; set; }

        [Column("time_slot_id")]
        public int? TimeSlotId { get; set; }

        [Column("structure_type_code")]
        public string StructureTypeCode { get; set; }

        [Column("grade_code")]
        public string GradeCode { get; set; }

        [Column("slump_code")]
        public string SlumpCode { get; set; }

        [Column("estimated_cost")]
        public decimal? EstimatedCost { get; set; }

        [Column("order_quantity")]
        public decimal? OrderQuantity { get; set; }

        [Column("shipment_interval")]
        public Int16? ShipmentInterval { get; set; }

        [Column("shipment_status")]
        public string Status { get; set; }

        [Column("created_time")]
        public DateTime? CreatedTime { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("modified_time")]
        public DateTime? ModifiedTime { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }
    }
}
