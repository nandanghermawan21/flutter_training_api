using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Helper
{
    public sealed class GlobalData
    {
        private static GlobalData Get = null;
        private static readonly object padlock = new object();

        public Resource resource = new Resource();

        public GlobalData()
        {
        }

        public static GlobalData get
        {
            get
            {
                if (Get == null)
                {
                    lock (padlock)
                    {
                        if (Get == null)
                        {
                            Get = new GlobalData();
                        }
                    }
                }
                return Get;
            }
        }
    }
}
