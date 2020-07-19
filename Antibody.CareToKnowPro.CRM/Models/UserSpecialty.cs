using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class UserSpecialty
    {
        public int UserSpecialtyId { get; set; }
        public int UserId { get; set; }
        public int SpecialityId { get; set; }
        [StringLength(200)]
        public string SpecialtyOther { get; set; }

        [ForeignKey("SpecialityId")]
        [InverseProperty("UserSpecialty")]
        public virtual Specialty Speciality { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("UserSpecialty")]
        public virtual User User { get; set; }
    }
}
