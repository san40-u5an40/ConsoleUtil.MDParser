using ParseResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<string, string>;

// Главный исполнитель парсинга, задающий шаблон данного процесса
public class MarkdownParseProcessor<TOptions>(IParser<TOptions> _parser)
    where TOptions : IOptions
{
    public Result<string, string> Parse(TOptions options)
    {
        if (!GeneralValidation.IsValid(options))
            return ParseResult.CreateFailure(GeneralValidation.ErrorMessage);

        if (!_parser.IsValid(options))
            return ParseResult.CreateFailure(_parser.ErrorMessage);

        string parsed = _parser.Parse(options);
        return ParseResult.CreateSuccess(parsed);
    }
}