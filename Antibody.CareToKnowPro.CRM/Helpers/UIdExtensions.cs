using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Antibody.CareToKnowPro.CRM.Helpers
{
    public static class UIdExtensions
    {
        public static string ToUIdString(this Guid guid)
        {
            if (guid.Equals(Guid.Empty))
            {
                return string.Empty;
            }

            return WebEncoders.Base64UrlEncode(guid.ToByteArray());
        }

        public static Guid ToUIdGuid(this string uId)
        {
            if (string.IsNullOrEmpty(uId))
            {
                return Guid.Empty;
            }

            try
            {
                return new Guid(WebEncoders.Base64UrlDecode(uId));
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}
