using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace AspnetRunBasics.ApiCollection.Infrastructure
{
    public abstract class BaseHttpClientWithFactory
    {
        private readonly IHttpClientFactory _factory;

        public Uri BaseAddress { get; set; }
        public string BasePath { get; set; }

        protected BaseHttpClientWithFactory(IHttpClientFactory factory) => _factory = factory;

        private HttpClient GetHttpClient() => _factory.CreateClient();

        protected virtual async Task<T> SendRequest<T>(HttpRequestMessage request) where T : class
        {
            var client = GetHttpClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<T>(GetFormatters())
                : default;
        }

        protected virtual IEnumerable<MediaTypeFormatter> GetFormatters() => new List<MediaTypeFormatter> {new JsonMediaTypeFormatter()};

        public abstract HttpRequestBuilder GetHttpRequestBuilder(string path);
    }
}