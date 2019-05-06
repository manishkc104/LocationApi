using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }
        public string EventTitle { get; set; }
        public string EventDesc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CreatedBy { get; set; }
    }

}
