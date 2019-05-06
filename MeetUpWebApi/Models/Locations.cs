using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class Locations
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationTitle { get; set; }
        public string LocationDesc { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MeetupDate { get; set; }
        public string MeetupTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
