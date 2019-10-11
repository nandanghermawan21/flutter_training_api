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

        public List<Shipment> get(string shipmentId = null)
        {
            IQueryable<Project> qProject = _db.Projects;

            if (!string.IsNullOrEmpty(shipmentId)) qProject = qProject.Where(x => x.id == shipmentId);

            var data = (from project in _db.Projects
                        join shipment in _db.SAPShipments on project.sap_shipment_no equals shipment.shipment_no
                        join shipmenactivity in _db.ShipmentActivities on shipment.LdtP_no equals shipmenactivity.ticket_number
                        join vehicle in _db.Vehicles on shipmenactivity.truck_number equals vehicle.vehicle_number
                        join vehicledriver in _db.VehicleDrivers on vehicle.vehicle_id equals vehicledriver.vehicle_id
                        join diverData in _db.Drivers on vehicledriver.driver_id equals diverData.driver_id

                        select new Shipment()
                        {
                            shipmentId = shipmenactivity.id,
                            ticketNumber = shipmenactivity.ticket_number,
                            volume = shipmenactivity.volume,
                            vehicleNumber = shipmenactivity.truck_number,
                            driverName = diverData.driver_name,
                            statusId = shipmenactivity.statusId,
                            status = shipmenactivity.status,
                        }
                        ).ToList();

            return data;
        }

        public class Shipment
        {
            public int shipmentId { get; set; }
            public string ticketNumber { get; set; }
            public string driverName { get; set; }
            public string vehicleNumber { get; set; }
            public int? volume { get; set; }
            public int statusId { get; set; }
            public string status { get; set; }
            public string qcFile { get; set; }
        }

        public class ShipmentStatus : Shipment
        {
            public DateTime? beginLoadingTime { get; set; }
            public DateTime? leavePlantTime { get; set; }
            public DateTime? arrivalTime { get; set; }
            public DateTime? beginUnloadingTime { get; set; }
            public DateTime? unloadingByDriver { get; set; }
            public String podRecipientName { get; set; }
            public DateTime? podTime { get; set; }
            public string podFile1 { get; set; }
            public string podFile2 { get; set; }
            public DateTime? returningTime { get; set; }
            public DateTime? availableTime { get; set; }
            public bool isEmergency { get; set; }
        }

        public class Vehicle
        {
            public int deliveryId { get; set; }
            public string driverName { get; set; }
        }

        public class LivePosition : Vehicle
        {
            public List<LatLng> history { get; set; }
        }

        public class Position : Vehicle
        {
            public LatLng position { get; set; }
        }

        public class LatLng
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

    }
}
