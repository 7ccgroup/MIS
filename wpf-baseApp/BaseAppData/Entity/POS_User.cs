using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BaseAppData.Entity
{
    public class POS_User
    {
        [Key]
        public int ID { get; set; }
        public int iUserId { get; set; }

        [MaxLength(10)]
        public string vUserCode { get; set; }

        [MaxLength(10)]
        public string vUserAltCode { get; set; }
        [MaxLength(100)]
        public string vUserName { get; set; }
        [MaxLength(10)]
        public string vUserType { get; set; }
        [MaxLength(10)]
        public string vUserOLS { get; set; }
        [MaxLength(10)]
        public string vUserStartTime { get; set; }
        [MaxLength(10)]
        public string vUserWorkShift { get; set; }
        [MaxLength(10)]
        public string vStatus { get; set; }

        public DateTime dStartDt { get; set; }
        public DateTime dEndDt { get; set; }

        [MaxLength(8)]
        public string vEntryBy { get; set; }

        public long? tTimestamp { get; set; }


        public POS_User()
        {


        }

    }
}
