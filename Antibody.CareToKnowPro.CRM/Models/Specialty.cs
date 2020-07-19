using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class Specialty
    {
        public Specialty()
        {
            UserSpecialty = new HashSet<UserSpecialty>();
        }

        public int SpecialtyId { get; set; }
        [Column("SpecialtyNameEN")]
        [StringLength(300)]
        public string SpecialtyNameEn { get; set; }
        [Column("SpecialtyNameFR")]
        [StringLength(300)]
        public string SpecialtyNameFr { get; set; }
        public int? Position { get; set; }
        public int? SpecialtyGroupId { get; set; }

        [ForeignKey("SpecialtyGroupId")]
        [InverseProperty("Specialty")]
        public virtual SpecialtyGroup SpecialtyGroup { get; set; }
        [InverseProperty("Speciality")]
        public virtual ICollection<UserSpecialty> UserSpecialty { get; set; }
    }
}
