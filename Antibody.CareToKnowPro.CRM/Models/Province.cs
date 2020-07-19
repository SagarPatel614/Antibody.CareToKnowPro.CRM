using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class Province
    {
        public Province()
        {
            User = new HashSet<User>();
        }

        [Column("ProvinceID")]
        public int ProvinceId { get; set; }
        [Required]
        [StringLength(75)]
        public string FrenchName { get; set; }
        [Required]
        [StringLength(75)]
        public string EnglishName { get; set; }
        [StringLength(3)]
        public string Abbreviation { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<User> User { get; set; }
    }
}
