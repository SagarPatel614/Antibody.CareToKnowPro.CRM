using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class EventMapper
    {
        public static DTO.Event Map(this Event obj)
        {
            return new DTO.Event
            {
                EventId = obj.EventId,
                EventType = obj.EventType,
                EventDateUtc = obj.EventDateUtc,
                LoginProfileId = obj.LoginProfileId,
                EventNotes = obj.EventNotes,
                LoginProfile = obj.LoginProfile?.Map(),
                //EventEntity = obj.EventEntity.Select(a => a.Map()).ToList()
            };
        }
    }
}
