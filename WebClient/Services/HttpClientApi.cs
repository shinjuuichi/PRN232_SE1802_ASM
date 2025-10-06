using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebClient.Models;

namespace WebClient.Services
{
    public class HttpClientApi : IHttpClientApi
    {
        private readonly HttpClient _client;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpClientApi(HttpClient client)
        {
            _client = client;
            if (_client.DefaultRequestHeaders.Accept.All(h => h.MediaType != "application/json"))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<T?> GetAsync<T>(string relativeUrl)
        {
            var resp = await _client.GetAsync(relativeUrl);
            if (!resp.IsSuccessStatusCode) return default;
            await using var stream = await resp.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
        }

        public async Task<IEnumerable<T>?> GetListAsync<T>(string relativeUrl)
        {
            var resp = await _client.GetAsync(relativeUrl);
            if (!resp.IsSuccessStatusCode) return default;
            await using var stream = await resp.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream, _jsonOptions);
        }

        public async Task<(TResponse? data, ValidationProblemDetails? problem)> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _client.PostAsync(relativeUrl, content);
            await using var stream = await resp.Content.ReadAsStreamAsync();
            if (!resp.IsSuccessStatusCode)
            {
                return (default, await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(stream, _jsonOptions));
            }
            return (await JsonSerializer.DeserializeAsync<TResponse>(stream, _jsonOptions), default);
        }

        public async Task<(bool success, ValidationProblemDetails? problem)> PutAsync<TRequest>(string relativeUrl, TRequest data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _client.PutAsync(relativeUrl, content);
            if (!resp.IsSuccessStatusCode)
            {
                await using var stream = await resp.Content.ReadAsStreamAsync();
                return (false, await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(stream, _jsonOptions));
            }
            return (true, default);
        }

        public async Task<bool> DeleteAsync(string relativeUrl)
        {
            var resp = await _client.DeleteAsync(relativeUrl);
            return resp.IsSuccessStatusCode;
        }
    }
}
