using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class Event
    {
        public Event()
        {
            EventEntity = new HashSet<EventEntity>();
        }

        public long EventId { get; set; }
        public int EventType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EventDateUtc { get; set; }
        public int LoginProfileId { get; set; }
        public string EventNotes { get; set; }

        [ForeignKey("LoginProfileId")]
        [InverseProperty("Event")]
        public virtual LoginProfile LoginProfile { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<EventEntity> EventEntity { get; set; }
    }
}
