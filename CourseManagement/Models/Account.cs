using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            ClassIdLecturerNavigations = new HashSet<Class>();
            ClassIdStudentNavigations = new HashSet<Class>();
        }

        public int IdAccount { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public int? IdRole { get; set; }
        public bool Active { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual ICollection<Class> ClassIdLecturerNavigations { get; set; }
        public virtual ICollection<Class> ClassIdStudentNavigations { get; set; }
    }
}
