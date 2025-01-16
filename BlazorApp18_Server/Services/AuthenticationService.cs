// Services/AuthenticationService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using ModelsClassLibrary.Models.DTO;

namespace BlazorApp18_Server.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public async Task Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { username, password });
            response.EnsureSuccessStatusCode();

            var tokens = await response.Content.ReadFromJsonAsync<AuthTokenDTO>();
            Session.SetString("AccessToken", tokens.AccessToken);
            Session.SetString("RefreshToken", tokens.RefreshToken);
        }

        public async Task Logout()
        {
            Session.Remove("AccessToken");
            Session.Remove("RefreshToken");
        }

        public string GetAccessToken()
        {
            return Session.GetString("AccessToken");
        }

        public string GetRefreshToken()
        {
            return Session.GetString("RefreshToken");
        }

        public async Task RefreshToken()
        {
            var refreshToken = GetRefreshToken();
            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
            response.EnsureSuccessStatusCode();

            var tokens = await response.Content.ReadFromJsonAsync<AuthTokenDTO>();
            Session.SetString("AccessToken", tokens.AccessToken);
            Session.SetString("RefreshToken", tokens.RefreshToken);
        }
    }
}
