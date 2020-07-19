using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class PagedResponse<T> where T : class
    {
        public int TotalCount { get; private set; }
        public int RecordsPerPage { get; private set; }
        public int CurrentPage { get; private set; }

        public IEnumerable<T> Results { get; private set; }

        public PagedResponse(IEnumerable<T> results, int totalCount, int recordsPerPage, int currentPage)
        {
            this.Results = results;
            this.TotalCount = totalCount;
            this.RecordsPerPage = recordsPerPage;
            this.CurrentPage = currentPage;
        }
    }
}
