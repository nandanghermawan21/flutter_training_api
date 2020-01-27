using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterTraining.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username {get; set;}
        public string password {get; set;}
        public string name {get; set;}
        public string phone {get; set;}
        public string address {get; set;}
        [NotMapped]
        public string token { get; set; }
    }
}
