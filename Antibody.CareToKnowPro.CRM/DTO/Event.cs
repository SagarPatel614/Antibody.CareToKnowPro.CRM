using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.Models;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class Event
    {
        public long EventId { get; set; }
        public int EventType { get; set; }
        public DateTime EventDateUtc { get; set; }
        public int LoginProfileId { get; set; }
        public string EventNotes { get; set; }
        public LoginProfile LoginProfile { get; set; }
        public List<EventEntity> EventEntity { get; set; }
    }
}
