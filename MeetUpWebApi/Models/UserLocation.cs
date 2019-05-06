using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class UserLocation
    {
        [Key]
        public int  UserID{ get; set; }
        [Key]
        public int LocationID { get; set; }
    }
}
