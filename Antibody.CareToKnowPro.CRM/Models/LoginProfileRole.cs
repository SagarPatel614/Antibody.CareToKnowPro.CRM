using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class LoginProfileRole
    {
        public int LoginProfileRoleId { get; set; }
        public int? LoginProfileId { get; set; }
        public int? RoleId { get; set; }
        [StringLength(10)]
        public string Status { get; set; }

        [ForeignKey("LoginProfileId")]
        [InverseProperty("LoginProfileRole")]
        public virtual LoginProfile LoginProfile { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("LoginProfileRole")]
        public virtual Role Role { get; set; }
    }
}
