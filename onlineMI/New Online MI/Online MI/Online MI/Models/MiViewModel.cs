using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_MI.Models
{
    public class MiViewModel
    {
        public IList<Online_MI.Models.Dashboardv2_Result> Dashboard { get; set; }
        public IEnumerable<Online_MI.Models.ComplaintsSummary_Result> ComplaintsSummary { get; set; }
    }
}