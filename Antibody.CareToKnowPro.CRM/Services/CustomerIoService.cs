using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.DTO;
using Antibody.CareToKnowPro.CRM.Helpers;
using Antibody.CareToKnowPro.CRM.IService;
using Antibody.CareToKnowPro.CRM.Models.Mapper;
using CustomerIOSharp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnixTimestampConverter = CustomerIOSharp.UnixTimestampConverter;

namespace Antibody.CareToKnowPro.CRM.Services
{
    public class CustomerIoService : ICustomerIoService
    {
        private const string TrackEndpoint = "https://track.customer.io/api/v1";
        private const string ApiEndpoint = "https://beta-api.customer.io/v1/api/";

        private readonly ICustomerFactory _customerFactory;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        public CustomerIoService(IConfiguration configuration)
        {
            //  _customerFactory = customerFactory;

            _configuration = configuration;

            this._httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://beta-api.customer.io/v1/api/")
            };


            var token = _configuration["CustomerIO:APIAuthToken"];
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            foreach (var converter in _jsonSerializerSettings.Converters.OfType<DateTimeConverterBase>().ToList())
            {
                _jsonSerializerSettings.Converters.Remove(converter);
            }

            _jsonSerializerSettings.Converters.Add(new UnixTimestampConverter());
        }

        public async Task<List<Message>> GetMessages(int id, string continuationToken = "", int limit = 5)
        {
            if (string.IsNullOrEmpty(continuationToken))
            {
              //  Guid guid = Guid.NewGuid();
               // continuationToken = System.Web.HttpUtility.UrlEncode(guid.ToUIdString());
            }

            var resource = $"customers/{id}/messages?limit={limit}&start={continuationToken}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, resource);
            var result = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new CustomerIoApiException(result.StatusCode, result.ReasonPhrase);
            }

            var response = await result.Content.ReadAsStringAsync();
            var record = JsonConvert.DeserializeObject<CRM.Models.RootObject>(response);

            var messages = record.messages.Select(a => a.Map()).ToList();

            foreach (var message in messages)
            {
                message.Campaign = await GetCampaign(message.CampaignId);
            }

            return messages;
        }

        public async Task<Campaign> GetCampaign(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new Campaign();
            }
            var resource = $"campaigns/{id}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, resource);
            var result = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                //Campaign might be deleted in IO
                return new Campaign();
            }

            var response = await result.Content.ReadAsStringAsync();
            var record = JsonConvert.DeserializeObject<CRM.Models.ParentObject>(response);

            return record.campaign.Map();
        }
    }
}