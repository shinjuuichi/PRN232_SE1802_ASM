using CsvHelper;
using System.Globalization;

namespace SharedLibrary.Formatters
{
    public class CsvFormatter : Singleton<CsvFormatter>, IValueFormatter
    {
        public T? FormatTo<T>(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;

            try
            {
                using var reader = new StringReader(value);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var t = typeof(T);

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elem = t.GetGenericArguments()[0];
                    var method = typeof(CsvReader)
                        .GetMethods()
                        .First(m => m.Name == "GetRecords" && m.IsGenericMethod && m.GetParameters().Length == 0)
                        .MakeGenericMethod(elem);

                    var records = method.Invoke(csv, null)!;
                    var toList = typeof(Enumerable)
                        .GetMethod("ToList")!
                        .MakeGenericMethod(elem)
                        .Invoke(null, [records]);

                    return (T)toList!;
                }
                else
                {
                    var record = csv.GetRecords<T>().FirstOrDefault();
                    return record;
                }
            }
            catch
            {
                return default;
            }
        }
    }
}