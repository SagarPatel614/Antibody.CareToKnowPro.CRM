using Antibody.CareToKnowPro.CRM.Helpers;
using Antibody.CareToKnowPro.CRM.IService;
using Microsoft.Extensions.Configuration;

namespace Antibody.CareToKnowPro.CRM.Services
{
    public class EncryptionService : IEncryptionService
    {
        public IConfiguration Configuration { get; }
        
        private readonly string _cipher;

        public EncryptionService(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this._cipher = this.Configuration["Cipher"];
        }

        public string EncryptPassword(string clearPassword)
        {
            return Encryptor.Encrypt(clearPassword, _cipher);
        }

        public string DecryptPassword(string encryptedPassword)
        {
            return StringCipher.Decrypt(encryptedPassword, _cipher);
        }
    }
}