using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class EventEntityPropertyMapper
    {
        public static DTO.EventEntityProperty Map(this EventEntityProperty obj)
        {
            return new DTO.EventEntityProperty
            {
                EventEntityPropertyId = obj.EventEntityPropertyId,
                EventEntityId = obj.EventEntityId,
                PropertyName = obj.PropertyName,
                OriginalValue = obj.OriginalValue,
                NewValue = obj.NewValue,
                //EventEntity = obj.EventEntity.Map()
            };
        }
    }
}
