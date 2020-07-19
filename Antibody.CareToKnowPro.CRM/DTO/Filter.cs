using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class Filter
    {
        public Filter()
        {
            ProvinceList = new List<SelectListItem>();
            GraduationList = new List<SelectListItem>();
            SpecialtyIds = new List<int>();
            ProvinceIds = new List<int>();
        }

        public List<SelectListItem> GraduationList { get; set; }
        public List<SelectListItem> ProvinceList { get; set; }
        public IEnumerable<IGrouping<SelectListGroup, SelectListItem>> Specialties { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<int> ProvinceIds { get; set; }
        public int? GraduationYear { get; set; }
        public string PreferredLanguage { get; set; }
        public string EmailStatus { get; set; }
        public string Status { get; set; }
        public List<int> SpecialtyIds { get; set; }

        public string OtherSpecialties { get; set; }

        public string[] OtherSpecialtiesArray => OtherSpecialties.Trim().Split(",").Select(a => a.Trim()).ToArray();
    }
}
