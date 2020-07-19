using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class Message
    {
        public string Id { get; set; }
        public string DeduplicateId { get; set; }
        public string MsgTemplateId { get; set; }
        public string ActionId { get; set; }
        public string CustomerId { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public Metrics Metrics { get; set; }
        public DateTime? Created { get; set; }
        public string FailureMessage { get; set; }
        public string NewsletterId { get; set; }
        public string ContentId { get; set; }
        public string CampaignId { get; set; }
        public string BroadcastId { get; set; }
        public string MessageType { get; set; }
        public bool? Forgotten { get; set; }
        public Campaign Campaign { get; set; }
    }

    public class Metrics
    {
       public List<MessageEvent> Events { get; set; }
    }

    public class MessageEvent
    {
        public string Name { get; set; }
        public DateTime? EventDateTime { get; set; }
    }
}
