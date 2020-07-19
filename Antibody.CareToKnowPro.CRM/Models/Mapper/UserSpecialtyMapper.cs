using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class UserSpecialtyMapper
    {
        public static DTO.UserSpecialty Map(this UserSpecialty obj)
        {
            return new DTO.UserSpecialty
            {
                UserSpecialtyId = obj.UserSpecialtyId,
                UserId = obj.UserId,
                SpecialtyOther = obj.SpecialtyOther,
                Speciality = obj.Speciality?.Map()
            };
        }
    }
}
