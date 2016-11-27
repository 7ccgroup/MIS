using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BaseAppData.Entity
{
    public class POS_TimeSheet
    {
        [Key]
        public int ID { get; set; }
        public int iUserId { get; set; }

        [MaxLength(10)]
        public string vUserCode { get; set; }

        public string vUserStartTime { get; set; }
        [MaxLength(10)]
        public string vUserEndTime { get; set; }
        [MaxLength(10)]
        public string vStatus { get; set; }

        public DateTime dShiftStartDt { get; set; }
        public DateTime dShiftEndDt { get; set; }

        [MaxLength(8)]
        public string vEntryBy { get; set; }

        public long? tTimestamp { get; set; }


        public POS_TimeSheet()
        {


        }

    }
}
