using System.Xml.Serialization;

namespace SharedLibrary.Formatters
{
    public class XmlFormatter : Singleton<XmlFormatter>, IValueFormatter
    {
        public T? FormatTo<T>(string? value)
        {
            if (value == null)
                return default;

            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(value);
            try
            {
                var result = serializer.Deserialize(reader);
                return result is T typedResult ? typedResult : default;
            }
            catch
            {
                return default;
            }
        }
    }
}
