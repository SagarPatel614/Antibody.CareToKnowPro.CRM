using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Antibody.CareToKnowPro.CRM.Models
{
    [Serializable]
    public class ParentObject
    {
        public Campaign campaign { get; set; }
    }

    [Serializable]
    public class Actions
    {
        public int id { get; set; }
        public string type { get; set; }

    }

    [Serializable]
    public class Campaign
    {
        public int? id { get; set; }
        public string deduplicate_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? created { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? updated { get; set; }
        public bool? active { get; set; }
        public string state { get; set; }
        //  public IList<Actions> actions { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? first_started { get; set; }
        public string created_by { get; set; }
     //   public IList<string> tags { get; set; }
        public string event_name { get; set; }
    }
}
