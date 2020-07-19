using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class EventEntityProperty
    {
        public long EventEntityPropertyId { get; set; }
        public long EventEntityId { get; set; }
        public string PropertyName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public EventEntity EventEntity { get; set; }
    }
}
