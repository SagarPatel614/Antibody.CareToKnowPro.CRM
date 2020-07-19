using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class EventEntityProperty
    {
        public long EventEntityPropertyId { get; set; }
        public long EventEntityId { get; set; }
        [Required]
        [StringLength(100)]
        public string PropertyName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }

        [ForeignKey("EventEntityId")]
        [InverseProperty("EventEntityProperty")]
        public virtual EventEntity EventEntity { get; set; }
    }
}
