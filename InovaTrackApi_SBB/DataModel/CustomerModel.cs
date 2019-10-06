using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class CustomerModel
    {
        private ApplicationDbContext _db;

        public CustomerModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
        }

        public Customer CheckPhoneExist(string phoneNumber)
        {

           var customer = _db.Customers.Where((c) => c.MobileNumber == phoneNumber
           || (!String.IsNullOrEmpty(c.MobileNumber) ? c.MobileNumber.Substring(0, 1) == "0" ? "62" + c.MobileNumber.Substring(1) : c.MobileNumber.Substring(0, 1) == "+" ? c.MobileNumber.Substring(1) : c.MobileNumber : "")
           == (phoneNumber.Substring(0, 1) == "0" ? "62" + phoneNumber.Substring(1) : phoneNumber.Substring(0, 1) == "+" ? phoneNumber.Substring(1) : phoneNumber)).FirstOrDefault();


            return customer;
        }
    }
}
