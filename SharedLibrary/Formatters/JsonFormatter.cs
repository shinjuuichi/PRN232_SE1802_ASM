using System.Text.Json;

namespace SharedLibrary.Formatters
{
    public class JsonFormatter : Singleton<JsonFormatter>, IValueFormatter
    {
        public T? FormatTo<T>(string? value)
        {
            if (value == null)
                return default;

            try
            {
                var result = JsonSerializer.Deserialize<T>(value);
                return result is T typedResult ? typedResult : default;
            }
            catch
            {
                return default;
            }
        }
    }
}
