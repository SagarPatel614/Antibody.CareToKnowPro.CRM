using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.Helpers;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class MessageMapper
    {
        public static DTO.Message Map(this Message obj)
        {
            var msg = new DTO.Message()
            {
                Id = obj.id,
                BroadcastId = obj.broadcast_id,
                CampaignId = obj.campaign_id,
                ContentId = obj.content_id,
                Created = obj.created,
                CustomerId = obj.customer_id,
                DeduplicateId = obj.deduplicate_id,
                FailureMessage = obj.failure_message,
                Forgotten = obj.forgotten,
                MsgTemplateId = obj.msg_template_id,
                ActionId = obj.action_id,
                NewsletterId = obj.newsletter_id,
                Recipient = obj.recipient,
                Subject = obj.subject,
                MessageType = obj.type
            };

            List<MessageEvent> events = new List<MessageEvent>();

            foreach (KeyValuePair<string, string> metric in obj.metrics)
            {
                if (metric.Key.ToUpper().Contains("LINK"))
                {
                    continue;
                }
                MessageEvent msgEvent = new MessageEvent
                {
                    Name = metric.Key.FirstCharToUpper(),
                    EventDateTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(metric.Value)).DateTime.ToLocalTime()
                };
                events.Add(msgEvent);
            }

            msg.Metrics = new DTO.Metrics()
            {
                Events = events.OrderByDescending(a => a.EventDateTime).ToList()
            };

            return msg;
        }
    }
}
