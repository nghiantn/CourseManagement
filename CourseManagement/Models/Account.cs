using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Calendars = new HashSet<Calendar>();
            Learns = new HashSet<Learn>();
        }

        public int IdAccount { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public int? IdRole { get; set; }
        public bool Active { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<Learn> Learns { get; set; }
    }
}
