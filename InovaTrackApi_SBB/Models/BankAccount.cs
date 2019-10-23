using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("gen_bank_account")]
    public class BankAccount
    {
      [Key]
      public string guid_account {get; set;}
      public string bank_code {get; set;}
      public string bank_acc_no {get; set;}
      public string bank_acc_name {get; set;}
      public string branch {get; set;}
      public char? status {get; set;}
      public string created_by {get; set;}
      public DateTime? created_date {get; set;}
      public string modified_by {get; set;}
      public DateTime? modified_date {get; set;}
    }
}
