using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class LoginProfile
    {
        public int LoginProfileId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string ProvCode { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public bool? Status { get; set; }
    }
}
