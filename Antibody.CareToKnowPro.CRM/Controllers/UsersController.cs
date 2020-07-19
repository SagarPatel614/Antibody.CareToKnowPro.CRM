using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.Helpers;
using Antibody.CareToKnowPro.CRM.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Antibody.CareToKnowPro.CRM.Models;
using Antibody.CareToKnowPro.CRM.Models.Mapper;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CustomerIOSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using EventEntityProperty = Antibody.CareToKnowPro.CRM.Models.EventEntityProperty;
using Specialty = Antibody.CareToKnowPro.CRM.Models.Specialty;
using User = Antibody.CareToKnowPro.CRM.Models.User;
using UserSpecialty = Antibody.CareToKnowPro.CRM.Models.UserSpecialty;

namespace Antibody.CareToKnowPro.CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController, IDisposable
    {
        private readonly DbAntibodyCareToKnowProContext _context;
        private readonly EventContext _eventContext;
        private CustomerIo _customerIo;
        private readonly IConfiguration _configuration;
        private ICustomerIoService _customerIoService;

        public UsersController(DbAntibodyCareToKnowProContext context, IConfiguration configuration, ICustomerIoService customerIoService) : base(context)
        {
            _context = context;
            _configuration = configuration;
            _eventContext = new EventContext(context);
            _customerIoService = customerIoService;
        }


        [HttpPost("AddUserToCustomerIO")]
        public async Task<IActionResult> AddUserToCustomerIo()
        {
            var users = await _context.User.Where(a => a.Imported.HasValue && a.Imported.Value && (a.Synced == null || !a.Synced.Value)).ToListAsync();

            foreach (var user in users)
            {
                try
                {
                    await _context.SaveChangesAsync();

                    //modified in Customer.io
                    _customerIo = new CustomerIo(_configuration["CustomerIO:SITE_ID"], _configuration["CustomerIO:API_SECRET"], new CustomerFactory(user));
                    var customerIoUser = user.AsNewCustomer();
                    await _customerIo.IdentifyAsync(customerIoUser);

                    _context.Entry(user).State = EntityState.Modified;
                    user.Synced = true;

                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    
                }
            }

            return Ok();
        }

        [HttpPost("Paged")]
        public async Task<PagedResponse<DTO.User>> GetPagedUser(PagedRequest request)
        {
            if (request.CurrentPageIndex <= 0)
            {
                request.CurrentPageIndex = 1;
            }

            if (request.PageSize > 100)
            {
                request.PageSize = 100;
            }

            if (request.PageSize <= 25)
            {
                request.PageSize = 25;
            }

            if (request.Sort == null)
            {
                request.Sort = "";
            }

            if (request.SortDirection == null)
            {
                request.SortDirection = "";
            }

            IQueryable<User> qry = _context.User
                .Include(a => a.Province)
                .Include(b => b.UserUnsubscribe)
                .ThenInclude(b => b.Reason)
                .Include(c => c.UserSpecialty)
                .ThenInclude(c => c.Speciality);

            if (request.Filter != null)
            {
                if (!string.IsNullOrEmpty(request.Filter.FirstName))
                {
                    qry = qry.Where(a => a.FirstName.Contains(request.Filter.FirstName));
                }

                if (!string.IsNullOrEmpty(request.Filter.LastName))
                {
                    qry = qry.Where(a => a.LastName.Contains(request.Filter.LastName));
                }

                if (!string.IsNullOrEmpty(request.Filter.Email))
                {
                    qry = qry.Where(a => a.Email.Contains(request.Filter.Email) || a.SecondaryEmails.Contains(request.Filter.Email));
                }

                if (request.Filter.GraduationYear != null)
                {
                    qry = qry.Where(a => a.GraduationYear == request.Filter.GraduationYear);
                }

                if (request.Filter.ProvinceIds != null && request.Filter.ProvinceIds.Any())
                {
                    qry = qry.Where(a => request.Filter.ProvinceIds.Any(b=> b == a.ProvinceId));
                }

                if (!string.IsNullOrEmpty(request.Filter.PreferredLanguage))
                {
                    qry = qry.Where(a => a.PreferredLanguage == request.Filter.PreferredLanguage);
                }

                if (!string.IsNullOrEmpty(request.Filter.EmailStatus))
                {
                    qry = qry.Where(a => a.EmailStatus == request.Filter.EmailStatus);
                }

                if (!string.IsNullOrEmpty(request.Filter.Status))
                {
                    qry = qry.Where(a => a.Status == request.Filter.Status);
                }

                if (!string.IsNullOrEmpty(request.Filter.OtherSpecialties))
                {
                    foreach (var otherSpecialty in request.Filter.OtherSpecialtiesArray)
                    {
                        qry = qry.Where(a =>
                            a.UserSpecialty.Any(b => b.SpecialtyOther.Contains(otherSpecialty)));
                    }
                    //qry = qry.Where(a =>
                    //    a.UserSpecialty.Any(b => request.Filter.OtherSpecialtiesArray.Contains(b.SpecialtyOther)));
                }

                if (request.Filter.SpecialtyIds.Count > 0)
                {
                    qry = qry.Where(a =>
                        a.UserSpecialty.Any(b => request.Filter.SpecialtyIds.Contains(b.Speciality.SpecialtyId)));
                }
            }

            switch (request.Sort.ToLower())
            {
                case "firstname":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.FirstName)
                        : qry.OrderByDescending(a => a.FirstName);
                    break;
                case "lastname":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.LastName)
                        : qry.OrderByDescending(a => a.LastName);
                    break;
                case "email":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.Email)
                        : qry.OrderByDescending(a => a.Email);
                    break;
                case "address":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a =>
                            string.IsNullOrEmpty(a.Street1)
                                ? a.Street1
                                : (string.IsNullOrEmpty(a.City) ? a.City : a.Province.EnglishName))
                        : qry.OrderByDescending(a =>
                            string.IsNullOrEmpty(a.Street1)
                                ? a.Street1
                                : (string.IsNullOrEmpty(a.City) ? a.City : a.Province.EnglishName));
                    break;
                //case "userspecialty":
                //    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                //          string.IsNullOrEmpty(request.SortDirection)
                //        ? qry.OrderBy(a =>
                //            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                //                .OrderBy(n => n).FirstOrDefault())
                //        : qry.OrderByDescending(a =>
                //            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                //                .OrderByDescending(n => n).FirstOrDefault());
                //    break;
                case "userspecialty":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a =>
                            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                                .OrderBy(n => n).FirstOrDefault())
                        : qry.OrderByDescending(a =>
                            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                                .OrderByDescending(n => n).FirstOrDefault());
                    break;
                case "graduationyear":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.GraduationYear)
                        : qry.OrderByDescending(a => a.GraduationYear);
                    break;
                case "emailstatus":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.EmailStatus)
                        : qry.OrderByDescending(a => a.EmailStatus);
                    break;
                case "status":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.Status)
                        : qry.OrderByDescending(a => a.Status);
                    break;
                default:
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.FirstName)
                        : qry.OrderByDescending(a => a.FirstName);
                    break;
            }

            qry = qry.Skip((request.CurrentPageIndex - 1) * request.PageSize).Take(request.PageSize);

            var users = await qry.ToListAsync();

            var usersDto = users.Select(a => a.Map()); 
            int totalRecords = await GetUserTotalCountAsync(request);

            PagedResponse<DTO.User> response = new PagedResponse<DTO.User>(usersDto, totalRecords, request.PageSize, request.CurrentPageIndex);

            return response;
        }

        [HttpPost("Export/{currentPageOnly}")]
        public async Task<IEnumerable<ExportUserTocsv>> GetUserForExport(PagedRequest request, bool currentPageOnly = false)
        {
           
            if (request.CurrentPageIndex <= 0)
            {
                request.CurrentPageIndex = 1;
            }

            if (request.PageSize > 100)
            {
                request.PageSize = 100;
            }

            if (request.PageSize <= 25)
            {
                request.PageSize = 25;
            }

            if (request.Sort == null)
            {
                request.Sort = "";
            }

            if (request.SortDirection == null)
            {
                request.SortDirection = "";
            }

            IQueryable<User> qry = _context.User
                .Include(a => a.Province)
                .Include(b => b.UserUnsubscribe)
                .ThenInclude(b => b.Reason)
                .Include(c => c.UserSpecialty)
                .ThenInclude(c => c.Speciality);

            if (request.Filter != null)
            {
                if (!string.IsNullOrEmpty(request.Filter.FirstName))
                {
                    qry = qry.Where(a => a.FirstName.Contains(request.Filter.FirstName));
                }

                if (!string.IsNullOrEmpty(request.Filter.LastName))
                {
                    qry = qry.Where(a => a.LastName.Contains(request.Filter.LastName));
                }

                if (!string.IsNullOrEmpty(request.Filter.Email))
                {
                    qry = qry.Where(a => a.Email.Contains(request.Filter.Email));
                }

                if (request.Filter.GraduationYear != null)
                {
                    qry = qry.Where(a => a.GraduationYear == request.Filter.GraduationYear);
                }

                if (request.Filter.ProvinceIds != null && request.Filter.ProvinceIds.Any())
                {
                    qry = qry.Where(a => request.Filter.ProvinceIds.Any(b => b == a.ProvinceId));
                }

                if (!string.IsNullOrEmpty(request.Filter.PreferredLanguage))
                {
                    qry = qry.Where(a => a.PreferredLanguage == request.Filter.PreferredLanguage);
                }

                if (!string.IsNullOrEmpty(request.Filter.EmailStatus))
                {
                    qry = qry.Where(a => a.EmailStatus == request.Filter.EmailStatus);
                }

                if (!string.IsNullOrEmpty(request.Filter.Status))
                {
                    qry = qry.Where(a => a.Status == request.Filter.Status);
                }

                if (!string.IsNullOrEmpty(request.Filter.OtherSpecialties))
                {
                    foreach (var otherSpecialty in request.Filter.OtherSpecialtiesArray)
                    {
                        qry = qry.Where(a =>
                            a.UserSpecialty.Any(b => b.SpecialtyOther.Contains(otherSpecialty)));
                    }
                    //qry = qry.Where(a =>
                    //    a.UserSpecialty.Any(b => request.Filter.OtherSpecialtiesArray.Contains(b.SpecialtyOther)));
                }

                if (request.Filter.SpecialtyIds.Count > 0)
                {
                    qry = qry.Where(a =>
                        a.UserSpecialty.Any(b => request.Filter.SpecialtyIds.Contains(b.Speciality.SpecialtyId)));
                }
            }

            switch (request.Sort.ToLower())
            {
                case "firstname":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.FirstName)
                        : qry.OrderByDescending(a => a.FirstName);
                    break;
                case "lastname":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.LastName)
                        : qry.OrderByDescending(a => a.LastName);
                    break;
                case "email":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.Email)
                        : qry.OrderByDescending(a => a.Email);
                    break;
                case "address":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a =>
                            string.IsNullOrEmpty(a.Street1)
                                ? a.Street1
                                : (string.IsNullOrEmpty(a.City) ? a.City : a.Province.EnglishName))
                        : qry.OrderByDescending(a =>
                            string.IsNullOrEmpty(a.Street1)
                                ? a.Street1
                                : (string.IsNullOrEmpty(a.City) ? a.City : a.Province.EnglishName));
                    break;
                case "userspecialty":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a =>
                            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                                .OrderBy(n => n).FirstOrDefault())
                        : qry.OrderByDescending(a =>
                            a.UserSpecialty.Select(b => b.Speciality == null ? b.Speciality.SpecialtyNameEn : "")
                                .OrderByDescending(n => n).FirstOrDefault());
                    break;
                case "graduationyear":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.GraduationYear)
                        : qry.OrderByDescending(a => a.GraduationYear);
                    break;
                case "emailstatus":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.EmailStatus)
                        : qry.OrderByDescending(a => a.EmailStatus);
                    break;
                case "status":
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.Status)
                        : qry.OrderByDescending(a => a.Status);
                    break;
                default:
                    qry = string.Equals(request.SortDirection, "ASC", StringComparison.CurrentCultureIgnoreCase) ||
                          string.IsNullOrEmpty(request.SortDirection)
                        ? qry.OrderBy(a => a.FirstName)
                        : qry.OrderByDescending(a => a.FirstName);
                    break;
            }
            if (currentPageOnly)
            {
                qry = qry.Skip((request.CurrentPageIndex - 1) * request.PageSize).Take(request.PageSize);
            }

            var users = await qry.ToListAsync();

            var data = users.Select(a => new ExportUserTocsv
            {
                FirstName = a.FirstName ?? "",
                LastName = a.LastName ?? "",
                Email = a.Email ?? "",
                SecondaryEmails = a.SecondaryEmails ?? "",
                Address = a.GetAddress(),
                Phone = a.PhoneNumber ?? "",
                Fax = a.Fax ?? "",
                AdditionalInfo = a.AdditionalInfo ?? "",
                Specialty = string.Join(", ", a.UserSpecialty?.Select(b => b.Speciality?.SpecialtyNameEn) ?? new List<string>()),
                OtherSpecialty = string.Join(", ", a.UserSpecialty?.Where(b=>!string.IsNullOrEmpty(b.SpecialtyOther)).Select(b => b.SpecialtyOther) ?? new List<string>()),
                GraduationYear = a.GraduationYear?.ToString() ?? "",
                EmailStatus = a.EmailStatus ?? "",
                Status = a.Status ?? "",
                Language = a.PreferredLanguage == "EN" ? "English" : "French",
                CreatedBy = a.CreatedBy ?? "",
                DateCreated = a.DateCreated != null ? a.DateCreated.Value.ToString("yyyy MMMM dd") : "",
                ModifiedBy = a.ModifiedBy ?? "",
                DateModified = a.DateModified != null ? a.DateModified.Value.ToString("yyyy MMMM dd") : "",
            });


            return data;
        }
        public async Task<int> GetUserTotalCountAsync(PagedRequest request)
        {
            IQueryable<User> qry = _context.User
                .Include(a => a.Province)
               // .Include(b => b.UserUnsubscribe)
                //.ThenInclude(b => b.Reason)
                .Include(c => c.UserSpecialty)
                .ThenInclude(c => c.Speciality);

            if (request.Filter != null)
            {
                if (!string.IsNullOrEmpty(request.Filter.FirstName))
                {
                    qry = qry.Where(a => a.FirstName.Contains(request.Filter.FirstName));
                }
                if (!string.IsNullOrEmpty(request.Filter.LastName))
                {
                    qry = qry.Where(a => a.LastName.Contains(request.Filter.LastName));
                }
                if (!string.IsNullOrEmpty(request.Filter.Email))
                {
                    qry = qry.Where(a => a.Email.Contains(request.Filter.Email) || a.SecondaryEmails.Contains(request.Filter.Email));
                }
                if (request.Filter.GraduationYear != null)
                {
                    qry = qry.Where(a => a.GraduationYear == request.Filter.GraduationYear);
                }
                if (request.Filter.ProvinceIds != null && request.Filter.ProvinceIds.Any())
                {
                    qry = qry.Where(a => request.Filter.ProvinceIds.Any(b => b == a.ProvinceId));
                }
                if (!string.IsNullOrEmpty(request.Filter.PreferredLanguage))
                {
                    qry = qry.Where(a => a.PreferredLanguage == request.Filter.PreferredLanguage);
                }
                if (!string.IsNullOrEmpty(request.Filter.EmailStatus))
                {
                    qry = qry.Where(a => a.EmailStatus == request.Filter.EmailStatus);
                }
                if (!string.IsNullOrEmpty(request.Filter.Status))
                {
                    qry = qry.Where(a => a.Status == request.Filter.Status);
                }
                if (!string.IsNullOrEmpty(request.Filter.OtherSpecialties))
                {
                    foreach (var otherSpecialty in request.Filter.OtherSpecialtiesArray)
                    {
                        qry = qry.Where(a =>
                            a.UserSpecialty.Any(b => b.SpecialtyOther.Contains(otherSpecialty)));
                    }
                    //qry = qry.Where(a =>
                    //    a.UserSpecialty.Any(b => request.Filter.OtherSpecialtiesArray.Contains(b.SpecialtyOther)));
                }
                if (request.Filter.SpecialtyIds.Count > 0)
                {
                    qry = qry.Where(a => a.UserSpecialty.Any(b => request.Filter.SpecialtyIds.Contains(b.Speciality.SpecialtyId)));
                }
            }
            return await qry.CountAsync();
        }

        // GET: api/Users/filter
        [HttpGet("filter")]
        public ActionResult<DTO.Filter> GerFilter()
        {
            var model = new Filter
            {
                FirstName = "",
                LastName = "",
                Email = "",
                EmailStatus = "",
                GraduationYear = null,
                PreferredLanguage = "",
                OtherSpecialties = ""
            };

            model.GraduationList.Add(new SelectListItem
            {
                Text = "",
                Value = ""
            });
            //model.ProvinceList.Add(new SelectListItem
            //{
            //    Text = "",
            //    Value = ""
            //});

            for (int i = DateTime.Now.Year; i >= 1940; i--)
            {
                model.GraduationList.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            };

            model.ProvinceList.AddRange(_context.Province.Select(p =>
                new SelectListItem
                {
                    Text = p.EnglishName,
                    Value = p.ProvinceId.ToString()
                }
            ).ToList());

           

            var specialties = _context.Specialty.Include(x => x.SpecialtyGroup)
                .OrderBy(s => s.SpecialtyGroup.Position).ThenBy(s => s.SpecialtyNameEn).GroupBy(x => x.SpecialtyGroup.GroupNameEn);

            var specialtiesSelectList = new List<SelectListItem>();
            foreach (var group in specialties)
            {
                // Create a SelectListGroup
                var optionGroup = new SelectListGroup() { Name = @group.Key != null ? @group.Key.ToString() : "General" };
                // Add SelectListItem's
                foreach (var item in group)
                {
                    specialtiesSelectList.Add(new SelectListItem()
                    {
                        Value = item.SpecialtyId.ToString(),
                        Text = item.SpecialtyNameEn,
                        Group = optionGroup
                    });
                }
            }
            model.Specialties = specialtiesSelectList.GroupBy(a => a.Group);

            return model;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.User>> GetUser(int id)
        {
            var user = _context.User.Include(a => a.Province)
                .Include(b => b.UserUnsubscribe)
                    .ThenInclude(b => b.Reason)
                .Include(c => c.UserSpecialty)
                    .ThenInclude(c => c.Speciality)
                .Include(d => d.EventEntity)
                    .ThenInclude(d => d.Event)
                        .ThenInclude(e=>e.LoginProfile)
                .Include(d => d.EventEntity)
                .Include(d=>d.EventEntity)
                    .ThenInclude(d=>d.EventEntityProperty).FirstOrDefault(a => a.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = user.Map();

            for (int i = DateTime.Now.Year; i >= 1940; i--)
            {
                model.GraduationList.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            };
            model.ProvinceList.AddRange(_context.Province.Select(p =>
                new SelectListItem
                {
                    Text = p.EnglishName,
                    Value = p.ProvinceId.ToString(),
                    Selected = p.ProvinceId == user.ProvinceId
                }
            ).ToList());

            var specialities = _context.Specialty.Include(x => x.SpecialtyGroup)
                .OrderBy(s => s.SpecialtyGroup.Position).ThenBy(s => s.SpecialtyNameEn).GroupBy(x => x.SpecialtyGroup.GroupNameEn);

            var specialitiesSelectList = new List<SelectListItem>();
            foreach (var group in specialities)
            {
                // Create a SelectListGroup
                var optionGroup = new SelectListGroup() { Name = @group.Key != null ? @group.Key.ToString() : "General" };
                // Add SelectListItem's
                foreach (var item in group)
                {
                    specialitiesSelectList.Add(new SelectListItem()
                    {
                        Value = item.SpecialtyId.ToString(),
                        Text = item.SpecialtyNameEn,
                        Group = optionGroup
                    });
                }
            }
            model.Specialities = specialitiesSelectList.GroupBy(a => a.Group);
            model.Other = string.Join(",", user.UserSpecialty.Where(a => a.SpecialityId == 31).Select(a => a.SpecialtyOther));

            model.Messages = await _customerIoService.GetMessages(model.UserId);

            return model;
        }

        // GET: api/Users/register
        [HttpGet("register")]
        public ActionResult<RegistrationModel> Register()
        {
            RegistrationModel model = new RegistrationModel { IsError = false };
            for (int i = DateTime.Now.Year; i >= 1940; i--)
            {
                model.GraduationList.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            };
            model.ProvinceList.AddRange(_context.Province.Select(p =>
                new SelectListItem
                {
                    Text = p.EnglishName,
                    Value = p.ProvinceId.ToString()
                }
            ).ToList());

            var specialties = _context.Specialty.Include(x => x.SpecialtyGroup)
                .OrderBy(s => s.SpecialtyGroup.Position).ThenBy(s => s.SpecialtyNameEn).GroupBy(x => x.SpecialtyGroup.GroupNameEn);

            var specialtiesSelectList = new List<SelectListItem>();
            foreach (var group in specialties)
            {
                // Create a SelectListGroup
                var optionGroup = new SelectListGroup() { Name = @group.Key != null ? @group.Key.ToString() : "General" };
                // Add SelectListItem's
                foreach (var item in group)
                {
                    specialtiesSelectList.Add(new SelectListItem()
                    {
                        Value = item.SpecialtyId.ToString(),
                        Text = item.SpecialtyNameEn,
                        Group = optionGroup
                    });
                }
            }
            model.Specialities = specialtiesSelectList.GroupBy(a => a.Group);

            var list = _context.Specialty.Include(x => x.SpecialtyGroup)
                .OrderBy(s => s.SpecialtyGroup.Position).ThenBy(s => s.SpecialtyNameEn)
                .Select(s =>
                    new SpecialtyItems
                    {
                        SpecialtyId = s.SpecialtyId,
                        SpecialtyGroupName = s.SpecialtyGroup.GroupNameEn,
                        SpecialtyName = s.SpecialtyNameEn
                    }).ToList();


            model.SpecialtyList = new MultiSelectList(list, "SpecialtyId", "SpecialtyName", model.SpecialtyIds, "SpecialtyGroupName");

            return model;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Invalid request."));
            }

            if (user.SpecialtyIds.IndexOf(Convert.ToInt32(_configuration["OtherSpecialtyId"])) >= 0)
            {
                if (string.IsNullOrEmpty(user.Other))
                {
                    return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Please indicate other specialty."));
                }
            }

            if (!string.IsNullOrEmpty(user.Other) && user.SpecialtyIds.IndexOf(Convert.ToInt32(_configuration["OtherSpecialtyId"])) <= 0)
            {
                user.SpecialtyIds.Add(Convert.ToInt32(_configuration["OtherSpecialtyId"]));
            }


            foreach (var specialty in user.SpecialtyIds)
            {
                if (specialty != 31)
                {
                    user.UserSpecialty.Add(new UserSpecialty { SpecialityId = specialty });
                }
                else
                {
                    user.UserSpecialty.Add(new UserSpecialty
                    { SpecialityId = specialty, SpecialtyOther = user.Other });
                }
            }

            user.ModifiedBy = _CurrentUser.UserName;
            user.DateModified = DateTime.Now;

            var currentUser = _context.User.Where(a => a.UserId == id).Include(a => a.UserSpecialty).ThenInclude(a => a.Speciality).First();

            currentUser.EmailStatus = user.EmailStatus;
            currentUser.Verified = string.Equals(currentUser.EmailStatus, "Subscribed", StringComparison.CurrentCultureIgnoreCase);
            currentUser.PreferredLanguage = user.PreferredLanguage;
            currentUser.ProvinceId = user.ProvinceId;
            currentUser.City = user.City;
            currentUser.Country = user.Country;
            currentUser.Email = user.Email;
            currentUser.SecondaryEmails = user.SecondaryEmails;
            currentUser.Street1 = user.Street1;
            currentUser.Status = user.Status;
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.GraduationYear = user.GraduationYear;
            currentUser.Notes = user.Notes;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Fax = user.Fax;
            currentUser.AdditionalInfo = user.AdditionalInfo;
            currentUser.Postal = user.Postal;

            _context.Entry(currentUser).State = EntityState.Modified;

            _eventContext.Initialize(EventType.UserUpdated, _CurrentUser, user.Notes);
            _eventContext.AddEntity(currentUser);

            currentUser.ModifiedBy = user.ModifiedBy;
            currentUser.DateModified = user.DateModified;

            var needToRemove = new List<UserSpecialty>();
            
            //remove all the specialty that are not present in dto
            foreach (var userSpecialty in currentUser.UserSpecialty)
            {
                if (user.UserSpecialty.All(a => a.SpecialityId != userSpecialty.SpecialityId))
                {
                    needToRemove.Add(userSpecialty);
                }
            }

            //todo this can be improve through reflection and combine with event context
            string removedSpecialties = $"{string.Join(", ", needToRemove.Select(a => a.Speciality.SpecialtyNameEn))}";
            
            foreach (var userSpecialty in needToRemove)
            {
                currentUser.UserSpecialty.Remove(userSpecialty);
            }

            string newSpecialties = "";

            //add newly added specialty
            foreach (var userSpecialty in user.UserSpecialty)
            {
                if (currentUser.UserSpecialty.All(a => a.SpecialityId != userSpecialty.SpecialityId))
                {
                    //todo improve it through eager loading
                    userSpecialty.Speciality = _context.Specialty.FirstOrDefault(a => a.SpecialtyId == userSpecialty.SpecialityId);

                    if (userSpecialty.Speciality != null)
                    {
                        newSpecialties += $"{userSpecialty.Speciality.SpecialtyNameEn}{(!string.IsNullOrEmpty(userSpecialty.SpecialtyOther) ? ": " + userSpecialty.SpecialtyOther : "")}, ";
                    }
                    currentUser.UserSpecialty.Add(userSpecialty);
                }
            }


            if (!string.IsNullOrEmpty(removedSpecialties) || !string.IsNullOrEmpty(newSpecialties))
            {
                string newSpecialty = string.IsNullOrEmpty(newSpecialties) ? "" : newSpecialties.Substring(0, newSpecialties.Length - 2);

                //todo this can be improve through reflection and combine with event context
                var property = new EventEntityProperty
                {
                    PropertyName = "Specialty",
                    OriginalValue = removedSpecialties,
                    NewValue = newSpecialty
                };

                var currentEvent = _eventContext.GetEvent(true);
                var entity = currentEvent.EventEntity.FirstOrDefault();

                if (entity == null)
                {
                    entity = new Models.EventEntity();
                    entity.GetType().GetProperty("UserId")?.SetValue(entity, user.UserId);
                    entity.ActionType = (int)ActionType.Modified;
                    entity.Position = currentEvent.EventEntity.Count + 1;

                    currentEvent.EventEntity.Add(entity);
                }
                var entityProperties = entity.EventEntityProperty;

                entityProperties.Add(property);
            }

            try
            {
                await _context.SaveChangesAsync();

                //modified in Customer.io
                _customerIo = new CustomerIo(_configuration["CustomerIO:SITE_ID"], _configuration["CustomerIO:API_SECRET"], new CustomerFactory(currentUser));
                var customerIoUser = currentUser.AsNewCustomer();
                await _customerIo.IdentifyAsync(customerIoUser);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(id))
                {
                    return NotFound(ErrorResponse.Create(StatusCodes.Status404NotFound, "User not found"));
                }

                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, ex.Message));
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<DTO.User>> PostUser(RegistrationModel model)
        {
            if (_context.User.Any(x => model.Email.Equals(x.Email)))
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Email already in use."));
            }

            if (model.SpecialtyIds.IndexOf(Convert.ToInt32(_configuration["OtherSpecialtyId"])) >= 0)
            {
                if (String.IsNullOrEmpty(model.Other))
                {
                    return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Please indicate other specialty."));
                }
            }

            if (!string.IsNullOrEmpty(model.Other) && model.SpecialtyIds.IndexOf(Convert.ToInt32(_configuration["OtherSpecialtyId"])) <= 0)
            {
                model.SpecialtyIds.Add(Convert.ToInt32(_configuration["OtherSpecialtyId"]));
            }

            if (ModelState.IsValid)
            {
                //currently passwordHash is set to allow nulls in db. 
                try
                {
                    Guid userGuid = Guid.NewGuid();
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        SecondaryEmails = model.SecondaryEmails,
                        PreferredLanguage = model.PreferredLanguage,
                        GraduationYear = int.Parse(model.GraduationYear),
                        Verified = true,
                        ProvinceId = model.ProvinceId,
                        UserGuid = userGuid,
                        Registered =  true,
                        EmailStatus = model.EmailStatus,
                        Status = model.Status,
                        Notes = model.Notes,
                        Street1 = model.Street1,
                        City = model.City,
                        Postal = model.Postal,
                        Country = model.Country,
                        PhoneNumber = model.PhoneNumber,
                        Fax = model.Fax,
                        AdditionalInfo = model.AdditionalInfo,
                        CreatedBy = _CurrentUser.UserName,
                        ModifiedBy = _CurrentUser.UserName,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    _context.Entry(newUser).State = EntityState.Added;

                    _eventContext.Initialize(EventType.UserCreated, _CurrentUser, model.Notes);
                    _eventContext.AddEntity(newUser);

                    _context.User.Add(newUser);

                    List<EventEntityProperty> properties = new List<EventEntityProperty>();
                    var spe = _context.Specialty.Where(a => model.SpecialtyIds.Contains(a.SpecialtyId));
                    string specialties = $"{string.Join(", ", spe.Select(a => a.SpecialtyNameEn))}";
                    string other = spe.Any(a => a.SpecialtyId == 31) ? $"{model.Other}" : "";
                    string value = $"{specialties}{other}";
                    var property = new EventEntityProperty
                    {
                        PropertyName = "Specialty",
                        OriginalValue = "",
                        NewValue = value
                    };
                    properties.Add(property);

                    foreach (var specialty in model.SpecialtyIds)
                    {
                        newUser.UserSpecialty.Add(specialty != 31
                            ? new UserSpecialty {SpecialityId = specialty}
                            : new UserSpecialty {SpecialityId = specialty, SpecialtyOther = model.Other});
                    }

                    //Add Specialty to event context
                    //todo this can be improve through reflection and combine with event context
                    var currentEvent = _eventContext.GetEvent(true);
                    var entity = currentEvent.EventEntity.FirstOrDefault();

                    if (entity == null)
                    {
                        entity = new Models.EventEntity();
                        entity.GetType().GetProperty("UserId")?.SetValue(entity, newUser.UserId);
                        entity.ActionType = (int)ActionType.Added;
                        entity.Position = currentEvent.EventEntity.Count + 1;

                        currentEvent.EventEntity.Add(entity);
                    }
                    var entityProperties = entity.EventEntityProperty;

                    foreach (var eventEntityProperty in properties)
                    {
                        entityProperties.Add(eventEntityProperty);
                    }


                    Guid guid = Guid.NewGuid();
                    string token = System.Web.HttpUtility.UrlEncode(guid.ToString());

                    var userToken = new UserToken
                    {
                        Email = newUser.Email,
                        UserId = newUser.UserId,
                        Token = token,
                        CreatedOn = DateTime.Now,
                        Type = TokenType.NewAccount
                    };
                    newUser.UserToken.Add(userToken);

                    await _context.SaveChangesAsync();
                    // var loggedIn = LogInUser(newUser);

                    _customerIo = new CustomerIo(_configuration["CustomerIO:SITE_ID"], _configuration["CustomerIO:API_SECRET"], new CustomerFactory(newUser));

                    var user = newUser.AsNewCustomer();
                    // var result = Task.Run(async () => await _customerIo.IdentifyAsync(_user)).Status;

                    await _customerIo.IdentifyAsync(user);

                    //string lang = newUser.PreferredLanguage == "EN" ? "/en" : "/fr";
                    //string emailVerifyLink = $"{_configuration["CustomerIO:EmailBlastURL"]}{lang}/Verify?id={newUser.UserId}&email={newUser.Email}&token={userToken.Token}";

                    //var emailVerifyData = new
                    //{
                    //    email = newUser.Email,
                    //    language = newUser.PreferredLanguage == "FR" ? "french" : "english",
                    //    link = emailVerifyLink
                    //};
                    //try
                    //{
                    //    await _customerIo.TrackEventAsync(_configuration["CustomerIO:EventName"], emailVerifyData, DateTime.Now,
                    //        newUser.UserId.ToString());
                    //}
                    //catch (Exception ex)
                    //{
                    //    return BadRequest(ErrorResponse.Create(StatusCodes.Status500InternalServerError, ex.Message));
                    //}

                    return CreatedAtAction("GetUser", new { id = newUser.UserId }, newUser.Map());
                }
                catch (Exception ex)
                {
                    return BadRequest(ErrorResponse.Create(StatusCodes.Status500InternalServerError, ex.Message));

                }
            }

            var errors = ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .ToList();

            return BadRequest(ErrorResponse.Create(StatusCodes.Status500InternalServerError, string.Join(", ", errors)));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = _context.User.Where(a => a.UserId == id).Include(a=>a.EventEntity).ThenInclude(a=>a.EventEntityProperty).Include(a=>a.EventEntity).ThenInclude(a=>a.Event).Include(a=>a.UserSpecialty).Include(a=>a.UserToken).Include(a=>a.UserUnsubscribe).FirstOrDefault();
            
            //var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "User not found"));
                //NotFound();
            }

            //var events = _context.EventEntity.Where(a => a.UserId == id).Include(a=>a.EventEntityProperty);
            //var eventProperties = events.Select(a => a.EventEntityProperty);

            //var userSpecialty = _context.UserSpecialty.Where(a => a.UserId == id);

            //foreach (var eventProperty in eventProperties)
            //{
            //    _context.EventEntityProperty.RemoveRange(eventProperty);
            //}
            //_context.EventEntity.RemoveRange(events);

            //_context.UserSpecialty.RemoveRange(userSpecialty);

            _context.User.Remove(user);
            try
            {
                await _context.SaveChangesAsync();

                //set delete attributes in customer.io
                _customerIo = new CustomerIo(_configuration["CustomerIO:SITE_ID"], _configuration["CustomerIO:API_SECRET"], new CustomerFactory(user));
                var customerIoUser = user.AsDeleteCustomer();
                await _customerIo.IdentifyAsync(customerIoUser);
            }
            catch (Exception ex)
            {
                //todo rollback
            }

            return Ok();
        }

        // POST: api/DuplicateUserCheck
        [HttpPost("DuplicateUserCheck")]
        public ActionResult<DuplicateCheckResponse> DuplicateUserCheck(IFormFile file)
        {
            try
            {
                TextReader reader = new StreamReader(file.OpenReadStream());
                var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true });
                var records = csvReader.GetRecords<HcpDuplicateCheck>();
                var data = records.ToList();

                var existingRecords = data.Where(a => _context.User.Include(u => u.Province)
                                                                .Include(c => c.UserSpecialty)
                                                                    .ThenInclude(c => c.Speciality).Any(u =>
                                                                        (string.IsNullOrEmpty(a.FirstName) || string.Equals(u.FirstName, a.FirstName.Trim(), StringComparison.CurrentCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(a.LastName) || string.Equals(u.LastName, a.LastName.Trim(), StringComparison.CurrentCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(a.Email) || string.Equals(u.Email, a.Email.Trim(), StringComparison.CurrentCultureIgnoreCase)) &&
                                                                        (a.GraduationYear == null || Equals(u.GraduationYear, a.GraduationYear)) &&
                                                                        (string.IsNullOrEmpty(a.Language) || string.Equals(u.PreferredLanguage, a.Language.Trim(), StringComparison.CurrentCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(a.ProvinceCode) || string.Equals(u.Province.Abbreviation, a.ProvinceCode.Trim(), StringComparison.CurrentCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(a.Specialties) || u.UserSpecialty.Any(s => a.SpecialtiesArray.Contains(s.Speciality.SpecialtyNameEn.ToLower())))
                                                                ));

                var notExistingRecords = data.Where(a => existingRecords.All(e => e != a));

                return new DuplicateCheckResponse()
                {
                    ExistingRecords = existingRecords,
                    NotExistingRecords = notExistingRecords
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public class DuplicateCheckResponse
    {
        public IEnumerable<HcpDuplicateCheck> ExistingRecords { get; set; }
        public IEnumerable<HcpDuplicateCheck> NotExistingRecords { get; set; }
    }

    public class HcpDuplicateCheck
    {
        [Index(0)]
        public string FirstName { get; set; }
        [Index(1)]
        public string LastName { get; set; }
        [Index(2)]
        public string Email { get; set; }
        [Index(3)]
        public string ProvinceCode { get; set; }
        [Index(4)]
        public string Specialties { get; set; }
        [Index(5)]
        public int? GraduationYear { get; set; }
        [Index(6)]
        public string Language { get; set; }

        public string[] SpecialtiesArray => Specialties.Trim().ToLower().Split(",");
    }
    public class ExportUserTocsv
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SecondaryEmails { get; set; }
        public string Address { get; set; }
        public string Specialty { get; set; }
        public string GraduationYear { get; set; }
        public string EmailStatus { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }
        public string OtherSpecialty { get; set; }
        public string Phone { get; set; }
        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public string DateModified { get; set; }
        public string Fax { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
