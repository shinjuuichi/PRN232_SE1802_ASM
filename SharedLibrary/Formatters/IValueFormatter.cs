namespace SharedLibrary.Formatters
{
    public interface IValueFormatter
    {
        T? FormatTo<T>(string? value);
    }
}
