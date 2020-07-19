using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.IService;
using Antibody.CareToKnowPro.CRM.Models;
using Antibody.CareToKnowPro.CRM.Models.Mapper;
using Antibody.CareToKnowPro.CRM.Security;
using FluentValidation;
using MediatR;
using LoginProfile = Antibody.CareToKnowPro.CRM.Models.LoginProfile;

namespace Antibody.CareToKnowPro.CRM.Controllers.Account
{
    public class Login
    {
        public class Command : IRequest<CommandResponse>
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class CommandResponse
        {
            public DTO.LoginProfile User { get; set; }
            public bool IsAuthenticated { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResponse>
        {
            private readonly ILoginService _loginService;
            private readonly DbAntibodyCareToKnowProContext _dbContext;
            private readonly IEncryptionService _encryptionService;

            public CommandHandler(ILoginService loginService, DbAntibodyCareToKnowProContext dbContext, IEncryptionService encryptionService)
            {
                _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
                _dbContext = dbContext;
                this._encryptionService = encryptionService;
            }

            public async Task<CommandResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                var response = new CommandResponse();

                if (string.IsNullOrWhiteSpace(command.UserName))
                    throw new ArgumentException($@"{nameof(command.UserName)} cannot be null or empty string");
                if (string.IsNullOrWhiteSpace(command.Password))
                    throw new ArgumentException($@"{nameof(command.Password)} cannot be null or empty string");


                var user = _dbContext.LoginProfile.FirstOrDefault(a => a.UserName == command.UserName);

                if (user == null)
                {
                    throw new ArgumentException(@"The username and password were not recognised");
                }

                var isValid = await Task.FromResult(command.Password.Equals( this._encryptionService.DecryptPassword(user.PasswordHash), StringComparison.OrdinalIgnoreCase));
                
                if (isValid)
                {
                    response.IsAuthenticated = true;
                    response.User = user.Map();
                }
                else
                {
                    response.IsAuthenticated = false;
                }

                return response;
            }
        }
    }
}
