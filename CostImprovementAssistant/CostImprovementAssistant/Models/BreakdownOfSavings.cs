namespace CostImprovementAssistant.Models
{
    public class BreakdownOfSaving
    {
        public int SchemeId { get; set; }
        public string BudgetTypeNameA { get; set; }
        public string BudgetTypeNameB { get; set; }
        public decimal? Wte { get; set; }
        public decimal? Year1Saving { get; set; }
        public decimal? ReccurentSavingValue { get; set; }
        public string TrustCode { get; set; }
    }
}
