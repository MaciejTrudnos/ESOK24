using esok.api.DTO;
using esok.api.DTO.PageAccessTokens;
using esok.api.Interfaces.Services;
using System.Text.Json;

namespace esok.api.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FacebookService> _logger;

        public FacebookService(HttpClient httpClient,
            ILogger<FacebookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private async Task<string> GetAccessToken()
        {
            var userAccessToken = "*********";
            
            try
            {
                var response = await _httpClient
                    .GetFromJsonAsync<DataDTO>($"https://graph.facebook.com/v19.0/6995721457147957/accounts?access_token={userAccessToken}");

                var accessToken = response
                    .Data[0]
                    .Access_token;

                return accessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<string> AddPost(string message, string address)
        {
            var accessToken = await GetAccessToken();

            var link = "https://esok24.pl/blog-details.html?title=" + address;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.facebook.com/v19.0/234322339764471/feed?message={message}&link={link}&access_token={accessToken}");

                var response = await _httpClient
                    .SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response
                        .Content
                        .ReadAsStringAsync();

                    var result = JsonSerializer
                        .Deserialize<PostDTO>(responseBody);

                    return result.Id;
                }

                throw new HttpRequestException($"{response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
