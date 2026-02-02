using OptionsTuple = (string? Text, int Limit, bool IsAnchorContains);

namespace MdParser.Tests.OptionsValidation;

public static class ContentOptionsValidationTests
{
    static MarkdownParseProcessor<ContentOptions> processor = new(new ContentParser());
    static readonly int minAllowValue = ContentParser.MinAllowLimit;
    static readonly int maxAllowValue = ContentParser.MaxAllowLimit;

    [Test]
    public static void When_ComparingMinValueWithOne_Should_BeEqual()
    {
        int minExpected = 1;
        int minActual = minAllowValue;

        Assert.That(minActual, Is.EqualTo(minExpected));
    }

    [Test]
    public static void When_ComparingMaxValueWithMinValue_Should_BeGreater()
    {
        bool IsMaxGreaterMin = maxAllowValue > minAllowValue;
        Assert.That(IsMaxGreaterMin, Is.True);
    }

    [TestCaseSource(nameof(ValidOptionsDataSource))]
    public static void ParserContent_WithValidOptions_ReturnValidResult(ContentOptions options)
    {
        var parseResult = processor.Parse(options);

        Assert.That(parseResult.IsValid, Is.True);
    }

    [TestCaseSource(nameof(InvalidOptionsDataSource))]
    public static void ParserContent_WithNonValidOptions_ReturnNonvalidResult(ContentOptions options)
    {
        var parseResult = processor.Parse(options);

        Assert.That(parseResult.IsValid, Is.False);
    }

    private static IEnumerable<ContentOptions> ValidOptionsDataSource()
    {
        List<OptionsTuple> optionsCases = [
            (string.NotEmpty, minAllowValue, true),
            (string.NotEmpty, maxAllowValue, true),
            (string.NotEmpty, minAllowValue, false),
            (string.NotEmpty, maxAllowValue, false)];

        return CreateOptionsCases(optionsCases);
    }

    private static IEnumerable<ContentOptions> InvalidOptionsDataSource()
    {
        List<OptionsTuple> optionsCases = [
            (string.NotEmpty, minAllowValue - 1, default),
            (string.NotEmpty, maxAllowValue + 1, default)];

        return CreateOptionsCases(optionsCases);
    }

    private static IEnumerable<ContentOptions> CreateOptionsCases(List<OptionsTuple> optionsCases)
    {
        foreach (var optionsCase in optionsCases)
            yield return new ContentOptions(optionsCase.Text!, optionsCase.Limit, optionsCase.IsAnchorContains);
    }
}