// Основная логика обработки команд-парсеров
public abstract class CommandBase<TOptions>(IParser<TOptions> _parser) : ICommand
    where TOptions : IOptions
{
    // Обязательная общая опция - файл с источником текста
    [CommandOption("file", 'f', IsRequired = true)]
    public string File { get; init; } = string.Empty;

    // Абстрактный метод создания опций парсера из свойств экземпляра команды
    public abstract Result<TOptions, string> CreateOptionsFromProperties(string text);

    // Логика выполнения команд
    // 
    // Сначала создаётся парсер
    // И затем исполняется цепочка из следующих действий
    // Путь до файла конвертируется в объект FileInfo
    // Проверяется его расширение
    // Получается весь текст из данного файла
    // Создаются опции для парсера
    // Парсится текст
    // 
    // Если на каком-то из этапов возникнет ошибка
    // Будет выведено соответствующее уведомление
    // Иначе будет выведен спарсенный текст
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var parseProcessor = new MarkdownParseProcessor<TOptions>(_parser);

        var parseTextFromFileResult = new Chain<string, string, string>(File)
            .AddMethod<string, FileInfo>(IOHelpers.ConvertPathToFile, out Readyable<FileInfo> sourceFile)
            .AddMethod<FileInfo, FileInfo>(IOHelpers.ValidationExtension)
            .AddMethod<FileInfo, string>(IOHelpers.GetAllTextFromFile)
            .AddMethod<string, TOptions>(CreateOptionsFromProperties)
            .AddMethod<TOptions, string>(parseProcessor.Parse)
            .Execute();

        if (!parseTextFromFileResult.IsValid)
            await console.Output.WriteAsync(parseTextFromFileResult.Error);
        else
            await console.Output.WriteAsync(parseTextFromFileResult.Value);
    }
}