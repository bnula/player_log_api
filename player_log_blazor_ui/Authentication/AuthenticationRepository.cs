using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using player_log_blazor_ui.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace player_log_blazor_ui.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _storage;
        private readonly AuthenticationStateProvider _authStateProvider;
        public AuthenticationRepository(
            IHttpClientFactory clientFactory,
            ILocalStorageService storage,
            AuthenticationStateProvider authStateProvider)
        {
            _clientFactory = clientFactory;
            _storage = storage;
            _authStateProvider = authStateProvider;
        }
        public async Task<bool> Login(LoginModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.Login);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<ResponseModel>(content);

            await _storage.SetItemAsync("authToken", token.Token);

            await ((APIAuthenticationStateProvider)_authStateProvider).LoggedIn();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

            return true;
        }

        public async Task Logout()
        {
            await _storage.RemoveItemAsync("authToken");
            ((APIAuthenticationStateProvider)_authStateProvider).LoggedOut();
        }

        public async Task<bool> Register(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.Register);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
