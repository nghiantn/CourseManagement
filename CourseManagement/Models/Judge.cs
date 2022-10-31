using System;
using System.Collections.Generic;

namespace CourseManagement.Models
{
    public partial class Judge
    {
        public int IdJudge { get; set; }
        public string Judge1 { get; set; }
        public int? IdClass { get; set; }

        public virtual Class IdClassNavigation { get; set; }
    }
}
