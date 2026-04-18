using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Class
    {
        public Class()
        {
            AssignmentCategories = new HashSet<AssignmentCategory>();
            EnrolledIns = new HashSet<EnrolledIn>();
        }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int ClassId { get; set; }
        public ushort Year { get; set; }
        public string Season { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string ProfessorId { get; set; } = null!;
        public uint CourseNum { get; set; }
        public string CourseDeptAbbreviation { get; set; } = null!;

        public virtual Professor Professor { get; set; } = null!;
        public virtual ICollection<AssignmentCategory> AssignmentCategories { get; set; }
        public virtual ICollection<EnrolledIn> EnrolledIns { get; set; }
    }
}
