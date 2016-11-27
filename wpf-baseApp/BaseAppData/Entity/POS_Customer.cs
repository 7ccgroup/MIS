using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseAppData.Entity
{
   public class POS_Customer
    {
       [Key]
       public int Id { get; set; }
       public int iCustid { get; set; }

        [MaxLength(50)]
       public string vCustName { get; set; }
        [MaxLength(50)]
       public string vCustContactNme { get; set; }
        [MaxLength(50)]
       public string vCustPrimaryPh { get; set; }
        [MaxLength(50)]
       public string vCustPhone2 { get; set; }
        [MaxLength(50)]
       public string vCustPhone3 { get; set; }
        [MaxLength(50)]
       public string vCustPhone4 { get; set; }
        [MaxLength(50)]
       public string vCustFax1 { get; set; }
        [MaxLength(50)]
       public string vCustFax2 { get; set; }
        [MaxLength(50)]
       public string vCustEmail { get; set; }
        [MaxLength(50)]
       public string vCustAddress1 { get; set; }
        [MaxLength(50)]
       public string vCustAddress2 { get; set; }
        [MaxLength(50)]
       public string vCustCity { get; set; }
        [MaxLength(50)]
       public string vCustState { get; set; }
        [MaxLength(50)]
       public string vCustCountry { get; set; }
        [MaxLength(50)]
       public string vCustZipCode { get; set; }
        [MaxLength(50)]
       public string vCustBillAddress1 { get; set; }
        [MaxLength(50)]
       public string vCustBillAddress2 { get; set; }
        [MaxLength(50)]
       public string vCustBillCity { get; set; }
        [MaxLength(50)]
       public string vCustBillCountry { get; set; }
        [MaxLength(50)]
       public string vCustBillZipCode { get; set; }
        [MaxLength(50)]
        public string vCustShipAddress1 { get; set; }
        [MaxLength(50)]
        public string vCustShipAddress2 { get; set; }
        [MaxLength(50)]
        public string vCustShipCity { get; set; }
        [MaxLength(50)]
        public string vCustShipState { get; set; }
        [MaxLength(50)]
        public string vCustShipCountry { get; set; }
        [MaxLength(50)]
        public string vCustShipZipCode { get; set; }
        [MaxLength(50)]
        public string vCustAccountNum { get; set; }
        [MaxLength(255)]
        public string vCustNote1 { get; set; }
        [MaxLength(255)]
        public string vCustNote2 { get; set; }
        [MaxLength(255)]
        public string vCustNote3 { get; set; }
       [MaxLength(8)]
       public string vCustCategory { get; set; }
       [MaxLength(255)]
       public string vCustComments { get; set; }
       [MaxLength(8)]
       public string vCustStatus { get; set; }
       [MaxLength(8)]
       public string vEntryBy { get; set; }      

       public long? tTimestamp { get; set; }

       public virtual POS_Setup POS_Setup { get; set; }

       public virtual ICollection<POS_OrderHeader> OrderHeaders { get; set; }
       public POS_Customer()
       {

           OrderHeaders = new List<POS_OrderHeader>();
       }
        
    }
}
