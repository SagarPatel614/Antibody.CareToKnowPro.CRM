using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Antibody.CareToKnowPro.CRM.Extensions
{
    public static class HttpResponseExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();

        static HttpResponseExtensions()
        {
            SerializerSettings.SetGlobalOptions();
        }

        //public static async Task WriteJsonAsync(this HttpResponse response, object obj, string contentType = null)
        //{
        //    response.ContentType = contentType ?? "application/json";
        //    await response.WriteAsync(JsonConvert.SerializeObject(obj, SerializerSettings));
        //}
    }
}
