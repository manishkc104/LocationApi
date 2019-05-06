using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class UserEmergency
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int EmergencyID { get; set; }
    }
}
