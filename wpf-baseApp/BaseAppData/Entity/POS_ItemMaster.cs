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
   public class POS_ItemMaster
    {
       [Key]
       public int Id { get; set; }

       [Index("ItemIndex", IsUnique = true)]
       public int itemID { get; set; }

       [MaxLength(30)]
       public string vItemSku { get; set; }

       [MaxLength(200)]
       public string vItemDesc1 { get; set; }

       [MaxLength(200)]
       public string vItemDesc2 { get; set; }



       [DecimalPrecision(6, 2)]
       public decimal vItemPrice { get; set; }

       public int vItemRelatedID { get; set; }

       [DecimalPrecision(6, 2)]
       public decimal? vItemMinPrice { get; set; }

       public char vItemDelivery { get; set; }

       [MaxLength(200)]
       public string vItemVendor { get; set; }

       [MaxLength(100)]
       public string vItemVendorContact { get;set; }

        [MaxLength(20)]
       public string vItemVendorPhone { get; set; }

       [MaxLength(300)]
       public string vItemNotes { get; set; }

       [MaxLength(30)]
       public string vItemStatus { get; set; }

       [MaxLength(30)]
       public string vItemAvailability { get; set; }

       public DateTime dStartDate { get; set; }

       public DateTime dEndDate { get; set; }

       [MaxLength(50)]
       public string vEntryBy { get; set; }

       public long tTimestamp { get; set; }

       public virtual int POS_ItemCategoryId {get;set;}
       public virtual POS_ItemCategory POS_ItemCategory { get; set; }

       [MaxLength(10)]
       public string vItemmodifier { get; set; }

    }
}
