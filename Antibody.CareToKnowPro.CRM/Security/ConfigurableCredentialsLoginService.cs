using System;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.Configuration;
using Microsoft.Extensions.Options;

namespace Antibody.CareToKnowPro.CRM.Security
{
    public class ConfigurableCredentialsLoginService : ILoginService
    {
        private readonly LoginCredentialsOptions _options;

        public ConfigurableCredentialsLoginService(IOptions<LoginCredentialsOptions> options)
        {
            _options = options.Value;
        }

        public async Task<bool> IsValidCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException($"{nameof(userName)} cannot be null or empty string", nameof(userName));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"{nameof(password)} cannot be null or empty string", nameof(password));

            return await Task.FromResult(userName.Equals(_options.Username, StringComparison.OrdinalIgnoreCase) &&
                                         password.Equals(_options.Password));
        }
    }
}
