using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class UserUnsubscribe
    {
        public int UnsubscribeId { get; set; }
        public int UserId { get; set; }
        public int? ReasonId { get; set; }
        public string Other { get; set; }
        public UnsubscribeReasons Reason { get; set; }
    }
}
