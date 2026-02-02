public static class ConsoleWriterExtension
{
    // Метод расширения для консольного вывода
    // Который выводит указанное сообщение
    // И завершает программу с указанным кодом завершения
    public static async Task WriteLineAndExitAsync(this ConsoleWriter consoleWriter, string message, int endCode)
    {
        await consoleWriter.WriteLineAsync(message);
        Environment.Exit(endCode);
    }
}