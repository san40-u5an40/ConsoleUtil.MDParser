using CreateOptionsResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<ContentOptions, string>;

// Команда для парсинга контента из текстового md-файла
[Command("content")]
public class ContentCLICommand(IParser<ContentOptions> _parser) : CommandBase<ContentOptions>(_parser)
{
    [CommandOption("limit", 'l', IsRequired = false)]
    public int Limit { get; init; } = 6;

    [CommandOption("anchor", 'a', IsRequired = false)]
    public bool IsAnchorContains { get; init; } = false;

    public override Result<ContentOptions, string> CreateOptionsFromProperties(string text) =>
        CreateOptionsResult.CreateSuccess(new ContentOptions(text, Limit, IsAnchorContains));
}