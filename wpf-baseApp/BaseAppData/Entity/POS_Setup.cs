using BaseAppData.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAppData.Entity
{
    public class POS_Setup
    {
        [Key]
        public int Id { get; set; }

         [MaxLength(200)]
        public string vCorpName { get; set; }

        [MaxLength(50)]
        public string vCorpPhone { get; set; }

        [MaxLength(500)]
        public string vCorpAddress1 { get; set; }

        [MaxLength(500)]
        public string vCorpAddress2 { get; set; }

        [MaxLength(100)]
        public string vCorpCity { get; set; }

        [MaxLength(100)]
        public string vCorpState { get; set; }

        [MaxLength(100)]
        public string vCorpCountry { get; set; }

        [DecimalPrecision(6, 2)]
        public Nullable<decimal> vSalesTax { get; set; }

        [MaxLength(500)]
        public string vReceiptComments { get; set; }

        [MaxLength(200)]
        public string vCorpEmail { get; set; }


        [MaxLength(200)]
        public string vCorpContact { get; set; }

        [MaxLength(500)]
        public string vAlertComments { get; set; }

        [MaxLength(100)]
        public string vStatus { get; set; }


        public Nullable<DateTime> dStartDt { get; set; }
        public Nullable<DateTime> dEndDt { get; set; }


        [MaxLength(8)]
        public string vEntryBy { get; set; }

        public Nullable<long> tTimestamp { get; set; }

        public virtual ICollection<POS_ItemCategory> ItemCategories { get; set; }
        public virtual ICollection<POS_Customer> Customers { get; set; }
        public virtual ICollection<POS_OrderHeader> OrderHeaders { get; set; }



        public POS_Setup()
        {
            ItemCategories = new List<POS_ItemCategory>();
            Customers = new List<POS_Customer>();
            OrderHeaders = new List<POS_OrderHeader>();
        }
      
    }
}
