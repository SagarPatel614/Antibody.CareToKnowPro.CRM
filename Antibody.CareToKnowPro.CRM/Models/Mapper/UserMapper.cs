using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Models.Mapper
{
    public static class UserMapper
    {
        public static DTO.User Map(this User obj)
        {
            return new DTO.User
            {
                UserId = obj.UserId,
                Email = obj.Email,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                GraduationYear = obj.GraduationYear,
                PasswordHash = obj.PasswordHash,
                Encrypted = obj.Encrypted,
                UserType = obj.UserType,
                ProvinceId = obj.ProvinceId,
                PreferredLanguage = obj.PreferredLanguage,
                Verified = obj.Verified,
                UserGuid = obj.UserGuid,
                Registered = obj.Registered,
                Province = obj.Province?.Map(),
                EventEntity = obj.EventEntity?.Select(a=>a?.Map()).ToList(),
                UserSpecialty = obj.UserSpecialty?.Select(b=>b?.Map()).ToList(),
                UserUnsubscribe = obj.UserUnsubscribe?.Select(c=>c?.Map()).ToList(),
                EmailStatus = obj.EmailStatus,
                Status = obj.Status,
                Notes = obj.Notes,
                CreatedBy = obj.CreatedBy,
                DateCreated = obj.DateCreated,
                DateModified = obj.DateModified,
                ModifiedBy = obj.ModifiedBy,
                Street1 = obj.Street1,
                City = obj.City,
                Postal = obj.Postal,
                Country = obj.Country,
                PhoneNumber = obj.PhoneNumber,
                SecondaryEmails = obj.SecondaryEmails,
                Fax = obj.Fax,
                AdditionalInfo = obj.AdditionalInfo
            };
        }
	}
}