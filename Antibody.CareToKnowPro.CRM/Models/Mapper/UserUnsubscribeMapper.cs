using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class UserUnsubscribeMapper
    {
        public static DTO.UserUnsubscribe Map(this UserUnsubscribe obj)
        {
            return new DTO.UserUnsubscribe
            {
                UnsubscribeId = obj.UnsubscribeId,
                UserId = obj.UserId,
                ReasonId = obj.ReasonId,
                Other = obj.Other,
                Reason = obj.Reason?.Map()
            };
        }
    }
}
