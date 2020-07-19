using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Antibody.CareToKnowPro.CRM.Helpers;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class UserToken
    {
        [Key]
        [Column("TokenID")]
        public int TokenId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(70)]
        public string Token { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime CreatedOn { get; set; }
        public TokenType? Type { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserToken")]
        public virtual User User { get; set; }
    }
}
