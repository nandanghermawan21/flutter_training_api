using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("project")]
    public class Project
    {
        [Key]
        public string id { get; set; }
        public string sap_shipment_no { get; set; }
        public string project_name { get; set; }
        public string shipto_name { get; set; }
        public string shipto_address { get; set; }
        public bool? ismixerallowed { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        public int? time_slot_id { get; set; }
        public int? batching_plant_id { get; set; }
        public long? product_material_id { get; set; }
        public string slump_code { get; set; }
        public decimal? volume { get; set; }
        public string lab_code { get; set; }
        public decimal? price { get; set; }
        public decimal? final_price { get; set; }
        public DateTime? shipment_date { get; set; }
        public int? shipment_interval { get; set; }
        public short status_id { get; set; }
        public string source { get; set; }
        public int? customer_id { get; set; }
        public string sales_id { get; set; }
        public string updated_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }

        //public Projectsource projectSource
        //{
        //    get
        //    {
        //        Projectsource value;
        //        switch (source)
        //        {
        //            case "C":
        //                value = Projectsource.C;
        //                break;
        //            case "S":
        //                value = Projectsource.S;
        //                break;
        //            default:
        //                value = Projectsource.N;
        //                break;
        //        }
        //        return value;
        //    }
        //}



    }


    public static class ProjectStatus
    {
        public static short Draft = 0;
        public static short Open = 1;
        public static short Payment = 2;
        public static short Shimpent = 3;
        public static short Finish = 4;
    }


}
