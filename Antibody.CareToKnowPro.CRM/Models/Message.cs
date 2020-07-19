using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Antibody.CareToKnowPro.CRM.Models
{
    [Serializable]
    public partial class RootObject
    {
        public List<Message> messages { get; set; }
    }

    [Serializable]
    public class Message
    {
        public string id { get; set; }
        public string deduplicate_id { get; set; }
        public string msg_template_id { get; set; }
        public string action_id { get; set; }
        public string customer_id { get; set; }
        public string recipient { get; set; }
        public string subject { get; set; }
        //public Metrics metrics { get; set; }
       // [JsonConverter(typeof(UnixDateTimeConverter))]
        public Dictionary<string, string> metrics { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? created { get; set; }
        public string failure_message { get; set; }
        public string newsletter_id { get; set; }
        public string content_id { get; set; }
        public string campaign_id { get; set; }
        public string broadcast_id { get; set; }
        public string type { get; set; }
        public bool? forgotten { get; set; }
    }
    [Serializable]
    public class Metrics
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? delivered { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? drafted { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? opened { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? sent { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? bounced { get; set; }
    }
}
