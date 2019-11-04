using InovaTrackApi_SBB.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("delivery_activity")]
    public class ShipmentActivity
    {
        [Key]
        public int id { get; set; }
        public string ticket_number { get; set; }
        public int? time_slot_id { get; set; }
        public int? batching_plant_id { get; set; }
        public int? kiriman_ke { get; set; }
        public int? interval_kiriman { get; set; }
        public int? volume { get; set; }
        public string truck_number { get; set; }
        public DateTime? begin_loading_time { get; set; }
        public DateTime? leave_plant_time { get; set; }
        public DateTime? arrival_time { get; set; }
        public DateTime? begin_unloading_time { get; set; }
        public DateTime? unloading_by_driver { get; set; }
        public decimal? unloading_by_driver_lat { get; set; }
        public decimal? unloading_by_driver_lon { get; set; }
        public String pod_recipient_name { get; set; }
        public DateTime? pod_time { get; set; }
        public string pod_file1 { get; set; }
        public string pod_file2 { get; set; }
        public DateTime? returning_time { get; set; }
        public DateTime? available_time { get; set; }
        public bool? is_emergency { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public byte confirm_status { get; set; }
        public string confirm_note { get; set; }
        public DateTime? confirm_date { get; set; }
        public byte? rating { get; set; }
        public string rating_note { get; set; }
        public DateTime? rating_date { get; set; }

        [NotMapped]
        public int statusId { get; set; }

        [NotMapped]
        public string status { get; set; }

        public ShipmentActivity readStatus()
        {
            status = GlobalData.get.resource.waiting;
            statusId = 0;
            if (confirm_date.HasValue)
            {
                status = GlobalData.get.resource.confirmed;
                statusId = 1;
            }
            if (begin_loading_time.HasValue)
            {
                status = GlobalData.get.resource.loading;
                statusId = 2;
            }
            if (leave_plant_time.HasValue)
            {
                status = GlobalData.get.resource.leavingPlant;
                statusId = 3;
            }
            if (arrival_time.HasValue)
            {
                status = GlobalData.get.resource.arriving;
                statusId = 4;
            }
            if (begin_unloading_time.HasValue)
            {
                status = GlobalData.get.resource.unloading;
                statusId = 5;
            }
            if (unloading_by_driver.HasValue)
            {
                status = GlobalData.get.resource.unloading;
                statusId = 6;
            }
            if (pod_time.HasValue)
            {
                status = GlobalData.get.resource.pod;
                statusId = 7;
            }
            if (returning_time.HasValue)
            {
                status = GlobalData.get.resource.returnning;
                statusId = 8;
            }
            if (available_time.HasValue)
            {
                status = GlobalData.get.resource.completed;
                statusId = 9;
            }
            return this;
        }
    }


}
