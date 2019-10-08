using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("project_status")]
    public class ProjectsStatus
    {
        [Key]
        public short id_status { get; set; }
        public string name {get; set;}
        public string group { get; set; }
        public string ordering {get; set;}
        public string created_by {get; set;}
        public DateTime created_date {get; set;}
        public string modified_by {get; set;}
        public DateTime modified_date {get; set;}
    }
}
