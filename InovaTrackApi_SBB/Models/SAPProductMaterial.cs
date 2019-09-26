using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("sap_product_material")]
    public class SAPProductMaterial
    {
        [Key]
        [Column("material_id")]
        public long MaterialId { get; set; }

        [Column("sap_material")]
        public long? SAPMaterial { get; set; }

        [Column("material_description")]
        public string Description { get; set; }

        [Column("mtype")]
        public string Type { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("grade_code")]
        public string GradeCode { get; set; }

        [Column("structure_code")]
        public string StructureCode { get; set; }
    }
}
