using InovaTrackApi_SBB.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace InovaTrackApi_SBB.DataModel
{
    public class ProjectModel
    {
        public string name { get; set; }
        public string shiptoName { get; set; }
        public string shiptoAdrress { get; set; }
        public bool isMixerAllowed { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public DateTime timeSlot { get; set; }
        public int batchingPlantId { get; set; }
        public int id { get; set; }
        public string batchingPlantName { get; set; }
        public double batchingPlantLat { get; set; }
        public double batchingPlantLong { get; set; }
        public string structureTypeCode { get; set; }
        public string structureTypeName { get; set; }
        public string gradeCode { get; set; }
        public string gradeName { get; set; }
        public string slumpCode { get; set; }
        public string slumName { get; set; }
        public int volume { get; set; }
        public double price { get; set; }
        public DateTime shipmentDate { get; set; }
        public int shipmentInterval { get; set; }
        public int shipmentCount { get; set; }
        public int totalShipmet { get; set; }
        public int status { get; set; }
        public string statusString { get; set; }
        public DateTime dateTime { get; set; }
        public Projectsource source { get; set; } // c or s
        public int customerId { get; set; }
        public string salesId { get; set; }

    }

  

}
