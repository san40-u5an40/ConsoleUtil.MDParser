namespace MdParser.Tests.Parsers;

public static class ContentParsingTests
{
    static MarkdownParseProcessor<ContentOptions> processor = new(new ContentParser());

    [TestCaseSource(nameof(DifferentContent))]
    public static void Parse_DifferentContent_ReturnExpected(ContentOptions options, string expected)
    {
        string actual = processor
            .Parse(options)
            .Value;

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<TestCaseData> DifferentContent()
    {
        var dataCollection = new List<(string Text, int Limit, bool IsAnchorContains, string Expected)>()
        {
            ("## Заголовок второго уровня", 2, false, "    - [Заголовок второго уровня]()\r\n"),
            ("## Заголовок второго уровня", 1, false, ""),
            ("## Заголовок второго уровня", 2, true, "    - [Заголовок второго уровня](#заголовок-второго-уровня)\r\n"),
            ("# Первый\n## Второй", 1, false, "- [Первый]()\r\n"),
            ("# Первый\r\n## Второй", 1, false, "- [Первый]()\r\n"),
            ("# Первый\r\n## Второй", 2, false, "- [Первый]()\r\n    - [Второй]()\r\n"),
        };

        foreach (var data in dataCollection)
            yield return new TestCaseData(new ContentOptions(data.Text, data.Limit, data.IsAnchorContains), data.Expected);
    }
}