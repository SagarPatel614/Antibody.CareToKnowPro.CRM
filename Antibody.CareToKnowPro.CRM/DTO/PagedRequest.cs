using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class PagedRequest
    {
        public Filter Filter { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
