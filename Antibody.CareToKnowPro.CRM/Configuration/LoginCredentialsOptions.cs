using FluentValidation;

namespace Antibody.CareToKnowPro.CRM.Configuration
{
    public class LoginCredentialsOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCredentialsOptionsValidator : AbstractValidator<LoginCredentialsOptions>
    {
        public LoginCredentialsOptionsValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
