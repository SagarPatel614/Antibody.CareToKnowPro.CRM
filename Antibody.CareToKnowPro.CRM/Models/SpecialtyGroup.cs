using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class SpecialtyGroup
    {
        public SpecialtyGroup()
        {
            Specialty = new HashSet<Specialty>();
        }

        public int SpecialtyGroupId { get; set; }
        [Column("GroupNameEN")]
        public string GroupNameEn { get; set; }
        [Column("GroupNameFR")]
        public string GroupNameFr { get; set; }
        public int? Position { get; set; }

        [InverseProperty("SpecialtyGroup")]
        public virtual ICollection<Specialty> Specialty { get; set; }
    }
}
