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
            _product = new ProductModel(db, config);
        }

        public ShipmentModel()
        {

        }
        #endregion

        #region parameter

        #endregion

        public IQueryable<ShipmentDetail> get(string projectId = null, int? shipmentId = null, bool? imageInclude = false)
        {
            IQueryable<Project> qProject = _db.Projects;
            IQueryable<ShipmentActivity> qShipmentAct = _db.ShipmentActivities;

            if (!string.IsNullOrEmpty(projectId)) qProject = qProject.Where(x => x.id == projectId);

            if (shipmentId.HasValue) qShipmentAct = _db.ShipmentActivities.Where(x => x.id == shipmentId);


            var data = (from project in qProject
                        join batchingPlant in _db.BatchingPlants on project.batching_plant_id equals batchingPlant.BatchingPlantId
                        join shipment in _db.SAPShipments on project.sap_shipment_no equals shipment.shipment_no
                        join shipmenactivity in qShipmentAct on shipment.LdtP_no equals shipmenactivity.ticket_number
                        join vehicle in _db.Vehicles on shipmenactivity.truck_number equals vehicle.vehicle_number
                        join vehicledriver in _db.VehicleDrivers on vehicle.vehicle_id equals vehicledriver.vehicle_id
                        join diverData in _db.Drivers on vehicledriver.driver_id equals diverData.driverId
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
                                datetime = shipmenactivity.created_date,
                                batchingPlantName = batchingPlant.BatchingPlantName,
                                customerName = project.shipto_name,
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
                            },
                            shipmentStatus = new ShipmentStatus
                            {
                                shipmentId = shipmenactivity.id,
                                projectId = project.id,
                                ticketNumber = shipmenactivity.ticket_number,
                                beginLoadingTime = shipmenactivity.begin_loading_time,
                                leavePlantTime = shipmenactivity.leave_plant_time,
                                arrivalTime = shipmenactivity.arrival_time,
                                beginUnloadingTime = shipmenactivity.begin_loading_time,
                                unloadingByDriver = shipmenactivity.unloading_by_driver,
                                podRecipientName = shipmenactivity.pod_recipient_name,
                                podTime = shipmenactivity.pod_time,
                                podFile1 = imageInclude == true ? shipmenactivity.pod_file1 : null,
                                podFile2 = imageInclude == true ? shipmenactivity.pod_file2 : null,
                                returningTime = shipmenactivity.returning_time,
                                availableTime = shipmenactivity.available_time,
                                isEmergency = shipmenactivity.is_emergency,
                                statusId = shipmenactivity.readStatus().statusId,
                                driverId = diverData.driverId,
                                vehicleId = vehicle.vehicle_id,
                                driverName = diverData.driverName,
                                vehicleNumber = vehicle.vehicle_number,
                            }

                        });

            return data;
        }

        public ShipmentStatus updateStatus(ShipmentStatus status, int? mode = 2, string modifier = "", bool imageInclude = false)
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
                    if (status.unloadingByDriver != null) shipment.unloading_by_driver = status.unloadingByDriver;
                    if (status.podRecipientName != null) shipment.pod_recipient_name = status.podRecipientName;
                    if (status.podTime != null) shipment.pod_time = status.podTime;
                    if (status.podFile1 != null) shipment.pod_file1 = status.podFile1;
                    if (status.podFile2 != null) shipment.pod_file2 = status.podFile2;
                    if (status.returningTime != null) shipment.returning_time = status.returningTime;
                    if (status.availableTime != null) shipment.available_time = status.availableTime;
                    if (status.isEmergency != null) shipment.is_emergency = status.isEmergency;
                }


                //updatelog
                shipment.modified_by = modifier;
                shipment.modified_date = System.DateTime.Now;


                _db.SaveChanges();
            }

            var data = get(shipmentId: status.shipmentId, imageInclude: imageInclude).Select(x => x.shipmentStatus).First();

            return data;
        }

        public List<ShipmentSummary> getSummary(string projectId = null, int? shipmentId = null)
        {
            IQueryable<ShipmentDetail> qDetail = get(projectId: projectId, shipmentId: shipmentId);

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

            var data = (from a in qEmergency
                        select new EmergencyResponse
                        {
                            id = a.emergency_file_guid,
                            shipmentId = a.delivery_id,
                            ticketNumber = a.ticket_number,
                            dateTime = a.emergency_time,
                            file = a.emergency_file,
                        }).ToList();

            return data;
        }

        public List<EmergencyResponse> addEmmergency(EmergencyInput data)
        {
            var emergency = new ShipmentEmergency()
            {
                emergency_file = data.file,
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
            public DateTime? datetime { get; set; }

            public string batchingPlantName { get; set; }
            public string customerName { get; set; }
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
            public DateTime? beginLoadingTime { get; set; }
            public DateTime? leavePlantTime { get; set; }
            public DateTime? arrivalTime { get; set; }
            public DateTime? beginUnloadingTime { get; set; }
            public DateTime? unloadingByDriver { get; set; }
            public string podRecipientName { get; set; }
            public DateTime? podTime { get; set; }
            public string podFile1 { get; set; }
            public string podFile2 { get; set; }
            public DateTime? returningTime { get; set; }
            public DateTime? availableTime { get; set; }
            public bool? isEmergency { get; set; }


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

    }
}
