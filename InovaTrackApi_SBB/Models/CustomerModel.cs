using InovaTrackApi_SBB.Context;
using InovaTrackApi_SBB.Helper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace InovaTrackApi_SBB.Models
{
  

    public class CustomerModel
    {
        private ApplicationDbContext _db;
        private readonly AppSettings _config;

        CustomerModel()
        {
            //_db = db;
            //_config = config.Value;
        }

        //public Context.Customer checkPhoneNUmberExist(string phoneNumber)
        //{
        //   var customers = _db.Customers.Where((c) => c.MobileNumber == phoneNumber
        //       || (!String.IsNullOrEmpty(c.MobileNumber) ? "62" + c.MobileNumber.Substring(1) : "") == phoneNumber).FirstOrDefault();

        //    return customers;
        //}
    }
}
