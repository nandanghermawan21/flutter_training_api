using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("sales_visit_log")]
    public class VisitLog
    {
      [Key]
      public long logid {get; set;}
      public string sales_nik {get; set;}
      public int? customer_id {get; set;}
      public string project_id {get; set;}
      public DateTime? visit_date {get; set;}
      public string note {get; set;}
      public string address {get; set;}
      public double lat {get; set;}
      public double lon {get; set;}
      public string source_sales_nik {get; set;}
      public bool? by_visit {get; set;}
      public bool? is_done {get; set;}
      public DateTime? confirm_visit_date {get; set;}
      public string confirm_visit_note {get; set;}
      public short confirm_persentage {get; set;}
      public double? confirm_visit_lat { get; set; }
      public double? confirm_visit_lon { get; set; }
      public string dataProject {get; set;}
      public string created_by {get; set;}
      public DateTime? created_date {get; set;}
      public string modified_by {get; set;}
      public DateTime? modified_date {get; set;}
    }
}
