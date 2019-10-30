using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class ProductModel
    {

        #region contructor

        private ApplicationDbContext _db;
        private AppSettings _config;

        public ProductModel(ApplicationDbContext db,AppSettings config)
        {
            _db = db;
            _config = config;
        }

        #endregion

        #region properties

        public int id { get; set; }
        public string quality { get; set; }
        public string qualityName { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }
        public string structureCode { get; set; }
        public string structureName { get; set; }

        #endregion

        public List<ProductModel> get(long? id = null, string structureCode = null, bool imageIncluded = false)
        {
            IQueryable<SAPProductMaterial> qData = _db.SAPProductMaterials;

            if (id.HasValue) qData = qData.Where(x => x.MaterialId == id);

            if (!string.IsNullOrEmpty(structureCode)) qData = qData.Where(x => x.StructureCode == structureCode);

            var data = (from a in qData
                        join b in _db.ProductStructureType on a.StructureCode equals b.StructureCode
                        join c in _db.ProductGrades on a.GradeCode equals c.GradeCode
                        let img = imageIncluded ? _db.ProductImages.FirstOrDefault((x) => x.productId == a.MaterialId) : null
                        select new ProductModel(_db,_config)
                        {
                            id = (int)a.MaterialId,
                            description = a.Description,
                            price = a.Price,
                            quality = a.GradeCode,
                            qualityName = c.GradeName,
                            structureCode = a.StructureCode,
                            structureName = b.StructureName,
                            imageUrl = img != null ? img.imageSrc : string.Empty
                        }
                    ).ToList();
            return (data);
        }
    }
}
