using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BaseAppData.Entity
{
    public class POS_CodesTable
    {
        [Key]
        public int ID { get; set; }
        public int iCorpId { get; set; }

        [MaxLength(10)]
        public string vTabCode { get; set; }

        [MaxLength(10)]
        public string vFldCode { get; set; }
        [MaxLength(255)]
        public string vCodeDesc { get; set; }
        [MaxLength(3)]
        public string vCodeSeq { get; set; }
        [MaxLength(50)]
        public string vAltCode { get; set; }
        [MaxLength(10)]
        public string vCodePurpose { get; set; }
        [MaxLength(10)]
        public string vStatus { get; set; }
        
        public DateTime dStartDt { get; set; }
        public DateTime dEndDt { get; set; }

        [MaxLength(8)]
        public string vEntryBy { get; set; }

        public long? tTimestamp { get; set; }


        public POS_CodesTable()
        {

            
        }

    }
}
