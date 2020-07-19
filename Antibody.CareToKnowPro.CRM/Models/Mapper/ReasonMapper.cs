using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class ReasonMapper
    {
        public static DTO.UnsubscribeReasons Map(this UnsubscribeReasons obj)
        {
            return new DTO.UnsubscribeReasons
            {
                ReasonId = obj.ReasonId,
                Reason = obj.Reason,
                ReasonFr = obj.ReasonFr
            };
        }
    }
}
