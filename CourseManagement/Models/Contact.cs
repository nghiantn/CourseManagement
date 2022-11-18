using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Contact
    {
        public int IdContact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public int IdCalendar { get; set; }
        public int IdStatus { get; set; }

        public virtual Calendar IdCalendarNavigation { get; set; }
        public virtual Status IdStatusNavigation { get; set; }
    }
}
