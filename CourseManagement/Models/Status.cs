using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Status
    {
        public Status()
        {
            Contacts = new HashSet<Contact>();
        }

        public int IdStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
