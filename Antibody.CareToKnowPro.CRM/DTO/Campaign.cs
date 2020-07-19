using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class Campaign
    {
        public int? Id { get; set; }
        public string DeduplicateId { get; set; }
        public string Name { get; set; }
        public string CampaignType { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public string State { get; set; }
        public DateTime? FirstStarted { get; set; }
        public string CreatedBy { get; set; }
        public string EventName { get; set; }
    }
}
