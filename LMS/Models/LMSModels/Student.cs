using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Student
    {
        public Student()
        {
            EnrolledIns = new HashSet<EnrolledIn>();
            Submitteds = new HashSet<Submitted>();
        }

        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MajorDeptAbbreviation { get; set; } = null!;
        public DateOnly Dob { get; set; }

        public virtual Department MajorDeptAbbreviationNavigation { get; set; } = null!;
        public virtual ICollection<EnrolledIn> EnrolledIns { get; set; }
        public virtual ICollection<Submitted> Submitteds { get; set; }
    }
}
