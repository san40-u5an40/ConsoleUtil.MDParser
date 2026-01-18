internal static partial class ContentsMdGenerator
{
    // Метод получения оглавления из файла, с указанным лимитом уровня заголовка
    // 
    // Сначала создаются переменные для работы метода: информация об указанном файле и переменная для хранения его текста
    // Открывается стрим из которого и получается текст, который записывается в эту переменную
    // Если текст пустой, возвращается соответствующая ошибка
    // После этого в переменную StringBuilder вносятся все заголовки и возвращается соответствующий результат
    private static ContentResult GetContents(ArgumentsInfo argumentsInfo)
    {
        var fileInfo = new FileInfo(argumentsInfo.Path);
        string fileText;

        try
        {
            using var streamFile = fileInfo.OpenText();
            fileText = streamFile.ReadToEnd();
        }
        catch (Exception ex)
        {
            return ContentResult.CreateFailure(ex.Message);
        }

        if (string.IsNullOrEmpty(fileText))
            return ContentResult.CreateFailure("Файл \"" + fileInfo.Name + "\" не содержит текста!");

        var contents = new StringBuilder();
        AppendContents(contents, fileText, argumentsInfo.Limit, argumentsInfo.IsContainsAnchor);

        return ContentResult.CreateSuccess(new ContentInfo(argumentsInfo, contents));

        // Локальная функция добавления заголовков в переменную StringBuilder
        // 
        // Сначала она парсит из текста по регулярному выражению все заголовки
        // После чего перебирает получившиеся совпадения и добавляет их в StringBuilder
        // При необходимости добавляет якоря на сами заголовки
        static void AppendContents(StringBuilder sb, string fileText, int limit, bool isContainsAnchor)
        {
            var matches = Regex.Matches(fileText,
                                   $"^(?<lvl>#{{1,{limit}}})\\s(?<content>.*?)\\r?\\n?$",
                                   RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                int lvl = match.Groups["lvl"].Value.Length;
                string tab = new string(' ', (lvl - 1) * 4);

                string contentName = match.Groups["content"].Value;
                string contentHref = isContainsAnchor ? AnchorFormat(contentName) : string.Empty;

                sb.AppendLine($"{tab}- [{contentName}]({contentHref})");
            }

            // Локальная функция форматирования под якорь
            // 
            // Используется простой алгоритм, т.к. специфика форматирования зависит от платформы, на которой используется md-файл
            static string AnchorFormat(string content) =>
                '#' + content
                .ToLower()
                .Replace(' ', '-');
        }
    }
}