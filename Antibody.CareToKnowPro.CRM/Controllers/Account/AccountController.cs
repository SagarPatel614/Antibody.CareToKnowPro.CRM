using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.IService;
using Antibody.CareToKnowPro.CRM.Models;
using Antibody.CareToKnowPro.CRM.Models.Mapper;
using Antibody.CareToKnowPro.CRM.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Antibody.CareToKnowPro.CRM.Controllers.Account
{
    [Route("api/account")]
    [ApiController]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [Produces("application/json")]
    public class AccountController : BaseController
    {
        //todo use the manager to perform the login instead of doing it in controller

        private readonly DbAntibodyCareToKnowProContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        
        public AccountController(DbAntibodyCareToKnowProContext dbContext, IEncryptionService encryptionService) : base(dbContext)
        {
            _dbContext = dbContext;
            this._encryptionService = encryptionService;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(Login.CommandResponse), 200)]
        public async Task<IActionResult> LoginAsync(Login.Command command)
        {
            try
            {
                // log the user out if they were already logged in
                await HttpContext.SignOutAsync();

                var response = new Login.CommandResponse();

                if (string.IsNullOrWhiteSpace(command.UserName))
                    throw new ArgumentException($@"{nameof(command.UserName)} cannot be null or empty string");
                if (string.IsNullOrWhiteSpace(command.Password))
                    throw new ArgumentException($@"{nameof(command.Password)} cannot be null or empty string");
                
                var user = _dbContext.LoginProfile.FirstOrDefault(a => a.UserName == command.UserName);

                if (user == null)
                {
                    throw new ArgumentException(@"The username and password were not recognised");
                }

                var isValid = await Task.FromResult(command.Password.Equals(this._encryptionService.DecryptPassword(user.PasswordHash), StringComparison.OrdinalIgnoreCase));

                if (isValid)
                {
                    response.IsAuthenticated = true;
                    response.User = user.Map();
                    string guid = Guid.NewGuid().ToString();

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, guid),
                        new Claim(ClaimTypes.Name, response.User.LoginProfileId.ToString())
                        //    new Claim(ClaimTypes.Role, "Administrator"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, AuthenticationSchemes.DefaultAuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(AuthenticationSchemes.DefaultAuthenticationScheme, claimsPrincipal, new AuthenticationProperties() { IsPersistent = false });

                }
                else
                {
                    //ModelState.AddModelError("", "Invalid credentials");
                    //return _apiOptions.InvalidModelStateResponseFactory(this.ControllerContext);
                    return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, "The username and password were not recognised"));
                }

                return Ok(response);
            }
            catch (ArgumentException e)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Create(StatusCodes.Status400BadRequest, e.Message));
            }
        }
    }
}