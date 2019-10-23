using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("gen_bank")]
    public class Bank
    {
      [Key]
      public string bank_code {get; set;}
      public string bank_name {get; set;}
      public string bank_link_code {get; set;}
      public string bank_swift_code {get; set;}
      public char status {get; set;}
      public string created_by {get; set;}
      public DateTime? created_date {get; set;}
      public string modified_by {get; set;}
      public DateTime? modified_date {get; set;}
    }
}
