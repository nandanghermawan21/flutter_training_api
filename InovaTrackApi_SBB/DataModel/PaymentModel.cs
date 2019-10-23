using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class PaymentModel
    {

        #region contructor

        private ApplicationDbContext _db;
        public PaymentModel() { }
        public PaymentModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
        }

        #endregion
        public List<Payment> get(string projectId = null, int? paymentId = null, bool includeImmage = false)
        {
            IQueryable<ProjectPayment> qPayment = _db.peojectPayments;

            if (!string.IsNullOrEmpty(projectId)) qPayment = qPayment.Where(x => x.project_id == projectId);

            if (paymentId.HasValue) qPayment = qPayment.Where(x => x.payment_id == paymentId);

            var data = (from payment in qPayment
                        join account in _db.BankAccounts on payment.paid_to_guid_account equals account.guid_account
                        select new Payment
                        {
                            projectId = payment.project_id,
                            id = payment.payment_id,
                            amount = payment.paid_amount,
                            bankAccountNo = account.bank_acc_no,
                            bankAccountId = account.guid_account,
                            bankCode = account.bank_code,
                            status = payment.status,
                            note = payment.note,
                            payerName = payment.paid_by,
                            payDate = payment.paid_date,
                            proofOfPayment = includeImmage == true ? payment.paid_file : "data:image/jpeg;base64",
                        }).ToList();

            return data;
        }

        public Payment create(PaymentParam data)
        {
            var payment = new ProjectPayment
            {
                project_id = data.projectId,
                paid_to_guid_account = data.accountid,
                paid_amount = data.paidAmount,
                paid_by = data.payerName,
                paid_date = data.payDate,
                paid_file = data.proofOfPayment,
                status = 0,
            };

            _db.peojectPayments.Add(payment);

            _db.SaveChanges();

            return get(paymentId: payment.payment_id).First();
        }

        public List<Account> GetBankAccounts(string accoundId = null)
        {
            IQueryable<BankAccount> qBankAcc = _db.BankAccounts;

            if (!string.IsNullOrEmpty(accoundId)) qBankAcc = _db.BankAccounts.Where(x => x.guid_account == accoundId);

            var data = (from x in qBankAcc
                        where x.status.Equals('1')
                        select new Account
                        {
                            accountId = x.guid_account,
                            bankCode = x.bank_code,
                            bankAccountName = x.bank_acc_name,
                            bankAccountNo = x.bank_acc_no,
                            brach = x.branch,
                            status = x.status,
                        }).ToList();

            return data;
        }

        public class Account
        {
            public string accountId { get; set; }
            public string bankCode { get; set; }
            public string bankAccountNo { get; set; }
            public string bankAccountName { get; set; }
            public string brach { get; set; }
            public char? status { get; set; }
        }

        public class Payment
        {
            public string projectId { get; set; }
            public int id { get; set; }
            public decimal? amount { get; set; }
            public byte? status { get; set; }
            public string note { get; set; }
            public string bankAccountId { get; set; }
            public string bankCode { get; set; }
            public string bankAccountNo { get; set; }
            public string payerName { get; set; }
            public DateTime? payDate { get; set; }
            public string proofOfPayment { get; set; }
        }

        public class PaymentParam
        {
            public string projectId { get; set; }
            public string accountid { get; set; }
            public string payerName { get; set; }
            public DateTime payDate { get; set; }
            public string proofOfPayment { get; set; }
            public decimal paidAmount { get; set; }
        }

    }
}
