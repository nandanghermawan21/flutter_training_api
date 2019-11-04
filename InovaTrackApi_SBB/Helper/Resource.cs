using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.Helper
{
    public class Resource
    {
        #region default resource
        public class ResourceData
        {
            //parameter resource
            public string phoneNotFound = "phone number not found";
            public string passwordDuccessfullyChanged = "Password successfully changed";
            public string invalidData = "Invalid data";
            public string confirmPaswordNotMatch = "Confirm password is not match";
            public string phoneNumberNotRegistered = "Phone number not registered";
            public string thereIsTheCodeToResetYourPassword = "This is the code to reset your password,";
            public string pleaseDoNotShareThisCodeWithAnyone = "please do not share this code with anyone";
            public string success = "Success";
            public string smsFailedToSend = "SMS failed to send";
            public string customerNotFound = "Customer not found";
            public string newPasswordMustBeAtLeast8CharacterLong = "New passwords must be at least 8 characters long";
            public string phoneNumberRegistered = "Phone number is registered";
            public string emailAlreadyRegistered = "Email already registered";
            public string registerSuccess = "Registration successful";
            public string productNotFound = "Product Material not found";
            public string loading = "Loading";
            public string leavingPlant = "Leaving Plant";
            public string arriving = "Arriving";
            public string unloading = "Unloading";
            public string returnning = "Returning";
            public string completed = "Completed";
            public string waiting = "Waiting";
            public string emailOrPasswordInCorrect = "email or password incorrect";
            public string thisUserIsNotPermitted = "This user is not permitted";
            public string confirmed = "Confirmed";
            public string pod = "Proof Of Delivery";
        }
        #endregion

        public static ResourceData Lang(string id)
        {
            ResourceData ressource = null;
            switch (id)
            {
                case "id":
                    ressource = Id;
                    break;

                default:
                    ressource = En;
                    break;
            }
            return ressource;
        }

        #region resource english
        public static ResourceData En
        {
            get
            {
                return new ResourceData();
            }
        }
        #endregion

        #region resource bahasa indonesia
        //function to set resource
        public static ResourceData Id
        {
            get
            {
                return new ResourceData()
                {
                    phoneNotFound = "No telepon tidak ditemukan",
                    passwordDuccessfullyChanged = "Kata sandi berhasil diubah",
                    invalidData = "Data tidak valid",
                    confirmPaswordNotMatch = "Konfirmasi kata sandi tidak cocok",
                    phoneNumberNotRegistered = "No telepon tidak terdaftar",
                    thereIsTheCodeToResetYourPassword = "ini adalah kode untuk mereset password anda,",
                    pleaseDoNotShareThisCodeWithAnyone = "mohon tidak memberitahukan kode ini kepada siapapun",
                    success = "sukses",
                    smsFailedToSend = "Gagal mengirim sms",
                    customerNotFound = "Pelanggan tidak ditemukan",
                    newPasswordMustBeAtLeast8CharacterLong = "Password baru minimal 8 karakter",
                    phoneNumberRegistered = "No telepon telah terdaftar",
                    emailAlreadyRegistered = "Email telah terdaftar",
                    registerSuccess = "Registrasi berhasil",
                    productNotFound = "Product tidak ditemukan",
                    emailOrPasswordInCorrect = "email atau kata sandi salah",
                    waiting = "menunggu",
                    loading = "Memuat",
                    arriving = "Tiba di Lokasi",
                    unloading = "Membongkar",
                    leavingPlant = "Meninggalkan Plant",
                    returnning = "Kembali ke Plant",
                    completed = "Complete",
                    thisUserIsNotPermitted = "User ini tidak diizinkan",
                    confirmed = "Terkonfirmasi",
                    pod = "Serah Terima",
                };
            }
        }
        #endregion
    }
}