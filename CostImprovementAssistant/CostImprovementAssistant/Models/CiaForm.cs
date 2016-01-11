using System;

namespace CostImprovementAssistant.Models
{
        public class CiaForm
        {
        public int SchemeID { get; set; }
        public DateTime? DateofScheme { get; set; }
        public string SchemeName { get; set; }
        public string CategoryName { get; set; }
        public string DepartmentName { get; set; }
        public string BusinessLevelName { get; set; }
        public string BusinessSectorName { get; set; }
        public string Overview { get; set; }
        public string CurrentState { get; set; }
        public string TargetState { get; set; }
        public string ProjectSupervisor { get; set; }
        public string ProjectManager { get; set; }
        public string BudgetSupervisor { get; set; }
        public string HrSupervisor { get; set; }
        public string ExecutiveOwner { get; set; }
        public decimal? ConfidenceFactor { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public decimal? BenefitExpectedInCiaYear { get; set; }
        public decimal? ReccuringAnualBenefit { get; set; }
        public decimal? EnablingCosts { get; set; }
        public decimal? NettCiaYearSavings { get; set; }
        public string PSAuthorisationCode { get; set; }
        public DateTime? PSAuthorisationDate { get; set; }
        public string BSAuthorisationCode { get; set; }
        public DateTime? BSAuthorisationDate { get; set; }
        public decimal RiskAdjustedValue { get; set; }
        public string ConfidenceCode { get; set; }
        public string TrustCode { get; set; }
        public bool Active { get; set; }
        public bool QIRequired { get; set; }
    
    }
}
