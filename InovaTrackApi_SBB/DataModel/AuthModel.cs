using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class AuthModel
    {
        public static class Actor
        {
            public const string customer = "C";
            public const string sales = "S";
            public const string driver = "D";
        }

        public class ProfileModel
        {
            public string name;
            public string email;
            public string phone;
            public string addrss;
            public string avatar;
        }

    }
}
