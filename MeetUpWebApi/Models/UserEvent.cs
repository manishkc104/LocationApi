﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetUpWebApi.Models
{
    public class UserEvent
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int EventID { get; set; }
    }
}
