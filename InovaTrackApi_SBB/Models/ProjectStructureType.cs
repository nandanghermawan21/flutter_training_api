using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("product_structure_type")]
    public class ProjectStructureType
    {
        [Key]
        [Column("structure_code")]
        public string StructureCode { get; set; }

        [Column("structure_name")]
        public string StructureName { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
