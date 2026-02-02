using CreateOptionsResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<LinksOptions, string>;

// Команда для парсинга ссылок из текстового md-файла
[Command("links")]
public class LinksCLICommand(IParser<LinksOptions> _parser) : CommandBase<LinksOptions>(_parser)
{
    [CommandOption("references", 'r', IsRequired = false)]
    public bool IsContainsReferences { get; init; } = false;

    [CommandOption("mask", 'm', IsRequired = false)]
    public string MaskForIntern { get; init; } = string.Empty;

    public override Result<LinksOptions, string> CreateOptionsFromProperties(string text) =>
        CreateOptionsResult.CreateSuccess(new LinksOptions(text, IsContainsReferences, MaskForIntern));
}