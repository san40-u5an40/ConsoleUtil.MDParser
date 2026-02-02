namespace MdParser.Tests.Parsers;

public class LinksParsingTests
{
    static MarkdownParseProcessor<LinksOptions> processor = new(new LinksParser());

    [TestCaseSource(nameof(DifferentContent))]
    public void Parse_DifferentContent_ReturnExpected(LinksOptions options, string expected)
    {
        string actual = processor
            .Parse(options)
            .Value;

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<TestCaseData> DifferentContent()
    {
        var dataCollection = new List<(string Text, bool IsReferenceContains, string? MaskForIntern, string Expected)>()
        {
            ("Какой-то текст со [ссылкой](на какой-то источник)!", true, null, "- [ссылкой](на какой-то источник)\r\n"),
            ("Какой-то текст со [ссылкой](на какой-то источник)!", false, null, "- ссылкой\r\n"),
            ("Какой-то текст со [ссылкой](на какой-то источник)!", true, $"Ссылка → {LinksParser.LinkInternSymbol}; Её источник → {LinksParser.ReferenceInternSymbol}.", "Ссылка → ссылкой; Её источник → на какой-то источник.\r\n"),
            ("Какой-то текст со [ссылкой](на какой-то источник)!", false, $"Ссылка → {LinksParser.LinkInternSymbol}; Её источник → {LinksParser.ReferenceInternSymbol}.", $"Ссылка → ссылкой; Её источник → {LinksParser.ReferenceInternSymbol}.\r\n"),
            ("А в [этом]() тексте есть [две]() пустые ссылки!", true, null, "- [этом]()\r\n- [две]()\r\n"),
            ("А тут что-то не понятное, [полурак] полуссылка!", false, null, ""),
        };

        foreach (var data in dataCollection)
            yield return new TestCaseData(new LinksOptions(data.Text, data.IsReferenceContains, data.MaskForIntern), data.Expected);
    }
}