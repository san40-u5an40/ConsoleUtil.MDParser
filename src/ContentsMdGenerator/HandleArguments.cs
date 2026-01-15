internal static partial class ContentsMdGenerator
{
    private const int MD_HEAD_MIN_LIMIT = 1;
    private const int MD_HEAD_MAX_LIMIT = 6;

    private const string HELP =
        "Для работы программы необходимо указать md-файл, оглавление которого формируется.\n\n" +
        "Лимит для уровня заголовка можно задать при помощи флага \"/l\". Допустим диапазон от 1 до 6. Например: \"/l 3\".\n\n" +
        "Для помещения в оглавление ссылок на сами заголовки, можно указать флаг \"/a\" (от слова anchor).";

    // Метод для обработки аргументов командой строки
    // 
    // Сначала создаётся и собирается обработчик
    // Если пользователь не указал аргументы, возвращается справочная информация
    // Если просто указаны невалидные аргументы, то возвращается соответствующая ошибка
    // Потом все полученные данные возвращаются из метода
    private static HandleArgumentsResult HandleArguments(string[] args)
    {
        var buildResult = new ArgumentsBuilder(args)
            .AddCustom(IsContainsMDFile, "'{0}' не является md-файлом")
            .AddOptionalPair("/l", IsValidLimit, $"'{0}' не является числом, либо не входит в допустимый диапазон: [{MD_HEAD_MIN_LIMIT};{MD_HEAD_MAX_LIMIT}]")
            .AddOptional("/a")
            .Build();

        if (!buildResult.IsValid)
        {
            if (buildResult.Error.Type == FailureArgumentsType.Zero)
                return HandleArgumentsResult.CreateFailure(HELP);

            return HandleArgumentsResult.CreateFailure(buildResult.Error.Message);
        }

        string pathFile = buildResult.Value[0];
        bool isContainsAnchor = buildResult.Value.Contains("/a");

        int limit = MD_HEAD_MAX_LIMIT;
        if (buildResult.Value.ContainsPair("/l", out string? number))
            limit = int.Parse(number!);

        return HandleArgumentsResult.CreateSuccess(new SuccessHandleArguments(pathFile, limit, isContainsAnchor));

        // Делегат, который проверяет существование указанного md-файла
        static bool IsContainsMDFile(string input)
        {
            var file = new FileInfo(input);

            if (!file.Exists)
                file = new FileInfo(Path.Combine(Environment.CurrentDirectory, input));

            if (!file.Exists)
                return false;

            return file.Extension == ".md";
        }

        // Делегат, который проверяет допустимость указанного лимита для уровня заголовка
        static bool IsValidLimit(string input) =>
            int.TryParse(input, out int number) && number >= MD_HEAD_MIN_LIMIT && number <= MD_HEAD_MAX_LIMIT;
    }
}