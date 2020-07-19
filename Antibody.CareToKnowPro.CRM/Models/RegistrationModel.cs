using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public class RegistrationModel
    {
        public RegistrationModel()
        {
            ProvinceList = new List<SelectListItem>();
            GraduationList = new List<SelectListItem>();
            SpecialtyIds = new List<int>();
        }


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public string GraduationYear { get; set; }

        [Required]
        public string PreferredLanguage { get; set; }
        public bool? Registered { get; set; }
        [Required]
        public List<int> SpecialtyIds { get; set; }
        public string SecondaryEmails { get; set; }
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

        public List<SelectListItem> ProvinceList { get; set; }

        public List<SelectListItem> GraduationList { get; set; }

        public IEnumerable<IGrouping<SelectListGroup, SelectListItem>> Specialities { get; set; }


        public MultiSelectList SpecialtyList { get; set; }

        //[Required]
        //public bool ConfirmPolicy { get; set; }

        public string ErrorMessage { get; set; }
        public bool IsError { get; set; }
        public int? UserId { get; set; }
        public string Fax { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
