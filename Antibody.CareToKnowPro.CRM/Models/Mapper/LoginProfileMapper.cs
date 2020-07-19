using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class LoginProfileMapper
    {
        public static DTO.LoginProfile Map(this LoginProfile obj)
        {
            return new DTO.LoginProfile
            {
                LoginProfileId = obj.LoginProfileId,
                Email = obj.Email,
                UserName = obj.UserName,
                PasswordHash = obj.PasswordHash,
                CompanyName = obj.CompanyName,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Street1 = obj.Street1,
                City = obj.City,
                ProvCode = obj.ProvCode,
                Postal = obj.Postal,
                Country = obj.Country,
                PhoneNumber = obj.PhoneNumber,
                Notes = obj.Notes,
                Status = obj.Status
            };
        }
    }
}
