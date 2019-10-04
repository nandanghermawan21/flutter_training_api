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
            phoneNumber = int.Parse(phoneNumber.Substring(0, 1)) == 0 ? "62" + phoneNumber.Substring(1) : phoneNumber;
            var customer = _db.Customers.Where((c) => c.MobileNumber == phoneNumber
              || (!String.IsNullOrEmpty(c.MobileNumber) ? "62" + c.MobileNumber.Substring(1) : "") == phoneNumber).FirstOrDefault();
            return customer;
        }
    }
}
