namespace CostImprovementAssistant.Models
{
    public class BreakdownOfEnablingCost
    {
        public int SchemeId { get; set; }
        public string BudgetTypeNameA { get; set; }
        public string BudgetTypeNameB { get; set; }
        public decimal? WTE { get; set; }
        public decimal? Costs { get; set; }
        public string TrustCode { get; set; }
    }
}
