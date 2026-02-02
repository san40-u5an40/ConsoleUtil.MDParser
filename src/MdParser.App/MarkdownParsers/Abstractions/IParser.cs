// Интерфейс для парсера
public interface IParser<in TOptions>
    where TOptions : IOptions
{
    public string Prefix { get; }
    public string Parse(TOptions options);
    public bool IsValid(TOptions options);
    public string ErrorMessage { get; }
}