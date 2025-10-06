using WebClient.Models;

namespace WebClient.Services
{
    public interface IHttpClientApi
    {
        Task<T?> GetAsync<T>(string relativeUrl);
        Task<IEnumerable<T>?> GetListAsync<T>(string relativeUrl);
        Task<(TResponse? data, ValidationProblemDetails? problem)> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data);
        Task<(bool success, ValidationProblemDetails? problem)> PutAsync<TRequest>(string relativeUrl, TRequest data);
        Task<bool> DeleteAsync(string relativeUrl);
    }
}
