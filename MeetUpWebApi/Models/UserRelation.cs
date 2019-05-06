using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class UserRelation
    {
        [Key]
        public int UserA { get; set; }
        [Key]
        public int UserB { get; set; }
        public string StatusA { get; set; }
        public string StatusB { get; set; }
    }
}
