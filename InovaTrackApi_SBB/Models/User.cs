using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int Id { get; set; }
        [Column("user_full_name")]
        public string UserFullName { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }
    }
}
