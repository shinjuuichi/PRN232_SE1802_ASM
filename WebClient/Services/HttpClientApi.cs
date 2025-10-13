using System.Globalization;
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
            if (_client.DefaultRequestHeaders.Accept.All(h => h.MediaType != "text/csv"))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));
            }
        }

        public async Task<T?> GetAsync<T>(string relativeUrl)
        {
            var resp = await _client.GetAsync(relativeUrl);
            if (!resp.IsSuccessStatusCode) return default;
            var csvContent = await resp.Content.ReadAsStringAsync();
            return ParseCsvSingle<T>(csvContent);
        }

        public async Task<IEnumerable<T>?> GetListAsync<T>(string relativeUrl)
        {
            var resp = await _client.GetAsync(relativeUrl);
            if (!resp.IsSuccessStatusCode) return default;
            var csvContent = await resp.Content.ReadAsStringAsync();
            return ParseCsvList<T>(csvContent);
        }

        private T? ParseCsvSingle<T>(string csvContent)
        {
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return default;

            var headers = ParseCsvLine(lines[0]);
            var values = ParseCsvLine(lines[1]);

            return CreateObject<T>(headers, values);
        }

        private IEnumerable<T>? ParseCsvList<T>(string csvContent)
        {
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return default;

            var headers = ParseCsvLine(lines[0]);
            var result = new List<T>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = ParseCsvLine(lines[i]);
                var obj = CreateObject<T>(headers, values);
                if (obj != null)
                {
                    result.Add(obj);
                }
            }

            return result;
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var inQuotes = false;
            var currentField = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentField.ToString().Trim());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }

            result.Add(currentField.ToString().Trim());
            return result.ToArray();
        }

        private T? CreateObject<T>(string[] headers, string[] values)
        {
            if (headers.Length != values.Length) return default;

            var obj = Activator.CreateInstance<T>();
            var type = typeof(T);

            for (int i = 0; i < headers.Length; i++)
            {
                var property = type.GetProperty(headers[i],
                    System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (property != null && property.CanWrite)
                {
                    var value = ConvertValue(values[i], property.PropertyType);
                    property.SetValue(obj, value);
                }
            }

            return obj;
        }

        private object? ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value) || value == "null") return null;

            // Remove quotes if present
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            if (targetType == typeof(string)) return value;
            if (targetType == typeof(int)) return int.Parse(value);
            if (targetType == typeof(int?)) return string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
            if (targetType == typeof(double)) return double.Parse(value, CultureInfo.InvariantCulture);
            if (targetType == typeof(double?)) return string.IsNullOrEmpty(value) ? (double?)null : double.Parse(value, CultureInfo.InvariantCulture);
            if (targetType == typeof(bool)) return bool.Parse(value);
            if (targetType == typeof(bool?)) return string.IsNullOrEmpty(value) ? (bool?)null : bool.Parse(value);
            if (targetType == typeof(DateTime)) return DateTime.Parse(value);
            if (targetType == typeof(DateTime?)) return string.IsNullOrEmpty(value) ? (DateTime?)null : DateTime.Parse(value);

            return Convert.ChangeType(value, targetType);
        }

        public async Task<(TResponse? data, ValidationProblemDetails? problem)> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data)
            where TResponse : class, new()
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _client.PostAsync(relativeUrl, content);

            if (!resp.IsSuccessStatusCode)
            {
                await using var stream = await resp.Content.ReadAsStreamAsync();
                return (default, await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(stream, _jsonOptions));
            }

            var csvContent = await resp.Content.ReadAsStringAsync();
            var parsedData = ParseCsvSingle<TResponse>(csvContent);

            if (parsedData is null)
            {
                return (new TResponse(), default);
            }
            
            return (parsedData, default);
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
