using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class EventEntity
    {
        public EventEntity()
        {
            EventEntityProperty = new HashSet<EventEntityProperty>();
        }

        public long EventEntityId { get; set; }
        public long EventId { get; set; }
        public int ActionType { get; set; }
        public int Position { get; set; }
        public int UserId { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("EventEntity")]
        public virtual Event Event { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EventEntity")]
        public virtual User User { get; set; }
        [InverseProperty("EventEntity")]
        public virtual ICollection<EventEntityProperty> EventEntityProperty { get; set; }
    }
}
