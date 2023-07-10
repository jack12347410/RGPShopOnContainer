using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly ILogger<CatalogService> _logger;
        private readonly string _remoteServiceBaseUrl;

        public CatalogService(IOptionsSnapshot<AppSettings> settings, IHttpClient httpClient, ILogger<CatalogService> logger)
        {
            _settings = settings;
            _apiClient = httpClient;
            _logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/catalog/";
        }


        public async Task<Catalog> GetCatalogItems(int? type, int pageSize, int pageIndex)
        {
            var uri = ApiPaths.Catalog.GetAllCatalogItemsUri(_remoteServiceBaseUrl, type, pageSize, pageIndex);
            var jsonData = await _apiClient.GetStringAsync(uri);
            var response = JsonConvert.DeserializeObject<Catalog>(jsonData);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var uri = ApiPaths.Catalog.GetAllTypesUri(_remoteServiceBaseUrl);
            var jsonData = await _apiClient.GetStringAsync(uri);

            var items = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = null,
                    Text = "All",
                    Selected = true,
                }
            };

            var types = JArray.Parse(jsonData);
            foreach(var brand in types.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("type")
                });
            }

            return items;
        }
    }
}
