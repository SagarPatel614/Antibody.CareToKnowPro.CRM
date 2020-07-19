using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class ContactReasons
    {
        [Key]
        public int ReasonId { get; set; }
        [Column("ReasonEN")]
        [StringLength(400)]
        public string ReasonEn { get; set; }
        [Column("ReasonFR")]
        [StringLength(400)]
        public string ReasonFr { get; set; }
        public int? Position { get; set; }
    }
}
