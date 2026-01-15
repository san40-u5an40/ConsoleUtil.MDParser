// Результат работы основных методов программы
global using HandleArgumentsResult = san40_u5an40.ExtraLib.Broad.Result<SuccessHandleArguments, string>;
global using GetContentsResult = san40_u5an40.ExtraLib.Broad.Result<System.Text.StringBuilder, string>;
global using WriteContentsResult = san40_u5an40.ExtraLib.Broad.Result<object, string>;

// Данные, указанные пользователем в командной строке
internal record SuccessHandleArguments(string Path, int Limit, bool IsContainsAnchor);

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
        var handleArgumentsResult = HandleArguments(args);
        if (!handleArgumentsResult.IsValid)
        {
            Console.WriteLine(handleArgumentsResult.Error);
            return;
        }

        var getContentsResult = GetContents(handleArgumentsResult.Value.Path, handleArgumentsResult.Value.Limit, handleArgumentsResult.Value.IsContainsAnchor);
        if (!getContentsResult.IsValid)
        {
            Console.WriteLine(getContentsResult.Error);
            return;
        }

        var outputFileInfo = GetOutputFileInfo(handleArgumentsResult.Value.Path);
        var writeContentsResult = WriteContents(getContentsResult.Value.ToString(), outputFileInfo);
        if (!writeContentsResult.IsValid)
        {
            Console.WriteLine(writeContentsResult.Error);
            return;
        }

        Console.WriteLine("Файл с оглавлением успешно создан!");
    }
}