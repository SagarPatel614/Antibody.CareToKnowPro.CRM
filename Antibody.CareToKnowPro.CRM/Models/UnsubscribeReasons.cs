using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class UnsubscribeReasons
    {
        public UnsubscribeReasons()
        {
            UserUnsubscribe = new HashSet<UserUnsubscribe>();
        }

        [Key]
        public int ReasonId { get; set; }
        [StringLength(200)]
        public string Reason { get; set; }
        [Column("ReasonFR")]
        [StringLength(200)]
        public string ReasonFr { get; set; }

        [InverseProperty("Reason")]
        public virtual ICollection<UserUnsubscribe> UserUnsubscribe { get; set; }
    }
}
