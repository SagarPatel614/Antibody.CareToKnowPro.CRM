using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Antibody.CareToKnowPro.CRM.DTO;
using CustomerIOSharp;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class User
    {
        public User()
        {
            EventEntity = new HashSet<EventEntity>();
            UserSpecialty = new HashSet<UserSpecialty>();
            UserToken = new HashSet<UserToken>();
            UserUnsubscribe = new HashSet<UserUnsubscribe>();
        }

        public int UserId { get; set; }
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(200)]
        public string FirstName { get; set; }
        [StringLength(200)]
        public string LastName { get; set; }
        public int? GraduationYear { get; set; }
        [StringLength(200)]
        public string PasswordHash { get; set; }
        [MaxLength(64)]
        public byte[] Encrypted { get; set; }
        public int? UserType { get; set; }
        public int? ProvinceId { get; set; }
        [StringLength(2)]
        public string PreferredLanguage { get; set; }
        public bool? Verified { get; set; }
        public Guid? UserGuid { get; set; }
        public bool? Registered { get; set; }
        public string EmailStatus { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string AdditionalInfo { get; set; }
        public string SecondaryEmails { get; set; }

        public bool? Imported { get; set; }

        public bool? Synced { get; set; }

        [Required]
        [NotMapped]
        public List<int> SpecialtyIds { get; set; }
        [NotMapped]
        public string Other { get; set; }

        //[NotMapped]
        //public string SpecialtyText
        //{
        //    get
        //    {
        //        var userSpecialties = this.UserSpecialty;
        //        if (userSpecialties != null)
        //            return string.Join(", ", userSpecialties?.Select(a => a.Speciality.SpecialtyNameEn));
        //        return "";
        //    }
        //}

        [ForeignKey("ProvinceId")]
        [InverseProperty("User")]
        public virtual Province Province { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EventEntity> EventEntity { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserSpecialty> UserSpecialty { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserToken> UserToken { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserUnsubscribe> UserUnsubscribe { get; set; }

        

        public Customer AsCustomer()
        {
            return new Customer
            {
                Email = this.Email,
                Id = this.UserId.ToString(),
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? this.Email : this.FirstName,
                LastName = string.Empty,
                Verified = this.Verified.ToString(),
                UserGuid = this.UserGuid.ToString(),
                Registered = this.Registered.ToString()
            };
        }

        public NewCustomer AsNewCustomer()
        {
            return new NewCustomer
            {
                Email = this.Email,
                Id = this.UserId.ToString(),
                UserGuid = this.UserGuid.ToString(),
                locale = this.PreferredLanguage == "FR" ? "french" : "english",
                Registered = "true",
                language = this.PreferredLanguage == "FR" ? "french" : "english",
                verified = true,
                active_crm = true
            };
        }

        public NewCustomer AsDeleteCustomer()
        {
            return new NewCustomer
            {
                Email = this.Email,
                Id = this.UserId.ToString(),
                UserGuid = this.UserGuid.ToString(),
                locale = this.PreferredLanguage == "FR" ? "french" : "english",
                Registered = "true",
                language = this.PreferredLanguage == "FR" ? "french" : "english",
                verified = true,
                active_crm = false
            };
        }

        public NewCustomer AsContactCustomer()
        {
            return new NewCustomer
            {
                Email = this.Email
            };
        }

        public string GetAddress()
        {
            var street = string.IsNullOrEmpty(Street1) ? "" : $"{Street1} ";
            var city = string.IsNullOrEmpty(City) ? "" : $"{City} ";
            var province = string.IsNullOrEmpty(Province.EnglishName) ? "" : $"{Province.EnglishName} ";
            var country = string.IsNullOrEmpty(Country) ? "" : $"{Country} ";
            var postalCode = string.IsNullOrEmpty(Postal) ? "" : $"{Postal} ";

            return $"{street}{city}{province}{country}{postalCode}";
        }
    }
}
