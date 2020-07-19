using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class SpecialityMapper
    {
        public static DTO.Specialty Map(this Specialty obj)
        {
            return new DTO.Specialty
            {
                SpecialtyId = obj.SpecialtyId,
                SpecialtyNameEn = obj.SpecialtyNameEn,
                SpecialtyNameFr = obj.SpecialtyNameFr,
                Position = obj.Position,
                SpecialtyGroupId = obj.SpecialtyGroupId
            };
        }
    }
}
