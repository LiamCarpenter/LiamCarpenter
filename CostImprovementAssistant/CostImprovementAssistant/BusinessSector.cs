//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CostImprovementAssistant
{
    using System;
    using System.Collections.Generic;
    
    public partial class BusinessSector
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BusinessSector()
        {
            this.BusinessLevel = new HashSet<BusinessLevel>();
            this.CiaForm = new HashSet<CiaForm>();
        }
    
        public string BusinessSectorName { get; set; }
        public Nullable<decimal> TargetValue { get; set; }
        public string TrustCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessLevel> BusinessLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CiaForm> CiaForm { get; set; }
    }
}
