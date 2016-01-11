using System;

namespace CostImprovementAssistant.Models
{
    public class Milestones
    {
        public int SchemeID { get; set; }
        public int MilestonesID { get; set; }
        public string MilestonesDetail { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public DateTime? RevisedDate { get; set; }
        public string RevisionReason { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public string TrustCode { get; set; }
    }
}
