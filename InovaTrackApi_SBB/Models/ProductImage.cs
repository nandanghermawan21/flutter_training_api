using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Models
{
    [Table("product_images")]
    public class ProductImage
    {
        [Key]
        [Column("image_guid")]
        public string imageId { get; set; }
        [Column("material_id")]
        public long productId { get; set; }
        [Column("image_file")]
        public string imageSrc { get; set; }
    }
}
