using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.IService;
using Antibody.CareToKnowPro.CRM.Models;
using Antibody.CareToKnowPro.CRM.Models.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginProfile = Antibody.CareToKnowPro.CRM.DTO.LoginProfile;

namespace Antibody.CareToKnowPro.CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginProfilesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;
        private readonly IEncryptionService _encryptionService;

        public LoginProfilesController(DbAntibodyCareToKnowProContext context, IEncryptionService encryptionService, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
            this._encryptionService = encryptionService;
        }

        // GET: api/LoginProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginProfile>>> GetLoginProfile()
        {
            return await _context.LoginProfile.Select(a=>a.Map()).ToListAsync();
        }

        // GET: api/LoginProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginProfile>> GetLoginProfile(int id)
        {
            var loginProfile = await _context.LoginProfile.FindAsync(id);

            if (loginProfile == null)
            {
                return NotFound();
            }

            return loginProfile.Map();
        }

        // PUT: api/LoginProfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginProfile(int id, LoginProfile loginProfile)
        {
            if (id != loginProfile.LoginProfileId)
            {
                return BadRequest();
            }

            Models.LoginProfile profile = await _context.LoginProfile.FindAsync(id);

            profile.FirstName = loginProfile.FirstName;
            profile.LastName = loginProfile.LastName;
            profile.Email = loginProfile.Email;
            profile.UserName = loginProfile.UserName;
            profile.CompanyName = loginProfile.CompanyName;
            profile.PhoneNumber = loginProfile.PhoneNumber;
            profile.Street1 = loginProfile.Street1;
            profile.City = loginProfile.City;
            profile.ProvCode = loginProfile.ProvCode;
            profile.Country = loginProfile.Country;
            profile.Postal = loginProfile.Postal; 
            profile.Notes = loginProfile.Notes;

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LoginProfiles
        [HttpPost]
        public async Task<ActionResult<LoginProfile>> PostLoginProfile(Models.LoginProfile loginProfile)
        {
            loginProfile.PasswordHash = this._encryptionService.EncryptPassword(loginProfile.Password);
            _context.LoginProfile.Add(loginProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoginProfile", new { id = loginProfile.LoginProfileId }, loginProfile.Map());
        }

        // POST: api/LoginProfiles
        [HttpPut("{id}/ChangePassword")]
        public async Task<ActionResult<LoginProfile>> ChangePassword(int id, PasswordChangeModel passwordChangeModel)
        {
            if (id != passwordChangeModel.LoginProfileId)
            {
                return BadRequest();
            }

            var loginProfile = await _context.LoginProfile.FindAsync(id);

            if (loginProfile == null)
            {
                return NotFound();
            }

            if (passwordChangeModel.NewPassword != passwordChangeModel.ConfirmNewPassword)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Password doesn't match"));
            }

            if (_encryptionService.DecryptPassword(loginProfile.PasswordHash) != passwordChangeModel.OldPassword)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "Incorrect Password!"));
            }

            loginProfile.PasswordHash = _encryptionService.EncryptPassword(passwordChangeModel.NewPassword);
            _context.Entry(loginProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LoginProfileExists(id))
                {
                    return NotFound();
                }

                return BadRequest(ErrorResponse.Create(StatusCodes.Status500InternalServerError, ex.Message));
            }

            return CreatedAtAction("GetLoginProfile", new { id = passwordChangeModel.LoginProfileId }, loginProfile);
        }

        // DELETE: api/LoginProfiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoginProfile>> DeleteLoginProfile(int id)
        {
            var loginProfile = await _context.LoginProfile.FindAsync(id);
            if (loginProfile == null)
            {
                return NotFound();
            }

            _context.LoginProfile.Remove(loginProfile);
            await _context.SaveChangesAsync();

            return loginProfile.Map();
        }

        private bool LoginProfileExists(int id)
        {
            return _context.LoginProfile.Any(e => e.LoginProfileId == id);
        }
    }
}
