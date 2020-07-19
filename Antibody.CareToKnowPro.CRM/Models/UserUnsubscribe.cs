using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class UserUnsubscribe
    {
        [Key]
        public int UnsubscribeId { get; set; }
        public int UserId { get; set; }
        public int? ReasonId { get; set; }
        [Column("other")]
        [StringLength(200)]
        public string Other { get; set; }

        [ForeignKey("ReasonId")]
        [InverseProperty("UserUnsubscribe")]
        public virtual UnsubscribeReasons Reason { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("UserUnsubscribe")]
        public virtual User User { get; set; }
    }
}
