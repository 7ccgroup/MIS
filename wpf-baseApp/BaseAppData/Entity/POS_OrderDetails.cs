using BaseAppData.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppData.Entity
{
   public class POS_OrderDetails
    {
       [Key]
       public int Id { get; set; }

       public int iOrderLineId { get; set; }

       public int fOrderQty { get; set; }

        [DecimalPrecision(10, 2)]
       public decimal fLineItemPrice { get; set; }

       [MaxLength(50)]
       public string vLineSplInstructions { get; set; }

       [DecimalPrecision(6, 2)]
       public decimal fLineSubTotal { get; set; }

       [MaxLength(255)]
       public string vComments { get; set; }

       [MaxLength(8)]
       public string vStatus { get; set; }
       public DateTime dCreatedDate { get; set; }
       public DateTime dModifiedDate { get; set; }

       [MaxLength(8)]
       public string vEntryBy { get; set; }

       public long? tTimestamp { get; set; }


       public virtual int POS_OrderHeaderId { get; set; }

       public virtual POS_OrderHeader POS_OrderHeader { get; set; }

       public virtual int POS_ItemMasterId { get; set; }

       public virtual POS_ItemMaster POS_ItemMaster { get; set; }

       [MaxLength(25)]
       public string vLineItemsize { get; set; } //

       [MaxLength(25)]
       public string vLineItemspice { get; set; }

       [MaxLength(255)]
       public string vLineitemaddon { get; set; }

       [MaxLength(25)]
       public string vLineItemcooked { get; set; }

       [MaxLength(255)]
       public string vCustomComments { get; set; }

        [DecimalPrecision(10, 2)]
        public decimal fLineItemAddonTotal { get; set; }
    }
}
