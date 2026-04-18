using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submitted
    {
        public DateTime SubmissionTime { get; set; }
        public uint Score { get; set; }
        public string Contents { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
