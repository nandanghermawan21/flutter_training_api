using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterTraining.Helper
{
    public class GlobalData
    {
        private static GlobalData Get = null;

        public Resource.ResourceData resource { get;  set; }

        public GlobalData()
        {
        }

        public static GlobalData get
        {
            get
            {
                if (Get == null)
                {
                    {
                        Get = new GlobalData();
                    }
                }
                return Get;
            }
        }
    }
}
