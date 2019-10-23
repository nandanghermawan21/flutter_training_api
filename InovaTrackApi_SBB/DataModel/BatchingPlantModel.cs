using GeoCoordinatePortable;
using InovaTrackApi_SBB.Models;
using System.Collections.Generic;
using System.Linq;

namespace InovaTrackApi_SBB.DataModel
{
    public class BatchingPlantModel : QcLabModel
    {
        #region contructor
        private ApplicationDbContext _db;

        public BatchingPlantModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public BatchingPlantModel()
        {

        }
        #endregion

        #region properties

        public int BatchingPlantId { get; set; }
        public string BatchingPlantName { get; set; }
        public double? BatchingPlantRadius { get; set; }
        public double? BatchingPlantLat { get; set; }
        public double? BatchingPlantLon { get; set; }

        #endregion

        /// <summary>
        /// get batching plant add lat and long to get nearest plant
        /// </summary>
        /// <param name="BatchingPlantId"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public List<BatchingPlantModel> get(int? BatchingPlantId = null, double? lat = null, double? lon = null, int? count = null)
        {
            IQueryable<BatchingPlant> qData = _db.BatchingPlants;
            if (BatchingPlantId.HasValue)
            {
                qData = qData.Where(x => x.BatchingPlantId == BatchingPlantId);
            }
            else if (lat.HasValue && lon.HasValue)
            {
                var coord = new GeoCoordinate(lat.Value, lon.Value);
                qData = qData.Where(m => m.Lat != null && m.Lon != null)
                    .OrderBy(x => new GeoCoordinate(x.Lat.Value, x.Lon.Value)
                    .GetDistanceTo(coord)).Take(count.HasValue ? count.Value : 5);
            }
            var data = (from bachingplant in qData
                        let qcLab = _db.QcLabs.FirstOrDefault(x => bachingplant.BatchingPlantCode == x.lab_code)
                        select new BatchingPlantModel()
                        {
                            BatchingPlantId = bachingplant.BatchingPlantId,
                            BatchingPlantName = bachingplant.BatchingPlantName,
                            BatchingPlantLat = bachingplant.Lat,
                            BatchingPlantLon = bachingplant.Lon,
                            BatchingPlantRadius = bachingplant.radius,
                            labCode = qcLab.lab_code,
                            labName = qcLab.lab_name,
                            labContact = qcLab.lab_contact,
                            labContactEmail = qcLab.contact_email,
                            LabContactPhone = qcLab.contact_phone,
                            labAddress = qcLab.lab_address,
                            labLat = qcLab.latitude,
                            labLon = qcLab.longitude,
                            labType = qcLab.lab_type,
                            labRemarks = qcLab.remarks,
                            qcPrice = qcLab.qc_price,
                        }).ToList();
            return data;
        }




    }
}
