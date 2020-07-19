using System.Collections.Generic;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;

namespace Antibody.CareToKnowPro.CRM.IService
{
    public interface ICustomerIoService
    {
        Task<List<Message>> GetMessages(int id, string continuationToken = "", int limit = 5);

        Task<Campaign> GetCampaign(string id);
    }
}