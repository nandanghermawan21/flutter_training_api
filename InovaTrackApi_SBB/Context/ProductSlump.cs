using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Context
{
    [Table("product_slump")]
    public class ProductSlump
    {
        [Key]
        [Column("slump_code")]
        public string SlumpCode { get; set; }

        [Column("slump_name")]
        public string SlumpName { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
