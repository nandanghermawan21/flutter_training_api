using System;

namespace FlutterTraining.Helper
{
    public class Geo
    {
        public static double Distance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            double R = 6371.0;          // R is earth radius.
            double dLat = ToRadian(Latitude2 - Latitude1);
            double dLon = ToRadian(Longitude2 - Longitude1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(ToRadian(Latitude1)) * Math.Cos(ToRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
