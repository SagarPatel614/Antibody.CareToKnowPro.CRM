using System.ComponentModel.DataAnnotations;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class ContactUsForms
    {
        [Key]
        public int ContactId { get; set; }
        [StringLength(200)]
        public string FirstName { get; set; }
        [StringLength(200)]
        public string LastName { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(2)]
        public string Language { get; set; }
        [StringLength(200)]
        public string Reason { get; set; }
        public string Comments { get; set; }
    }
}
