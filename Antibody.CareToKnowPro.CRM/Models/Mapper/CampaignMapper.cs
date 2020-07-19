using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.Helpers;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class CampaignMapper
    {
        public static DTO.Campaign Map(this Campaign obj)
        {
            return new DTO.Campaign()
            {
                Id = obj.id,
                DeduplicateId = obj.deduplicate_id,
                Name = obj.name,
                CampaignType = obj.type,
                Created = obj.created,
                Updated = obj.updated,
                Active = obj.active,
                State = obj.state,
                FirstStarted = obj.first_started,
                CreatedBy = obj.created_by,
                EventName = obj.event_name
            };

        }
    }
}
