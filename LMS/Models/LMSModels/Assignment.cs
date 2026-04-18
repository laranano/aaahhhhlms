using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignment
    {
        public Assignment()
        {
            Submitteds = new HashSet<Submitted>();
        }

        public string AssignmentName { get; set; } = null!;
        public uint MaxPoints { get; set; }
        public string Contents { get; set; } = null!;
        public int AssignmentId { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignmentCategoryId { get; set; }

        public virtual AssignmentCategory AssignmentCategory { get; set; } = null!;
        public virtual ICollection<Submitted> Submitteds { get; set; }
    }
}
