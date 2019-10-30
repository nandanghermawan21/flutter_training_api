using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class TimeSlotModel
    {
        #region contructor
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProductModel _product;

        public TimeSlotModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _product = new ProductModel(db, config.Value);
        }

        public TimeSlotModel()
        {

        }
        #endregion

        public List<SAPTimeSlot> get(int? batchingPlantId = null, int? timeSlotId = null)
        {
            IQueryable<SAPTimeSlot> qList = _db.SAPTimeSlots;
            if (batchingPlantId.HasValue) qList = qList.Where(x => x.BatchingPlantId == batchingPlantId);
            if (timeSlotId.HasValue) qList = qList.Where(x => x.SlotId == timeSlotId);
            var data = qList.ToList();
            return data;
        }
    }
}
