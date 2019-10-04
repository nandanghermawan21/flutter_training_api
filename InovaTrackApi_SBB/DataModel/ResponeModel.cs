using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class ResponeModel
    {
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public int statusCode { get; set;}
        public string statusString { get; set; }
        public object data { get; set; }
    }
}
