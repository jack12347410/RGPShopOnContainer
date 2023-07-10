using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        private HttpClient _client;
        private ILogger<CustomHttpClient> _logger;

        public CustomHttpClient(HttpClient client, ILogger<CustomHttpClient> logger)
        {
            _client = new HttpClient();
            _logger = logger;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var requestMsg = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await _client.SendAsync(requestMsg);
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var resquestMsg = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _client.SendAsync(resquestMsg);

            return await response.Content.ReadAsStringAsync();
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            return DoPostPutAsync(HttpMethod.Put, uri, item);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            return DoPostPutAsync(HttpMethod.Put, uri, item);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item)
        {
            if(method != HttpMethod.Put && method != HttpMethod.Post) throw new ArgumentException("Value must be either post or put.", nameof(method));

            var requestMsg = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(requestMsg);
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) throw new HttpRequestException();

            return response;
        }
    }
}
