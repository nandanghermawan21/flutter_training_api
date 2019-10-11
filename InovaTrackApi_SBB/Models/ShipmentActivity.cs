﻿using InovaTrackApi_SBB.Helper;
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
        public String pod_recipient_name { get; set; }
        public DateTime? pod_time { get; set; }
        public string pod_file1 { get; set; }
        public string pod_file2 { get; set; }
        public DateTime? returning_time { get; set; }
        public DateTime? available_time { get; set; }
        public bool is_emergency { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }

        [NotMapped]
        public int statusId { get; set; }

        [NotMapped]
        public string status
        {
            get
            {
                string sts = GlobalData.get.resource.waiting;
                statusId = 0;
                if (begin_loading_time.HasValue)
                {
                    sts = GlobalData.get.resource.loading;
                    statusId = 1;
                }
                if (leave_plant_time.HasValue)
                {
                    sts = GlobalData.get.resource.leavingPlant;
                    statusId = 2;
                }
                if (arrival_time.HasValue)
                {
                    sts = GlobalData.get.resource.arriving;
                    statusId = 3;
                }
                if (begin_unloading_time.HasValue)
                {
                    sts = GlobalData.get.resource.unloading;
                    statusId = 4;
                }
                if (unloading_by_driver.HasValue)
                {
                    sts = GlobalData.get.resource.unloading;
                    statusId = 5;
                }
                if (returning_time.HasValue)
                {
                    sts = GlobalData.get.resource.completed;
                    statusId = 6;
                }
                return sts;
            }
        }
    }


}