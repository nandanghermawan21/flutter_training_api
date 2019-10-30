using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public string EmailHost { get; set; }
        public int EmailPort { get; set; }
        public int ResetPasswordExpiredTime { get; set; }
        public string GinotaApiKey { get; set; }
        public string GinotaApiSecreet { get; set; }
        public string GinotaSender { get; set; }
        public string UploadLocation { get; set; }
        public string DownloadBaseUrl { get; set; }

    }
}
