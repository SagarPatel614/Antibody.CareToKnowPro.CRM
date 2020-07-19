using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class LoginProfile
    {
        public LoginProfile()
        {
            Event = new HashSet<Event>();
            LoginProfileRole = new HashSet<LoginProfileRole>();
        }
        [NotMapped]
        [Required]
        public string Password { get; set; }

        public int LoginProfileId { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        
        public string PasswordHash { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(200)]
        public string Street1 { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(10)]
        public string ProvCode { get; set; }
        [StringLength(20)]
        public string Postal { get; set; }
        [StringLength(40)]
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        [Required]
        public bool? Status { get; set; }
        [StringLength(40)]
        public string ProfileQuestion { get; set; }
        [StringLength(200)]
        public string ProfileAnswer { get; set; }

        [InverseProperty("LoginProfile")]
        public virtual ICollection<Event> Event { get; set; }
        [InverseProperty("LoginProfile")]
        public virtual ICollection<LoginProfileRole> LoginProfileRole { get; set; }
    }
}
