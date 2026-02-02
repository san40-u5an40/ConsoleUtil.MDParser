using OptionsTuple = (string Text, bool IsContainsReference, string MaskForIntern);

namespace MdParser.Tests.OptionsValidation;

public static class LinksOptionsValidationTests
{
    static MarkdownParseProcessor<LinksOptions> processor = new(new LinksParser());

    [TestCaseSource(nameof(ValidMasksDataSource))]
    public static void ParseLinks_WithValidOptions_ReturnValidResult(LinksOptions options)
    {
        var parseResult = processor.Parse(options);

        Assert.That(parseResult.IsValid, Is.True);
    }

    [TestCaseSource(nameof(InvalidMasksDataSource))]
    public static void ParseLinks_WithNonValidOptions_ReturnNonValidResult(LinksOptions options)
    {
        var parseResult = processor.Parse(options);

        Assert.That(parseResult.IsValid, Is.False);
    }

    private static IEnumerable<LinksOptions> ValidMasksDataSource()
    {
        List<OptionsTuple> optionsCases = [
            (string.NotEmpty, true, LinksParser.WithRefsDefault),
            (string.NotEmpty, false, LinksParser.NoRefsDefault),
            (string.NotEmpty, true, $"{LinksParser.LinkInternSymbol}{LinksParser.ReferenceInternSymbol}"),
            (string.NotEmpty, true, $"{LinksParser.ReferenceInternSymbol}{LinksParser.LinkInternSymbol}"),
            (string.NotEmpty, true, $"{LinksParser.LinkInternSymbol}{LinksParser.ReferenceInternSymbol}{LinksParser.LinkInternSymbol}{LinksParser.ReferenceInternSymbol}"),
            (string.NotEmpty, false, $"{LinksParser.LinkInternSymbol}"),
            (string.NotEmpty, false, $"{LinksParser.LinkInternSymbol}{LinksParser.LinkInternSymbol}{LinksParser.LinkInternSymbol}"),
            (string.NotEmpty, false, $"{LinksParser.LinkInternSymbol}{LinksParser.ReferenceInternSymbol}")];

        return CreateOptionsCases(optionsCases);
    }

    private static IEnumerable<LinksOptions> InvalidMasksDataSource()
    {
        List<OptionsTuple> optionsCases = [
            (string.NotEmpty, false, $"]_(-01=+;:Gg.,/"),
            (string.NotEmpty, false, $"{LinksParser.ReferenceInternSymbol}"),
            (string.NotEmpty, true, $"{LinksParser.LinkInternSymbol}")];

        return CreateOptionsCases(optionsCases);
    }

    private static IEnumerable<LinksOptions> CreateOptionsCases(List<OptionsTuple> optionsCases)
    {
        foreach (var optionsCase in optionsCases)
            yield return new LinksOptions(optionsCase.Text!, optionsCase.IsContainsReference, optionsCase.MaskForIntern);
    }
}