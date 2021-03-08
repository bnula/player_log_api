using Blazored.LocalStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace player_log_blazor_ui.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _storage;

        public BaseRepository(
            IHttpClientFactory clientFactory,
            ILocalStorageService storage)
        {
            _clientFactory = clientFactory;
            _storage = storage;
        }

        public async Task<bool> Create(string url, T item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (item == null)
            {
                return false;
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {

            if (id < 1)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
            
        }

        public async Task<T> Get(string url, int id)
        {
            if (id < 1)
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        public async Task<List<T>> GetAll(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content);
            }

            return null;
        }

        public async Task<bool> Update(string url, T item, int id)
        {
            if (item == null)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Put, url + id);
            request.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        private async Task<string> GetBearerToken()
        {
            var savedToken = await _storage.GetItemAsync<string>("authToken");
            return savedToken;
        }
    }
}
