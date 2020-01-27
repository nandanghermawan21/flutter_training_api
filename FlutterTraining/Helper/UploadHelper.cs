using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace FlutterTraining.Helper
{
    public static class UploadHelper
    {
        public static string saveImage(string base64, AppSettings settings, string folder)
        {
            string extention = base64.Split(',')[0].Split(';')[0].Split('/')[1];
            string guidId = Guid.NewGuid().ToString();
            string path = $@"{settings.UploadLocation}\{folder}";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes($@"{path}\{guidId}.{extention}", Convert.FromBase64String(base64.Split(',')[1]));
            return $@"{folder}/{guidId}.{extention}";
        }

        public static string getImageUrl(string imageLocation, AppSettings settings)
        {
            return string.IsNullOrEmpty(imageLocation) ? "" : $@"{settings.DownloadBaseUrl}/{imageLocation}";
        }
    }
}
