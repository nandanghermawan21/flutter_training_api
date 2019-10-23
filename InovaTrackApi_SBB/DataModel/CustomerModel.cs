﻿using InovaTrackApi_SBB.Helper;
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

        public CustomerModel(ApplicationDbContext db)
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
        public List<Customer> get(int? userId = null, string salesId = null)
        {
            IQueryable<SalesCustomer> qsalesCustomer = _db.salesCustomers;

            if (userId.HasValue) qsalesCustomer = qsalesCustomer.Where(x => x.cust_id == userId);

            if (!string.IsNullOrEmpty(salesId)) qsalesCustomer = qsalesCustomer.Where(x => x.sales_nik == salesId);

            var data = (from salesCust in qsalesCustomer
                        join customer in _db.Customers  on salesCust.cust_id equals customer.CustomerId
                        select customer
                        ).ToList();


            return data;
        }

        public Customer update(Customer customer)
        {
            var data = _db.Customers.FirstOrDefault(x => x.CustomerId == customer.CustomerId);

            if (data != null)
            {
                if (!string.IsNullOrEmpty(customer.CustomerName)) data.CustomerName = customer.CustomerName;
                if (!string.IsNullOrEmpty(customer.Email)) data.Email = customer.Email;
                if (!string.IsNullOrEmpty(customer.MobileNumber)) data.CustomerName = customer.MobileNumber;
                if (!string.IsNullOrEmpty(customer.Address1)) data.Address1 = customer.Address1;
                if (!string.IsNullOrEmpty(customer.customerAvatar)) data.customerAvatar = customer.customerAvatar;

                _db.SaveChanges();
            }

            return get(userId: Convert.ToInt32(data.CustomerId)).First();
        }

    }
}
