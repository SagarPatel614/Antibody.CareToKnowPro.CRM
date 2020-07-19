using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomerIOSharp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class User
    {
        public User()
        {
            ProvinceList = new List<SelectListItem>();
            GraduationList = new List<SelectListItem>();
        }

        public int UserId { get; set; }
        
        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public int? GraduationYear { get; set; }
        
        public string PasswordHash { get; set; }
       
        public byte[] Encrypted { get; set; }
        public int? UserType { get; set; }
        public int? ProvinceId { get; set; }

        public string SecondaryEmails { get; set; }

        public string PreferredLanguage { get; set; }
        public bool? Verified { get; set; }
        public Guid? UserGuid { get; set; }
        public bool? Registered { get; set; }

        public string Other { get; set; }

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

        public Province Province { get; set; }

        public List<SelectListItem> ProvinceList { get; set; }

        public List<SelectListItem> GraduationList { get; set; }


        public IEnumerable<IGrouping<SelectListGroup, SelectListItem>> Specialities { get; set; }


        public virtual ICollection<EventEntity> EventEntity { get; set; }
        
        public virtual ICollection<UserSpecialty> UserSpecialty { get; set; }
      
        public virtual ICollection<UserUnsubscribe> UserUnsubscribe { get; set; }
        public string Fax { get; set; }
        public string AdditionalInfo { get; set; }
        public List<Message> Messages { get; set; }

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
                Registered = this.Registered.ToString()
            };
        }
        public NewCustomer AsContactCustomer()
        {
            return new NewCustomer
            {
                Email = this.Email
            };
        }
    }

    public class Customer : ICustomerDetails
    {
        // these two fields are required:
        public string Id { get; set; }
        public string Email { get; set; }
        // these are my custom fields:
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Unsubscribed { get; set; }
        public string Verified { get; set; }
        public string Registered { get; set; }

        public string UserGuid { get; set; }
    }

    public class NewCustomer : ICustomerDetails
    {
        // these two fields are required:
        public string Id { get; set; }
        public string Email { get; set; }
        // these are my custom fields:
        public long created_at
        {
            get
            {
                var dat = DateTimeOffset.Now.ToUnixTimeSeconds();
                return dat;
            }
        }

        public string UserGuid { get; set; }
        public string Registered { get; set; }
        public string locale { get; set; }
        public bool? verified { get; set; }
        public string language { get; set; }
        public bool? active_crm { get; set; }
    }
}
