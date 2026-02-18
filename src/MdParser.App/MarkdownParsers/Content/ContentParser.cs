// Парсер контента
public class ContentParser : IParser<ContentOptions>
{
    public string Prefix => "content";

    private const string REGEX_LEVEL = "lvl";
    private const string REGEX_CONTENT = "cont";

    public string Parse(ContentOptions options)
    {
        var matchCollection = ParseContentCollection(options);
        var stringBuilder = FormatIntoStringBuilder(matchCollection, options.IsAnchorsContains);
        return stringBuilder.ToString();

        static MatchCollection ParseContentCollection(ContentOptions options) =>
            Regex.Matches(options.Text,
                          $"^(?<{REGEX_LEVEL}>#{{1,{options.Limit}}})\\s(?<{REGEX_CONTENT}>.*?)\\r?\\n?$",
                          RegexOptions.Multiline);

        static StringBuilder FormatIntoStringBuilder(MatchCollection matches, bool isContainsAnchor)
        {
            StringBuilder outputContent = new();

            foreach (Match match in matches)
            {
                int lvl = match.Groups[REGEX_LEVEL].Value.Length;
                string tab = new(' ', (lvl - 1) * 4);

                string contentName = match.Groups[REGEX_CONTENT].Value;
                string contentHref = isContainsAnchor ? AnchorFormat(contentName) : string.Empty;

                outputContent.AppendLine($"{tab}- [{contentName}]({contentHref})");
            }

            return outputContent;

            static string AnchorFormat(string content) =>
                '#' + content
                .ToLower()
                .Replace(' ', '-');
        }
    }

    public static int MinAllowLimit => 1;
    public static int MaxAllowLimit => 6;

    public bool IsValid(ContentOptions options) =>
        options.Limit >= MinAllowLimit && options.Limit <= MaxAllowLimit;
    public string ErrorMessage => "Неверные опции для парсинга заголовков из Markdown-файла.\n" +
                                 $"Лимит для заголовка не входит в допустимый диапазон [{MinAllowLimit}-{MaxAllowLimit}].";
}