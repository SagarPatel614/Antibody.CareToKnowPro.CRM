using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class Role
    {
        public Role()
        {
            LoginProfileRole = new HashSet<LoginProfileRole>();
        }

        public int RoleId { get; set; }
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Description { get; set; }
        [StringLength(20)]
        public string Permission { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<LoginProfileRole> LoginProfileRole { get; set; }
    }
}
