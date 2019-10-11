using GeoCoordinatePortable;
using InovaTrackApi_SBB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class QcLabModel
    {
        #region contructor
        private ApplicationDbContext _db;

        public QcLabModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public QcLabModel()
        {

        }
        #endregion

        public string labCode { get; set; }
        public char? labType { get; set; }
        public string labName { get; set; }
        public string labAddress { get; set; }
        public string labContact { get; set; }
        public string labContactEmail { get; set; }
        public string LabContactPhone { get; set; }
        public double? labLon { get; set; }
        public double? labLat { get; set; }
        public string labRemarks { get; set; }

        public List<QcLabModel> get(string labCode = null, double? lat = null, double? lon = null, int? count = null)
        {
            IQueryable<QcLab> qData = _db.QcLabs;
            if (!string.IsNullOrEmpty(labCode))
            {
                qData = qData.Where(x => x.lab_code == labCode);
            }
            else if (lat.HasValue && lon.HasValue)
            {
                var coord = new GeoCoordinate(lat.Value, lon.Value);
                qData = qData
                    .OrderBy(x => new GeoCoordinate(Convert.ToDouble(x.latitude), Convert.ToDouble(x.longitude))
                    .GetDistanceTo(coord)).Take(count.HasValue ? count.Value : 5);
            }
            var data = (from qcLab in qData
                        select new QcLabModel()
                        {
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
                        }).ToList();
            return data;
        }
    }
}
