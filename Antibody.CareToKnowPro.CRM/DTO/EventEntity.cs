using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class EventEntity
    {
        public long EventEntityId { get; set; }
        public long EventId { get; set; }
        public int ActionType { get; set; }
        public int Position { get; set; }
        public int UserId { get; set; }
        public Event Event { get; set; }
        public List<EventEntityProperty> EventEntityProperty { get; set; }
    }
}
