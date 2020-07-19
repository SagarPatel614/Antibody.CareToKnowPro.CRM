using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class UserSpecialty
    {
        public int UserSpecialtyId { get; set; }
        public int UserId { get; set; }
        public Specialty Speciality { get; set; }
        public string SpecialtyOther { get; set; }
    }
}
