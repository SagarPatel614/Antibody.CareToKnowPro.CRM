using Antibody.CareToKnowPro.CRM.Serialization;
using Newtonsoft.Json;

namespace Antibody.CareToKnowPro.CRM.Extensions
{
    public static class JsonSerializationSettingsExtensions
    {
        public static void SetGlobalOptions(this JsonSerializerSettings settings)
        {
            settings.ContractResolver = new CamelCaseIncludingNestedDictionaryKeysContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
