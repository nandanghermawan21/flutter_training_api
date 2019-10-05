using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Helper
{
    public class Resource
    {
        private static Resource instance = null;
        private static string currentLang = "";

        #region default resource
        //parameter resource
        public string phoneNotFound = "phone number not found";
        #endregion

        #region resource bahasa indonesia
        //function to set resource
        private void id()
        {
           phoneNotFound = "No telepon tidak ditemukan";
        }
        #endregion

        #region siggleton setting
        private Resource(string id = "")
        {
            //string id = culture.Name.Split('-')[0];

            switch (id)
            {
                case "id":
                    this.id();
                    break;
            }
        }
        public static Resource Instance(string lang = "")
        {
            lang = string.IsNullOrEmpty(lang) ? currentLang : lang;
            if (instance == null || currentLang != lang)
            {
                currentLang = lang;
                instance = new Resource(lang);
            }
            return instance;
        }
        #endregion
    }
}
