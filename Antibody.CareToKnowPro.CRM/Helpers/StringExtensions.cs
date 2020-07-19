using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Helpers
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => "",
                "" => "",
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
    }
}
