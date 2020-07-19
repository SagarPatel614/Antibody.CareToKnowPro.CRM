using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class ProvinceMapper
    {
        public static DTO.Province Map(this Province obj)
        {
            return new DTO.Province()
            {
                ProvinceId = obj.ProvinceId,
                FrenchName = obj.FrenchName,
                EnglishName = obj.EnglishName,
                Abbreviation = obj.Abbreviation
            };
        }
    }
}
