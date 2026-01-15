internal static partial class ContentsMdGenerator
{
    // Метод записи оглавления в новый файл
    // 
    // В случае существования файла с оглавлением у пользователя спрашивается необходимость его перезаписи
    // После чего открывается стрим, по которому и записывается оглавление
    private static WriteContentsResult WriteContents(string contents, FileInfo outputFileInfo)
    {
        if (!AskContinueIfOutputFileContains(outputFileInfo))
            return WriteContentsResult.CreateFailure("Операция отменена");

        try
        {
            using var streamFile = outputFileInfo.CreateText();

            streamFile.Write(contents);
            streamFile.Flush();

            return WriteContentsResult.CreateSuccess(new object());
        }
        catch (Exception ex)
        {
            return WriteContentsResult.CreateFailure(ex.Message);
        }

        // В случае существования файла у пользователя спрашивается необходимость его перезаписи
        static bool AskContinueIfOutputFileContains(FileInfo outputFile)
        {
            if (!outputFile.Exists)
                return true;

            Console.Write("Файл уже создан. Перезаписать? (Y/N): _\b");
            bool answer = Console.ReadKey().Key == ConsoleKey.Y;
            Console.CleanLine();

            return answer;
        }
    }
}