using System;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Antibody.CareToKnowPro.CRM.Serialization
{
    /// <summary>
    /// An implementation of the CamelCasePropertyNamesContractResolver which will serialize dictionaries
    /// such that, if the key represents a nested object (i.e. "Survey.Sections[2].Questions[4]") then each
    /// each segment is camel-case (i.e. "Survey.Sections[2].Questions[4]" becomes "survey.sections[2].questions[4]").
    /// Mainly this is done for the benefit of the ModelStateDictionary errors found in the ValidationProblemDetails object.
    /// See /Config/ApiConfig.cs.
    /// </summary>
    public class CamelCaseIncludingNestedDictionaryKeysContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);
            contract.DictionaryKeyResolver = (key) =>
            {
                // split the key by "." and camelcase each segment individually
                string[] segments = key.Split(new char[] { '.' }).Select(x => ResolvePropertyName(x)).ToArray();
                return string.Join(".", segments);
            };
            return contract;
        }
    }
}
