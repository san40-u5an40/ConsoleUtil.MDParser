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
    // Будет выведено соответствующее уведомление и программа завершит свою работу
    // 
    // Затем к файлу добавляется префикс, тем самым получается выходной файл для записи спарсенного текста
    // Если такой файл существует, у пользователя спрашивается необходимость его перезаписи
    // Если он выбрал перезаписать данные, то данные соответственно перезаписываются
    // В конце, если всё хорошо, будет выведено уведомление об успешном создании файла
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
            await console.Output.WriteLineAndExitAsync(parseTextFromFileResult.Error, 1);

        var outputFile = IOHelpers.AddPrefixFromFile(sourceFile.Value, _parser.Prefix);
        if (outputFile.Exists && !IOHelpers.AskOverwrite(console, outputFile.Name))
            await console.Output.WriteLineAndExitAsync("Операция отменена пользователем", 1);

        using (var outputStream = outputFile.CreateText())
        {
            await outputStream.WriteAsync(parseTextFromFileResult.Value);
            await outputStream.FlushAsync();
        }

        await console.Output.WriteLineAndExitAsync($"Файл \"{outputFile.Name}\" успешно создан", 0);
    }
}