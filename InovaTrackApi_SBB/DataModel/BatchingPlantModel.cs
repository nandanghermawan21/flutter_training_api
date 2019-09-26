using GeoCoordinatePortable;

namespace InovaTrackApi_SBB.DataModel
{
    public class BatchingPlantModel
    {
        public int BatchingPlantId { get; set; }
        public string BatchingPlantName { get; set; }
        public double DistanceKm { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
