using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Security
{
    public interface ILoginService
    {
        Task<bool> IsValidCredentials(string userName, string password);
    }
}
