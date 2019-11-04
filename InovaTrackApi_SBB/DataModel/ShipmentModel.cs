using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class ShipmentModel
    {
        #region contructor
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProductModel _product;

        public ShipmentModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _product = new ProductModel(db, config.Value);
        }

        //public ShipmentModel()
        //{

        //}
        #endregion

        #region parameter

        #endregion

        public IQueryable<ShipmentDetail> get(string projectId = null, int? shipmentId = null, int? driverId = null, bool? imageInclude = false)
        {
            IQueryable<Project> qProject = _db.Projects;
            IQueryable<ShipmentActivity> qShipmentAct = _db.ShipmentActivities;
            IQueryable<Driver> qDriver = _db.Drivers;

            if (!string.IsNullOrEmpty(projectId)) qProject = qProject.Where(x => x.id == projectId);

            if (shipmentId.HasValue) qShipmentAct = _db.ShipmentActivities.Where(x => x.id == shipmentId);

            if (driverId.HasValue) qDriver = _db.Drivers.Where(x => x.driverId == driverId);


            var data = (from project in qProject
                        join customer in _db.Customers on project.customer_id equals (int)customer.CustomerId
                        join batchingPlant in _db.BatchingPlants on project.batching_plant_id equals batchingPlant.BatchingPlantId
                        join shipment in _db.SAPShipments on project.sap_shipment_no equals shipment.shipment_no
                        join shipmenactivity in qShipmentAct on shipment.LdtP_no equals shipmenactivity.ticket_number
                        join vehicle in _db.Vehicles on shipmenactivity.truck_number equals vehicle.vehicle_number
                        join vehicledriver in _db.VehicleDrivers on vehicle.vehicle_id equals vehicledriver.vehicle_id
                        join diverData in qDriver on vehicledriver.driver_id equals diverData.driverId
                        let currentPosition = _db.Positions.Where(x => x.vehicle_id == vehicle.vehicle_id).OrderByDescending(x => x.date_time).FirstOrDefault()
                        let historyPosition = _db.Positions.Where(x => x.vehicle_id == vehicle.vehicle_id && x.date_time >= (shipmenactivity.begin_loading_time ?? System.DateTime.Now) && x.date_time <= (shipmenactivity.available_time ?? System.DateTime.Now))
                        select new ShipmentDetail
                        {
                            projectId = project.id,
                            shipmentId = shipmenactivity.id,
                            ticketNumber = shipmenactivity.ticket_number,
                            vehicleId = vehicle.vehicle_id,
                            driverId = diverData.driverId,
                            driverName = diverData.driverName,
                            vehicleNumber = vehicle.vehicle_number,
                            statusId = shipmenactivity.readStatus().statusId,
                            currentPosition = new Position()
                            {
                                shipmentId = shipmenactivity.id,
                                lat = currentPosition.lat,
                                lon = currentPosition.lon,
                                engine = currentPosition.engine,
                                course = currentPosition.course,
                            },
                            history = historyPosition.Select((x) => new Position
                            {
                                shipmentId = shipmenactivity.id,
                                lat = x.lat,
                                lon = x.lon,
                                engine = x.engine,
                                course = x.course,
                            }).ToList(),
                            shipmentSummary = new ShipmentSummary
                            {
                                confirmStatus = shipmenactivity.confirm_status,
                                confirmNote = shipmenactivity.confirm_note,
                                confirmDate = shipmenactivity.confirm_date,
                                datetime = shipmenactivity.created_date,
                                batchingPlantName = batchingPlant.BatchingPlantName,
                                customerName = customer.CustomerName,
                                shiptoName = project.shipto_name,
                                shiptoAddress = project.shipto_address,
                                shipmentId = shipmenactivity.id,
                                projectId = project.id,
                                ticketNumber = shipmenactivity.ticket_number,
                                volume = shipmenactivity.volume,
                                vehicleId = vehicle.vehicle_id,
                                vehicleNumber = shipmenactivity.truck_number,
                                driverName = diverData.driverName,
                                statusId = shipmenactivity.readStatus().statusId,
                                status = shipmenactivity.readStatus().status,
                                driverId = diverData.driverId,
                            },
                            shipmentStatus = new ShipmentStatus
                            {
                                confirmStatus = shipmenactivity.confirm_status,
                                confirmNote = shipmenactivity.confirm_note,
                                confirmDate = shipmenactivity.confirm_date,
                                shipmentId = shipmenactivity.id,
                                projectId = project.id,
                                ticketNumber = shipmenactivity.ticket_number,
                                beginLoadingTime = shipmenactivity.begin_loading_time,
                                leavePlantTime = shipmenactivity.leave_plant_time,
                                arrivalTime = shipmenactivity.arrival_time,
                                beginUnloadingTime = shipmenactivity.begin_loading_time,
                                unloadingByDriver = shipmenactivity.unloading_by_driver,
                                unloadingByDriverLat = shipmenactivity.unloading_by_driver_lat,
                                unloadingByDriverlon = shipmenactivity.unloading_by_driver_lon,
                                podRecipientName = shipmenactivity.pod_recipient_name,
                                podTime = shipmenactivity.pod_time,
                                podFile1 = imageInclude == true ? $@"{_config.DownloadBaseUrl}/{shipmenactivity.pod_file1}" : null,
                                podFile2 = imageInclude == true ? $@"{_config.DownloadBaseUrl}/{shipmenactivity.pod_file2}" : null,
                                returningTime = shipmenactivity.returning_time,
                                availableTime = shipmenactivity.available_time,
                                isEmergency = shipmenactivity.is_emergency,
                                statusId = shipmenactivity.readStatus().statusId,
                                driverId = diverData.driverId,
                                vehicleId = vehicle.vehicle_id,
                                driverName = diverData.driverName,
                                vehicleNumber = vehicle.vehicle_number,
                                customerName = customer.CustomerName,
                                projectName = project.project_name,
                                rating = shipmenactivity.rating,
                                ratingNote = shipmenactivity.rating_note,
                                ratingDate = shipmenactivity.rating_date,
                            }

                        });

            return data;
        }

        public Object updateStatus(ShipmentStatus status, int? mode = 2, string modifier = "", bool imageInclude = false, ReturnMode returnMode = ReturnMode.Status)
        {
            var shipment = _db.ShipmentActivities.FirstOrDefault(x => x.id == status.shipmentId);

            if (shipment != null)
            {
                if (mode == 1)
                {
                    shipment.begin_loading_time = status.beginLoadingTime;
                    shipment.leave_plant_time = status.leavePlantTime;
                    shipment.arrival_time = status.arrivalTime;
                    shipment.begin_unloading_time = status.beginUnloadingTime;
                    shipment.unloading_by_driver = status.unloadingByDriver;
                    shipment.pod_recipient_name = status.podRecipientName;
                    shipment.pod_time = status.podTime;
                    shipment.pod_file1 = status.podFile1;
                    shipment.pod_file2 = status.podFile2;
                    shipment.returning_time = status.returningTime;
                    shipment.available_time = status.availableTime;
                    shipment.is_emergency = status.isEmergency;
                }
                else if (mode == 2)
                {
                    if (status.beginLoadingTime != null) shipment.begin_loading_time = status.beginLoadingTime;
                    if (status.leavePlantTime != null) shipment.leave_plant_time = status.leavePlantTime;
                    if (status.arrivalTime != null) shipment.arrival_time = status.arrivalTime;
                    if (status.beginUnloadingTime != null) shipment.begin_unloading_time = status.beginUnloadingTime;
                    if (status.podRecipientName != null) shipment.pod_recipient_name = status.podRecipientName;
                    //if (status.podTime != null) shipment.pod_time = status.podTime;
                    if (status.podTime != null) shipment.pod_time = System.DateTime.Now;  //override datetime pod to server time
                    //if (status.podFile1 != null) shipment.pod_file1 = status.podFile1;
                    //if (status.podFile2 != null) shipment.pod_file2 = status.podFile2;
                    if (status.podFile1 != null) shipment.pod_file1 = UploadHelper.saveImage(status.podFile1, _config, "pod1");
                    if (status.podFile2 != null) shipment.pod_file2 = UploadHelper.saveImage(status.podFile2, _config, "pod2");
                    if (status.returningTime != null) shipment.returning_time = status.returningTime;
                    if (status.availableTime != null) shipment.available_time = status.availableTime;
                    if (status.isEmergency != null) shipment.is_emergency = status.isEmergency;

                    //for update confirm status
                    if (status.confirmStatus != 0) shipment.confirm_status = status.confirmStatus;
                    //if (status.confirmDate != null) shipment.confirm_date = status.confirmDate;
                    if (status.confirmStatus != 0) shipment.confirm_date = System.DateTime.Now; //ambil tanggal dari server
                    if (!string.IsNullOrEmpty(status.confirmNote)) shipment.confirm_note = status.confirmNote;

                    //for update unloading by driver
                    if (status.unloadingByDriver != null) shipment.unloading_by_driver = status.unloadingByDriver;
                    if (status.unloadingByDriverLat != null) shipment.unloading_by_driver_lat = status.unloadingByDriverLat;
                    if (status.unloadingByDriverlon != null) shipment.unloading_by_driver_lon = status.unloadingByDriverlon;

                    //for update ratting 
                    if (status.rating != null) shipment.rating = status.rating;
                    if (status.ratingNote != null) shipment.rating_note = status.ratingNote;
                    if (status.ratingDate != null) shipment.rating_date = status.ratingDate;



                }


                //updatelog
                shipment.modified_by = modifier;
                shipment.modified_date = System.DateTime.Now;


                _db.SaveChanges();
            }

            switch (returnMode)
            {
                case ReturnMode.Status:
                    return (get(shipmentId: status.shipmentId, imageInclude: imageInclude).Select(x => x.shipmentStatus).First());

                case ReturnMode.Summary:
                    return (get(shipmentId: status.shipmentId, imageInclude: imageInclude).Select(x => x.shipmentSummary).First());

                case ReturnMode.All:
                    return (get(shipmentId: status.shipmentId, imageInclude: imageInclude).FirstOrDefault());

                default:
                    return (get(shipmentId: status.shipmentId, imageInclude: imageInclude).Select(x => x.shipmentStatus).First());
            }

        }

        public List<ShipmentSummary> getSummary(string projectId = null, int? shipmentId = null, int? driverId = null)
        {
            IQueryable<ShipmentDetail> qDetail = get(projectId: projectId, shipmentId: shipmentId, driverId: driverId);

            return (from a in qDetail select a.shipmentSummary).ToList();
        }

        public List<ShipmentStatus> getStatus(string projectId = null, int? shipmentId = null, bool? imageIncluded = false)
        {
            IQueryable<ShipmentDetail> qDetail = get(projectId: projectId, shipmentId: shipmentId, imageInclude: imageIncluded);

            var data = from a in qDetail select a.shipmentStatus;

            return data.ToList();
        }

        public List<Position> getHistory(int shipmentId)
        {
            IQueryable<ShipmentDetail> qDetail = get(shipmentId: shipmentId);

            var data = (from a in qDetail select a.history).FirstOrDefault();

            return data;
        }

        public List<Position> getCurrent(int? shipmentId = null, string projectId = null)
        {
            IQueryable<ShipmentDetail> qDetail = get(shipmentId: shipmentId, projectId: projectId);

            var data = (from a in qDetail select a.currentPosition).ToList();

            return data;
        }

        public List<EmergencyResponse> getEmergency(int? shipmentId = null, string emergencyId = null)
        {
            IQueryable<ShipmentEmergency> qEmergency = _db.ShipmentEmergencies;

            if (shipmentId.HasValue) qEmergency = qEmergency.Where(x => x.delivery_id == shipmentId);

            if (!string.IsNullOrEmpty(emergencyId)) qEmergency = qEmergency.Where(x => x.emergency_file_guid == emergencyId);

            var data = (from emergency in qEmergency
                        join delivery in _db.ShipmentActivities on emergency.delivery_id equals delivery.id
                        join vehicle in _db.Vehicles on delivery.truck_number equals vehicle.vehicle_number
                        join vehicleDriver in _db.VehicleDrivers on vehicle.vehicle_id equals vehicleDriver.vehicle_id
                        join driver in _db.Drivers on vehicleDriver.driver_id equals driver.driverId
                        join sapShipment in _db.SAPShipments on delivery.ticket_number equals sapShipment.LdtP_no
                        join project in _db.Projects on sapShipment.shipment_no equals project.sap_shipment_no
                        select new EmergencyResponse
                        {
                            id = emergency.emergency_file_guid,
                            note = emergency.emergency_note,
                            shipmentId = emergency.delivery_id,
                            ticketNumber = delivery.ticket_number,
                            dateTime = emergency.emergency_time,
                            file = $"{_config.DownloadBaseUrl}/{emergency.emergency_file}",
                            driverId = delivery.id,
                            driverName = driver.driverName,
                            vehicleId = vehicle.vehicle_id,
                            vehicleNumber = vehicle.vehicle_number,
                            statusId = delivery.readStatus().statusId,
                            projectId = project.id,
                        }).ToList();


            return data;
        }

        public List<EmergencyResponse> addEmmergency(EmergencyInput data)
        {
            var emergency = new ShipmentEmergency()
            {
                emergency_file = UploadHelper.saveImage(data.file, _config, "emergency"),
                emergency_note = data.note,
                delivery_id = data.shipmentId,
                emergency_time = System.DateTime.Now,
            };

            //insert into emergency
            _db.ShipmentEmergencies.Add(emergency);

            _db.SaveChanges();

            return getEmergency(data.shipmentId, emergency.emergency_file_guid);

        }

        /// <summary>
        /// response shipment base
        /// </summary>
        public class Shipnent
        {
            public string projectId { get; set; }
            public int? shipmentId { get; set; }
            public string ticketNumber { get; set; }
            public int? vehicleId { get; set; }
            public int? driverId { get; set; }
            public string driverName { get; set; }
            public string vehicleNumber { get; set; }
            public int statusId { get; set; }
        }

        /// <summary>
        /// response shipmen summary
        /// </summary>
        public class ShipmentSummary : Shipnent
        {
            public byte confirmStatus { get; set; }
            public string confirmNote { get; set; }
            public DateTime? confirmDate { get; set; }
            public DateTime? datetime { get; set; }
            public string batchingPlantName { get; set; }
            public string customerName { get; set; }
            public string shiptoName { get; set; }
            public string shiptoAddress { get; set; }
            public int? volume { get; set; }
            public string status { get; set; }
            public string qcFile { get; set; }

        }

        /// <summary>
        /// response all sehipment data
        /// </summary>
        public class ShipmentDetail : Shipnent
        {
            public ShipmentSummary shipmentSummary { get; set; }
            public ShipmentStatus shipmentStatus { get; set; }
            public Position currentPosition { get; set; }
            public List<Position> history { get; set; }
        }

        /// <summary>
        /// model for response and input update shipment status
        /// </summary>
        public class ShipmentStatus : Shipnent
        {
            public byte confirmStatus { get; set; }
            public string confirmNote { get; set; }
            public DateTime? confirmDate { get; set; }
            public DateTime? beginLoadingTime { get; set; }
            public DateTime? leavePlantTime { get; set; }
            public DateTime? arrivalTime { get; set; }
            public DateTime? beginUnloadingTime { get; set; }
            public DateTime? unloadingByDriver { get; set; }
            public decimal? unloadingByDriverLat { get; set; }
            public decimal? unloadingByDriverlon { get; set; }
            public string podRecipientName { get; set; }
            public DateTime? podTime { get; set; }
            public string podFile1 { get; set; }
            public string podFile2 { get; set; }
            public DateTime? returningTime { get; set; }
            public DateTime? availableTime { get; set; }
            public bool? isEmergency { get; set; }
            public string customerName { get; set; }
            public string projectName { get; set; }
            public byte? rating { get; set; }
            public string ratingNote { get; set; }
            public DateTime? ratingDate { get; set; }

        }

        public class ShipmentConfirm
        {
            public int? shipmentId { get; set; }
            public byte confirmStatus { get; set; }
            public string confirmNote { get; set; }
        }

        public class Position
        {
            public int shipmentId { get; set; }
            public double? lat { get; set; }
            public double? lon { get; set; }
            public bool? engine { get; set; }
            public short? course { get; set; }
        }

        /// <summary>
        /// model for input pod
        /// </summary>
        public class Pod
        {
            public int shipmentId { get; set; }
            public string podRecipientName { get; set; }
            public DateTime? podTime { get; set; }
            public string podFile1 { get; set; }
            public string podFile2 { get; set; }
        }

        /// <summary>
        /// response emergency input
        /// </summary>
        public class EmergencyResponse : Shipnent
        {
            public string id { get; set; }
            public string file { get; set; }
            public string note { get; set; }
            public DateTime? dateTime { get; set; }

        }

        /// <summary>
        /// model for input emergency
        /// </summary>
        public class EmergencyInput
        {
            public int shipmentId { get; set; }
            public string file { get; set; }
            public string note { get; set; }
        }

        public class ConfirmUnloadingInput
        {
            public int shipmentId { get; set; }
            public decimal? unloadingByDriverLat { get; set; }
            public decimal? unloadingByDriverlon { get; set; }
        }

        public class RattingInput
        {
            public int shipmentId { get; set; }
            public byte? rating { get; set; }
            public string ratingNote { get; set; }
        }

        public enum ReturnMode
        {
            All,
            Status,
            Summary,
        }
    }
}
