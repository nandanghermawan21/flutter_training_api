using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InovaTrackApi_SBB.DataModel
{
    public class ProjectModel
    {
        #region contructor
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProductModel _product;

        public ProjectModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _product = new ProductModel(db, config);
        }

        public ProjectModel()
        {

        }
        #endregion

        #region properties

        public ResponseModel param { get; set; }

        #endregion

        public ProjectModel setDb(ApplicationDbContext db)
        {
            _db = db;
            return this;
        }

        public List<ResponseModel> get(int? customerId = null, string salesId = null, string id = null)
        {
            IQueryable<Project> qData = _db.Projects;

            if (customerId.HasValue) qData = qData.Where(x => x.customer_id == customerId);

            if (!string.IsNullOrEmpty(salesId)) qData = qData.Where(x => x.sales_id == salesId);

            if (!string.IsNullOrEmpty(id)) qData = qData.Where(x => x.id == id);

            var data = (from project in qData
                        join customer in _db.Customers on project.customer_id equals (int)customer.CustomerId
                        join product in _db.SAPProductMaterials on project.product_material_id equals product.MaterialId
                        join timeslots in _db.SAPTimeSlots on project.time_slot_id equals timeslots.SlotId
                        join slump in _db.ProductSlumps on project.slump_code equals slump.SlumpCode
                        join bachingplant in _db.BatchingPlants on project.batching_plant_id equals bachingplant.BatchingPlantId
                        join structure in _db.ProductStructureType on product.StructureCode equals structure.StructureCode
                        join grade in _db.ProductGrades on product.GradeCode equals grade.GradeCode
                        join projectstatus in _db.ProjectStatuses on project.status_id equals projectstatus.id_status
                        let qclab = _db.QcLabs.FirstOrDefault(x => project.lab_code == x.lab_code)
                        let shipmentCount = _db.SAPShipments.Count(x => x.shipment_no == project.sap_shipment_no)
                        let volumeCount = _db.SAPShipments.Where(x => x.shipment_no == project.sap_shipment_no).Sum(x => x.kirim_quantity)
                        let totalshipment = _db.SAPShipments.FirstOrDefault(x => x.shipment_no == project.sap_shipment_no).shipment_interval
                        let paidOf = _db.peojectPayments.Where(x => x.project_id == project.id && x.status == 1).Sum(x => x.paid_amount)
                        select new ResponseModel()
                        {
                            id = project.id,
                            shipmentNo = project.sap_shipment_no,
                            name = project.project_name,
                            shiptoName = project.shipto_name,
                            shiptoAdrress = project.shipto_address,
                            isMixerAllowed = project.ismixerallowed,
                            lat = project.lat,
                            lon = project.lon,
                            timeSlotId = timeslots.SlotId,
                            timeSlot = timeslots.Time,
                            batchingPlantId = project.batching_plant_id,
                            batchingPlantName = bachingplant.BatchingPlantName,
                            batchingPlantLat = bachingplant.Lat,
                            batchingPlantLong = bachingplant.Lon,
                            productId = product.MaterialId,
                            structureTypeCode = product.StructureCode,
                            structureTypeName = structure.StructureName,
                            gradeCode = product.GradeCode,
                            gradeName = grade.GradeName,
                            slumpCode = slump.SlumpCode,
                            slumpName = slump.SlumpName,
                            volume = project.volume,
                            volumeCount = volumeCount ?? 0,
                            labCode = project.lab_code,
                            labName = qclab.lab_name,
                            labAddress = qclab.lab_address,
                            qcPrice = qclab.qc_price,
                            price = project.price,
                            paidOff = paidOf,
                            finalPrice = project.final_price,
                            shipmentDate = project.shipment_date,
                            shipmentInterval = project.shipment_interval,
                            shipmentCount = shipmentCount,
                            totalShipmet = totalshipment,
                            projectStatus = project.status_id,
                            statusString = projectstatus.name,
                            dateTime = project.created_date,
                            source = project.source,
                            customerId = project.customer_id,
                            customerName = customer.CustomerName,
                            customerAvatar = customer.customerAvatar,
                            salesId = project.sales_id,
                        }).ToList();

            return data;
        }

        public void create()
        {

            _db.Projects.Add(new Project
            {
                id = param.id = generateProjectID(),
                sap_shipment_no = param.shipmentNo,
                project_name = param.name,
                shipto_name = param.shiptoName,
                shipto_address = param.shiptoAdrress,
                ismixerallowed = param.isMixerAllowed,
                lat = param.lat,
                lon = param.lon,
                time_slot_id = param.timeSlotId,
                batching_plant_id = param.batchingPlantId,
                product_material_id = param.productId,
                slump_code = param.slumpCode,
                volume = param.volume,
                lab_code = param.labCode,
                price = param.price,
                final_price = param.finalPrice,
                shipment_date = param.shipmentDate,
                shipment_interval = param.shipmentInterval,
                status_id = param.projectStatus.Value,
                source = param.source,
                customer_id = param.customerId,
                sales_id = param.salesId,
                created_date = System.DateTime.Now,
            });

            _db.SaveChanges();
            //return full data after insert
            param = get(id: param.id)[0];
        }

        public void update(string source)
        {
            var project = _db.Projects.FirstOrDefault(x => x.id == param.id);

            if (project != null)
            {
                if (param.shipmentNo != null) project.sap_shipment_no = param.shipmentNo;
                if (param.name != null) project.project_name = param.name;
                if (param.shiptoName != null) project.shipto_name = param.shiptoName;
                if (param.shiptoAdrress != null) project.shipto_address = param.shiptoAdrress;
                if (param.isMixerAllowed != null) project.ismixerallowed = param.isMixerAllowed;
                if (param.lat != null) project.lat = param.lat;
                if (param.lon != null) project.lon = param.lon;
                if (param.timeSlotId != null) project.time_slot_id = param.timeSlotId;
                if (param.batchingPlantId != null) project.batching_plant_id = param.batchingPlantId;
                if (param.productId != null) project.product_material_id = param.productId;
                if (param.slumpCode != null) project.slump_code = param.slumpCode;
                if (param.volume != null) project.volume = param.volume;
                if (param.labCode != null) project.lab_code = param.labCode;
                if (param.price != null) project.price = param.price;
                if (param.finalPrice != null) project.final_price = param.finalPrice;
                if (param.shipmentDate != null) project.shipment_date = param.shipmentDate;
                if (param.shipmentInterval != null) project.shipment_interval = param.shipmentInterval;
                if (param.projectStatus != null) project.status_id = param.projectStatus.Value;
                if (param.customerId != null) project.customer_id = param.customerId;
                if (param.salesId != null) project.sales_id = param.salesId;
                if (param.source != null) project.updated_by = param.source;
                if (param.projectStatus != null) project.status_id = param.projectStatus.Value;
                project.updated_date = System.DateTime.Now;

                _db.SaveChanges();

                param = get(id: param.id)[0];
            }

        }

        public string generateProjectID()
        {
            DateTime now = DateTime.Now;
            int orderToday = _db.Projects.Where(x => x.created_date.Value.Year == now.Year
                                                && x.created_date.Value.Month == now.Month
                                                && x.created_date.Value.Day == now.Day
                                                ).Count();
            return $"{now.ToString("yyyyMMdd")}{(orderToday + 1).ToString().PadLeft(4, '0')}";
        }

        public ProjectModel readParamFromObj(Object data)
        {
            param = JsonConvert.DeserializeObject<ProjectModel.ResponseModel>(JsonConvert.SerializeObject(data));
            return this;
        }

        public class CreateModel
        {
            public string name { get; set; }
            public string shiptoName { get; set; }
            public string shiptoAdrress { get; set; }
            public bool? isMixerAllowed { get; set; }
            public double? lat { get; set; }
            public double? lon { get; set; }
            public int? timeSlotId { get; set; }
            public int? batchingPlantId { get; set; }
            public long? productId { get; set; }
            public string slumpCode { get; set; }
            public string labCode { get; set; }
            public decimal? volume { get; set; }
            public decimal? price { get; set; }
            public decimal? finalPrice { get; set; }
            public DateTime? shipmentDate { get; set; }
            public int? shipmentInterval { get; set; }
        }

        public class UpdateModel : CreateModel
        {
            public string id { get; set; }
        }

        public class ResponseModel : CreateModel
        {
            public string id { get; set; }
            public string shipmentNo { get; set; }
            public TimeSpan timeSlot { get; set; }
            public string batchingPlantName { get; set; }
            public double? batchingPlantLat { get; set; }
            public double? batchingPlantLong { get; set; }
            public string structureTypeCode { get; set; }
            public string structureTypeName { get; set; }
            public string gradeCode { get; set; }
            public string gradeName { get; set; }
            public string slumpName { get; set; }
            public string labName { get; set; }
            public string labAddress { get; set; }
            public decimal? qcPrice { get; set; }
            public decimal? paidOff { get; set; }
            public int shipmentCount { get; set; }
            public decimal volumeCount { get; set; }
            public short? totalShipmet { get; set; }
            public short? projectStatus { get; set; }
            public string statusString { get; set; }
            public DateTime? dateTime { get; set; }
            public string source { get; set; } // c or s
            public int? customerId { get; set; }
            public string customerName { get; set; }
            public string customerAvatar { get; set; }
            public string salesId { get; set; }
        }
    }
}