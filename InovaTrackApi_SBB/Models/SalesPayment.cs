using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("sales_payment")]
    public class SalesPayment
    {
        [Key]
        [Column("id_payment")]
        public long PaymentId { get; set; }
        [Column("pay_date")]
        public DateTime? PaymentDate { get; set; }
        [Column("customer_email")]
        public string Email { get; set; }
        [Column("paid_amount")]
        public decimal? Amount { get; set; }
        [Column("shipment_number")]
        public string ShipmentNumber { get; set; }
        [Column("ticket_no")]
        public string TicketNumber { get; set; }
        [Column("paid_status")]
        public byte? PaymentStatus { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
    }
}
