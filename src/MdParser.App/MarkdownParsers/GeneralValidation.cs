// Общая логика проверки валидности опций для всех парсеров
public static class GeneralValidation
{
    public static bool IsValid(IOptions options) =>
        options != null &&
        !string.IsNullOrEmpty(options.Text);

    public static string ErrorMessage =>
        "Неверные опции для парсинга Markdown-файла.\n" +
        "Файл не содержит текста!";
}