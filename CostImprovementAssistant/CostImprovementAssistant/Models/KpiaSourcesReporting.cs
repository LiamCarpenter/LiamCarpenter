using System;

namespace CostImprovementAssistant.Models
{
    public class KpiaSourcesReporting
    {
        public string KPIAssurance { get; set; }
        public string TrustCode { get; set; }

        public static implicit operator KpiaSourcesReporting(CostImprovementAssistant.KpiaSourcesReporting v)
        {
            throw new NotImplementedException();
        }
    }
}
