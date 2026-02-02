// Опции для парсинга ссылок
public record LinksOptions: IOptions
{
    public LinksOptions(string text, bool isContainsReferences, string? maskForIntern = null)
    {
        (Text, IsContainsReferences) = (text, isContainsReferences);
        MaskForIntern = string.IsNullOrEmpty(maskForIntern) ? (isContainsReferences ? LinksParser.WithRefsDefault : LinksParser.NoRefsDefault) : maskForIntern;
    }

    public string Text { get; private init; }
    public bool IsContainsReferences { get; private init; }
    public string MaskForIntern { get; private init; }
}