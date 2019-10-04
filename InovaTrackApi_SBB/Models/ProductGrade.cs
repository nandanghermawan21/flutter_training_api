using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("product_grade")]
    public class ProductGrade
    {
        [Key]
        [Column("grade_code")]
        public string GradeCode { get; set; }
        [Column("grade_name")]
        public string GradeName { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
