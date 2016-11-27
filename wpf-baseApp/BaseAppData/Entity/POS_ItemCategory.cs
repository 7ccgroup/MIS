using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppData.Entity
{
   public class POS_ItemCategory
    {
       [Key]
       public int ID { get; set; }

       public int CatID { get; set; }

        [MaxLength(50)]
       public string vCategoryCode { get; set; }

        [MaxLength( 200)]
        public string vCategoryDesc { get; set; }

       [MaxLength(200)]
        public string vCategoryShortDesc { get; set; }

       [MaxLength(500)]
       public string vComments { get; set; }
       
       [MaxLength(8)]
       public string vStatus { get; set; }

       public DateTime? dStartDt { get; set; }
       public DateTime? dEndDt { get; set; }

       [MaxLength(200)]
       public string vEntryBy { get; set; }
       public long? tTimestamp { get; set; }

       public virtual POS_Setup POS_Setup { get; set; }
       public virtual ICollection<POS_ItemMaster> POS_ItemMasters { get; set; }

       public POS_ItemCategory()
       {

           POS_ItemMasters = new List<POS_ItemMaster>();

       }
    }
}
