using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int IdRole { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
