internal static partial class ContentsMdGenerator
{
    // Метод записи оглавления в новый файл
    // 
    // В случае существования файла с оглавлением у пользователя спрашивается необходимость его перезаписи
    // После чего открывается стрим, по которому и записывается оглавление
    private static OutputResult WriteContents(WriterInfo writerInfo)
    {
        if (!AskContinueIfOutputFileContains(writerInfo.FileInfo))
            return OutputResult.CreateFailure("Операция отменена");

        try
        {
            using var streamFile = writerInfo.FileInfo.CreateText();

            streamFile.Write(writerInfo.Content);
            streamFile.Flush();

            return OutputResult.CreateSuccess(new object());
        }
        catch (Exception ex)
        {
            return OutputResult.CreateFailure(ex.Message);
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