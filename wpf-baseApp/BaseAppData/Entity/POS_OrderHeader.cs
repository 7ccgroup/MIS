using BaseAppData.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppData.Entity
{
    public class POS_OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public int iOrderID { get; set; }
        public int? iRelatedOrderID { get; set; }

        [Index("ItemIndex", IsUnique = true)]
        public int vOrderNumber { get; set; }
        [MaxLength(25)]
        public string vOrderRef { get; set; }
        [MaxLength(10)]
        public string vOrderType { get; set; }
        [MaxLength(10)]
        public string vOrderMove { get; set; }
        [MaxLength(75)]
        public string vOrderAcct { get; set; }
        [MaxLength(75)]
        public string vOrderSubAcct { get; set; }

        [DecimalPrecision(10, 2)]
        public decimal dOrderOrigAmtDue { get; set; }

        [DecimalPrecision(10, 2)]
        public decimal dOrderAmtDue { get; set; }

        [DecimalPrecision(10, 2)]
        public decimal dOrderAmount { get; set; }

        [DecimalPrecision(10, 2)]
        public decimal dOrderTax { get; set; }

        [MaxLength(10)]
        public string vOrderCurr { get; set; }
        [MaxLength(200)]
        public string vOrderNotes { get; set; }

        [MaxLength(100)]
        public string vOrderStatus { get; set; }

        [MaxLength(50)]
        public string vOrderPaymentType { get; set; }

        [MaxLength(25)]
        public string vOrderPaymentRef { get; set; }

        [MaxLength(100)]
        public string vOrderEmail { get; set; }
        [MaxLength(100)]
        public string vOrderComments1 { get; set; }
        [MaxLength(100)]
        public string vOrderComments2 { get; set; }
        [MaxLength(100)]
        public string vOrderComments3 { get; set; }
        [MaxLength(100)]
        public string vOrderComments4 { get; set; }
        [MaxLength(100)]
        public string vOrderComments5 { get; set; }
        [MaxLength(100)]
        public string vOrderComments6 { get; set; }
        [MaxLength(100)]
        public string vOrderComments7 { get; set; }
        [MaxLength(100)]
        public string vOrderComments8 { get; set; }

        public DateTime dCreatedDate { get; set; }
        public DateTime dModifiedDate { get; set; }

        
        [MaxLength(100)]
        public string vEntryBy { get; set; }

        public long? tTimestamp { get; set; }

        public int CustomerId { get; set; }
        public virtual POS_Customer Customer { get; set; }

        public int POS_SetupId { get; set; }
        public virtual POS_Setup POS_Setup { get; set; }

        public virtual ICollection<POS_OrderDetails> POS_OrderDetails { get; set; }

        public POS_OrderHeader()
        {

            POS_OrderDetails = new List<POS_OrderDetails>();
        }

        [MaxLength(100)]
        public string vApprovalCode { get; set; }

        [MaxLength(100)]
        public string TxnId { get; set; }
        

        [DecimalPrecision(10, 2)]
        public decimal? dOrderTip { get; set; }

        [DecimalPrecision(10, 2)] //SAA 5/13/16
        public decimal? dOrderServiceFee { get; set; }

        [MaxLength(10)]
        public string vOrderTableNumber { get; set; }


        public int? iSplitCount { get; set; }

        [MaxLength(2000)]
        public string vSplitPayment { get; set; }

    }
}
