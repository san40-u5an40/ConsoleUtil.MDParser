// Результат работы основных методов программы
global using ArgumentsResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<ArgumentsInfo, string>;
global using ContentResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<ContentInfo, string>;
global using OutputResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<object, string>;
global using WriterResult = san40_u5an40.ExtraLib.Broad.Patterns.Result<WriterInfo, string>;

// Данные, указанные пользователем в командной строке
internal record ArgumentsInfo(string Path, int Limit, bool IsContainsAnchor);
internal record ContentInfo(ArgumentsInfo ArgumentsInfo, StringBuilder Content);
internal record WriterInfo(StringBuilder Content, FileInfo FileInfo);

internal static partial class ContentsMdGenerator
{
    // Точка входа и основная логика приложения
    // 
    // Сначала обрабатываются аргументы командой строки
    // Затем получается оглавления из файла, в соответствии с данными, указанными пользователем
    // И в конце полученное оглавление записывается в новый файл
    // Если всё прошло гуд, то выводится уведомление об успешном создании оглавления
    // Если что-то не гуд, то выводится информация о возникшей ошибке
    internal static void Run(string[] args)
    {
        var chainResult = new Chain<string[], object, string>(args)
            .AddMethod<string[], ArgumentsInfo>(HandleArguments)
            .AddMethod<ArgumentsInfo, ContentInfo>(GetContents)
            .AddMethod<ContentInfo, WriterInfo>(GetOutputFileInfo)
            .AddMethod<WriterInfo, object>(WriteContents)
            .Execute();

        if (!chainResult.IsValid)
        {
            Console.WriteLine(chainResult.Error);
            return;
        }

        Console.WriteLine("Файл с оглавлением успешно создан!");
    }
}