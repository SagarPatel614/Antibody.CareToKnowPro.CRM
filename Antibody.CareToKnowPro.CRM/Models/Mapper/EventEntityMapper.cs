using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class EventEntityMapper
    {
        public static DTO.EventEntity Map(this EventEntity obj)
        {
            return new DTO.EventEntity
            {
                EventEntityId = obj.EventEntityId,
                EventId = obj.EventId,
                ActionType = obj.ActionType,
                Position = obj.Position,
                UserId = obj.UserId,
                Event = obj.Event?.Map(),
                EventEntityProperty = obj.EventEntityProperty.Select(a=>a?.Map()).ToList()
            };
        }
    }
}
