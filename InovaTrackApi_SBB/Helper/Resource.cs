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

        #region contructor
        public Resource() { }

        public Resource(string id)
        {
            switch (id)
            {
                case "id":
                    this.id();
                    break;
            }
        }
        #endregion

        #region default resource
        //parameter resource
        public string phoneNotFound = "phone number not found";
        public string passwordDuccessfullyChanged = "Password successfully changed";
        public string invalidData = "Invalid data";
        public string confirmPaswordNotMatch = "Confirm password is not match";
        public string phoneNumberNotRegistered = "Phone number not registered";
        public string thereIsTheCodeToResetYourPassword = "There is the code to reset your password";
        public string pleaseDoNotShareThisCodeWithAnyone = "please do not share this code with anyone";
        public string success = "Success";
        public string smsFailedToSend = "SMS failed to send";
        public string customerNotFound = "customer not found";
        public string newPasswordMustBeAtLeast8CharacterLong = "New passwords must be at least 8 characters long"
        public string phoneNumberRegistered = "Phone number is registered";
        public string 
        #endregion

        #region resource bahasa indonesia
        //function to set resource
        public Resource id()
        {
            return new Resource()
            {
                phoneNotFound = "No telepon tidak ditemukan",
                passwordDuccessfullyChanged = "Kata sandi berhasil diubah",
                invalidData = "Data tidak valid",
                confirmPaswordNotMatch = "Konfirmasi kata sandi tidak cocok",
                phoneNumberNotRegistered = "No telepon tidak terdaftar",
                thereIsTheCodeToResetYourPassword = "Berikut ada adalah kode untuk mereset password anda",
                pleaseDoNotShareThisCodeWithAnyone = "mohon tidak memberitahukan kode ini kepada siapapun",
                success = "sukses",
                smsFailedToSend = "Gagal mengirim sms",
                customerNotFound = "Pelanggan tidak ditemukan",
                newPasswordMustBeAtLeast8CharacterLong = "Password baru minimal 8 karakter",
                phoneNumberRegistered = "No telepon telah terdaftar",

            };
        }

        #endregion
    }
}