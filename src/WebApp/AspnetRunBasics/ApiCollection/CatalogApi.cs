using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Infrastructure;
using AspnetRunBasics.ApiCollection.Interfaces;
using AspnetRunBasics.Models;
using AspnetRunBasics.Settings;

namespace AspnetRunBasics.ApiCollection
{
    public class CatalogApi : BaseHttpClientWithFactory, ICatalogApi
    {
        private readonly IApiSettings _settings;
        
        public CatalogApi(IHttpClientFactory factory, IApiSettings settings) : base(factory) => _settings = settings;

        public override HttpRequestBuilder GetHttpRequestBuilder(string path)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogs()
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                .SetPath(_settings.CatalogPath)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();

            return await SendRequest<IEnumerable<CatalogModel>>(message);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                .SetPath(_settings.CatalogPath)
                .AddToPath(category)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();

            return await SendRequest<IEnumerable<CatalogModel>>(message);
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                .SetPath(_settings.CatalogPath)
                .AddToPath(id)
                .HttpMethod(HttpMethod.Get)
                .GetHttpMessage();

            return await SendRequest<CatalogModel>(message);
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                .SetPath(_settings.CatalogPath)
                .HttpMethod(HttpMethod.Post)
                .GetHttpMessage();

            var json = JsonSerializer.Serialize(model);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<CatalogModel>(message);
        }
    }
}