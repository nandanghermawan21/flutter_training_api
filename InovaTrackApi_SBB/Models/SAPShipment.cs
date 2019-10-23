using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("sap_shipment")]
    public class SAPShipment
    {
      [Key]
      public string id {get; set;}
      public string shipment_no {get; set;}
      public string project_name {get; set;}
      public int? time_slot_id {get; set;}
      public DateTime? actual_send_date {get; set;}
      public bool? ismixerallowed {get; set;}
      public int? shipment_geofenceid {get; set;}
      public int? batching_plant_id {get; set;}
      public string LdtP_no {get; set;}
      public int? fl_close {get; set;}
      public string src {get; set;}
      public string ref_no1 {get; set;}
      public string ref_no2 {get; set;}
      public string shipto_name {get; set;}
      public string ship_to {get; set;}
      public string shipping_condition {get; set;}
      public string material_number {get; set;}
      public string material_name {get; set;}
      public string preference_truck_type {get; set;}
      public int? max_truck_weight {get; set;}
      public int? order_quantity {get; set;}
      public decimal? unit_pricing {get; set;}
      public decimal? total_order {get; set;}
      public short? shipment_interval {get; set;}
      public int? remaining_quantity_sap {get; set;}
      public int? flag_pecahldt {get; set;}
      public int? fl_fullyload {get; set;}
      public decimal? kirim_quantity {get; set;}
      public int? remaining_quantity_calculation {get; set;}
      public int? fullfilled_percentage {get; set;}
      public DateTime? request_date {get; set;}
      public int? lead_time {get; set;}
      public int? slot_default {get; set;}
      public DateTime? loading_time_awal {get; set;}
      public int? slot_penyesuaian {get; set;}
      public DateTime? loading_time_penyesuaian {get; set;}
      public DateTime? last_update_sap_header {get; set;}
      public DateTime? last_update_sap_detail {get; set;}
      public string debug_remark {get; set;}
      public DateTime? last_update {get; set;}
      public string last_update_by {get; set;}
      public int? shipment_status {get; set;}
      public string structure_type_code {get; set;}
      public string grade_code {get; set;}
      public string slump_code {get; set;}
      public decimal estimated_cost {get; set;}
      public DateTime? created_time {get; set;}
      public string created_by {get; set;}
      public DateTime? modified_time {get; set;}
      public string modified_by {get; set;}

    }
}
