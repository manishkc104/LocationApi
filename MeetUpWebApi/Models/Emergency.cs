using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class Emergency
    {
        [Key]
        public int EmergencyID { get; set; }
        public string EmergencyName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CreatedBy { get; set; }
    }
}   
