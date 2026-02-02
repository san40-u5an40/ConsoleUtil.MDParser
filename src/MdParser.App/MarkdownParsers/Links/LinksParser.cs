// Парсер ссылок
public class LinksParser : IParser<LinksOptions>
{
    public string Prefix => "links";

    private const string LINK = "link";
    private const string REF = "reference";

    public string Parse(LinksOptions options)
    {
        var matchCollection = ParseLinksCollection(options);
        var stringBuilder = FormatIntoStringBuilder(matchCollection, options.IsContainsReferences, options.MaskForIntern);
        return stringBuilder.ToString();

        static MatchCollection ParseLinksCollection(LinksOptions options) =>
            Regex.Matches(options.Text,
                          $"\\[(?<{LINK}>.*?)\\]\\((?<{REF}>.*?)\\)",
                          RegexOptions.Compiled);

        static StringBuilder FormatIntoStringBuilder(MatchCollection matches, bool isContainsReferences, string maskForIntern)
        {
            StringBuilder outputContent = new();

            foreach (Match match in matches)
            {
                string linkTextLine = InternLink(maskForIntern, match.Groups[LINK].Value);
                if (isContainsReferences)
                    linkTextLine = InternRef(linkTextLine, match.Groups[REF].Value);

                outputContent.AppendLine(linkTextLine);
            }

            return outputContent;

            static string InternLink(string mask, string link) =>
                mask.Replace(LinkInternSymbol, link);

            static string InternRef(string mask, string reference) =>
                mask.Replace(ReferenceInternSymbol, reference);
        }
    }

    public static string LinkInternSymbol => "@1";
    public static string ReferenceInternSymbol => "@2";
    public static string NoRefsDefault => $"- {LinkInternSymbol}";
    public static string WithRefsDefault => $"- [{LinkInternSymbol}]({ReferenceInternSymbol})";

    public bool IsValid(LinksOptions options)
    {
        return options.IsContainsReferences ? ContainsTwoSymbols(options.MaskForIntern) : ContainsOneSymbol(options.MaskForIntern);

        static bool ContainsTwoSymbols(string mask) =>
        mask.Contains(LinkInternSymbol) && mask.Contains(ReferenceInternSymbol);
        static bool ContainsOneSymbol(string mask) =>
        mask.Contains(LinkInternSymbol);
    }
    public string ErrorMessage => "Неверные опции для парсинга ссылок из Markdown-файла.\n" +
                                  "Маска для ссылок некорректно форматирует текст.";
}