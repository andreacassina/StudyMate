﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace StudyMate.Models
{
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
        }

        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string DegreeCourse { get; set; }  // piano di studi

        public virtual ICollection<Event> Events { get; set; }
    }
}
