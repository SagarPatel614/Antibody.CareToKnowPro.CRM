using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class Specialty
    {
        public int SpecialtyId { get; set; }
        public string SpecialtyNameEn { get; set; }
        public string SpecialtyNameFr { get; set; }
        public int? Position { get; set; }
        public int? SpecialtyGroupId { get; set; }
    }
}
