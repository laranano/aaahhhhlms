using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public string Name { get; set; } = null!;
        public uint CourseNum { get; set; }
        public string CourseDeptAbbreviation { get; set; } = null!;
        public uint CatalogId { get; set; }

        public virtual Department CourseDeptAbbreviationNavigation { get; set; } = null!;
    }
}
