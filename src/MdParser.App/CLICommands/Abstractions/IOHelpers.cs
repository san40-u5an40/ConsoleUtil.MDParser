using ResultWithFileAndString = san40_u5an40.ExtraLib.Broad.Patterns.Result<System.IO.FileInfo, string>;
using ResultWithDoubleString = san40_u5an40.ExtraLib.Broad.Patterns.Result<string, string>;

public class IOHelpers
{
    // Конвертирует текстовый путь до файла в объект FileInfo (с сопутствующими проверками)
    public static ResultWithFileAndString ConvertPathToFile(string path)
    {
        if (string.IsNullOrEmpty(path))
            return ResultWithFileAndString.CreateFailure("Указанный путь для создания файла пустой!");

        if (File.Exists(path))
            return ResultWithFileAndString.CreateSuccess(new FileInfo(path));

        string fileWithCurrentDirectory = Path.Combine(Environment.CurrentDirectory, path);
        var finalFile = new FileInfo(fileWithCurrentDirectory);

        if (finalFile.Exists)
            return ResultWithFileAndString.CreateSuccess(finalFile);
        else
            return ResultWithFileAndString.CreateFailure($"Указанный файл \"{path}\" не был найден!");
    }

    // Проверка расширения файла
    public static ResultWithFileAndString ValidationExtension(FileInfo sourceFile)
    {
        if (sourceFile.Extension == ".md")
            return ResultWithFileAndString.CreateSuccess(sourceFile);
        else
            return ResultWithFileAndString.CreateFailure("Необходимо указать .md-файл!");
    }

    // Получение из файла текста
    public static ResultWithDoubleString GetAllTextFromFile(FileInfo sourceFile)
    {
        string text;
        try
        {
            text = sourceFile
                .OpenText()
                .ReadToEnd();
        }
        catch (Exception ex)
        {
            return ResultWithDoubleString.CreateFailure($"При открытии файла \"{sourceFile.Name}\" возникла ошибка:\n" + ex.Message);
        }

        if (string.IsNullOrEmpty(text))
            return ResultWithDoubleString.CreateFailure("В указанном файле отсутствует текст!");
        else
            return ResultWithDoubleString.CreateSuccess(text);
    }

    // Добавление к файлу указанного префикса
    public static FileInfo AddPrefixFromFile(FileInfo sourceFile, string prefix)
    {
        string directory = sourceFile.DirectoryName!;
        string name = sourceFile.Name;
        string finalPath = Path.Combine(directory, prefix + '_' + name);

        return new FileInfo(finalPath);
    }

    // Вопрос пользователю о необходимости перезаписать файл
    public static bool AskOverwrite(IConsole console, string filePath)
    {
        console.Output.Write($"Файл \"{filePath}\" уже существует. Вы хотите перезаписать его? (\"Y\" - для подтверждения):_\b");
        string? answer = console.Input.ReadLine();
        return answer == "Y" || answer == "y";
    }
}