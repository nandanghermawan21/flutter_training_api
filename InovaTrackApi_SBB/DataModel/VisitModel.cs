using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class VisitModel
    {
        ProjectModel _projectModel;

        #region contructor

        private ApplicationDbContext _db;
        public VisitModel() { }
        public VisitModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _projectModel = new ProjectModel(db, config.Value);
        }

        #endregion

        public List<VisitResponse> get(string salesId = null, bool? isDone = null, long? visitId = null)
        {
            IQueryable<VisitLog> qVisit = _db.VisitLogs;

            if (!string.IsNullOrEmpty(salesId)) qVisit = qVisit.Where(x => x.sales_nik == salesId);

            if (isDone.HasValue) qVisit = qVisit.Where(x => x.is_done == isDone);

            if (visitId.HasValue) qVisit = qVisit.Where(x => x.logid == visitId);


            var data = (from a in qVisit
                        join customer in _db.Customers on a.customer_id equals (int)customer.CustomerId
                        select new VisitResponse
                        {
                            visitId = a.logid,
                            salesId = a.sales_nik,
                            customerId = a.customer_id,
                            projectId = a.project_id,
                            visitDate = a.visit_date,
                            note = a.note,
                            address = a.address,
                            lat = a.lat,
                            lon = a.lon,
                            sourceSalesId = a.source_sales_nik,
                            byVisit = a.by_visit,
                            isDone = a.is_done,
                            confirmVisitDate = a.confirm_visit_date,
                            cofirmVisitNote = a.confirm_visit_note,
                            confirmPercentage = a.confirm_persentage,
                            confirmVisitLat = a.confirm_visit_lat,
                            confirmVisitLon = a.confirm_visit_lon,
                            confirmVisitAddress = a.confirm_visit_address,
                            dataProject = a.dataProject,
                            customerName = customer.CustomerName,
                        }
                        ).ToList();

            return data;
        }

        public VisitResponse createVisit(Visitparam data, string salesSource)
        {
            var visitLog = new VisitLog
            {
                sales_nik = data.salesId,
                source_sales_nik = salesSource,
                customer_id = data.customerId,
                project_id = data.projectId,
                visit_date = data.visitDate,
                note = data.note,
                address = data.address,
                lat = data.lat,
                lon = data.lon,
            };

            _db.VisitLogs.Add(visitLog);

            _db.SaveChanges();

            return get(visitId: visitLog.logid).First();
        }

        public VisitResponse confirmVisit(ConfirmVisit data, string salesId)
        {
            var visitLog = _db.VisitLogs.FirstOrDefault(x => x.logid == data.visitId);

            if (visitLog != null)
            {
                visitLog.by_visit = data.byVisit;
                visitLog.confirm_visit_date = DateTime.Now;
                visitLog.confirm_visit_lat = data.confirmVisitLat;
                visitLog.confirm_visit_lon = data.confirmVisitLon;
                visitLog.confirm_visit_address = data.confirmVisitAddress;
                visitLog.modified_by = salesId;
                visitLog.modified_date = System.DateTime.Now;

                _db.SaveChanges();
                return get(visitId: data.visitId).First();
            }
            else
            {
                return null;
            }
        }

        public VisitResponse confirmVisitData(ConfirmVisiData data, string salesId)
        {
            var visitLog = _db.VisitLogs.FirstOrDefault(x => x.logid == data.visitId);

            if (visitLog != null)
            {
                visitLog.confirm_visit_note = data.confirmNote;
                visitLog.confirm_persentage = data.percentage;
                visitLog.modified_by = salesId;
                visitLog.modified_date = System.DateTime.Now;
                visitLog.dataProject = JsonConvert.SerializeObject(_projectModel.get(id: visitLog.project_id)?.First());

                _db.SaveChanges();
                return get(visitId: data.visitId).First();
            }
            else
            {
                return null;
            }
        }

        public class VisitResponse : Visitparam
        {
            public long visitId { get; set; }
            public string sourceSalesId { get; set; }
            public bool? byVisit { get; set; }
            public bool? isDone { get; set; }
            public DateTime? confirmVisitDate { get; set; }
            public string cofirmVisitNote { get; set; }
            public short? confirmPercentage { get; set; }
            public double? confirmVisitLat { get; set; }
            public double? confirmVisitLon { get; set; }
            public string confirmVisitAddress { get; set; }
            public string dataProject { get; set; }
            public string customerName { get; set; }
            public string projectName { get; set; }
        }

        public class Visitparam
        {
            public string salesId { get; set; }
            public int? customerId { get; set; }
            public string projectId { get; set; }
            public DateTime? visitDate { get; set; }
            public string note { get; set; }
            public string address { get; set; }
            public double? lat { get; set; }
            public double? lon { get; set; }
        }

        public class ConfirmVisit
        {
            public long visitId { get; set; }
            public bool byVisit { get; set; }
            public double? confirmVisitLat { get; set; }
            public double? confirmVisitLon { get; set; }
            public string confirmVisitAddress { get; set; }
        }

        public class ConfirmVisiData
        {
            public long visitId { get; set; }
            public short? percentage { get; set; }
            public string confirmNote { get; set; }
        }


    }
}
