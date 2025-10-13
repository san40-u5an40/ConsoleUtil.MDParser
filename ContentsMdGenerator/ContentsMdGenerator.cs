using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentsMdGenerator;

// Принцип работы программы
// 
// Сначала она проверяет наличие аргументов, если они не указаны, выводит соответствующую подсказку
// Затем проверяет в целом существование файла, если его нет, выводит соответствующее уведомление
// После этих проверок она получает лимит на уровень заголовка
// На основе указанного файла и лимита заголовка получает из него оглавление
// Которое затем записывает в отдельный файл с префиксом "contents_" (при желании можно поменять в константе)
internal static class ContentsMdGenerator
{
    private const string OUTPUT_NAME = "contents";   // Стандартный префикс для выходного файла
    private const int LVL_LIMIT = 6;                 // Стандартный лимит для уровня заголовка

    // Точка входа с основной логикой
    internal static void Generate(string[] args)
    {
        if (args.Length == 0 || args.Any(string.IsNullOrEmpty))
        {
            Console.WriteLine("Ошибка: В аргументе необходимо указать название md-файла для формирования его оглавления.");
            return;
        }

        var dir = new DirectoryInfo(Environment.CurrentDirectory);
        var file = new FileInfo(Path.Combine(dir.FullName, args[0]));

        if (!file.Exists || file.Extension != ".md")
        {
            Console.WriteLine("Ошибка: По указанному пути md-файл не найден!");
            return;
        } 

        int lvlLimit = GetLvlLimit(args);

        // Локальная функция получения лимита для уровня заголовка
        static int GetLvlLimit(string[] args)
        {
            if (args.Length > 1 &&
                int.TryParse(args[1], out int num) &&
                num >= 1 && num <= 6)
            {
                return num;
            }

            return LVL_LIMIT;
        }

        // Получение оглавления из файла
        (string? contents, string? error) = GetContents(file, lvlLimit);
        if (error != null)
        {
            Console.WriteLine(error);
            return;
        }

        // Если выходной файл уже существует, спрашивает о перезаписи
        var outputFile = new FileInfo(Path.Combine(dir.FullName, $"{OUTPUT_NAME}_{file.Name}"));
        if(outputFile.Exists)
        {
            Console.Write("Файл уже создан. Перезаписать? (Y/N): _\b");
            var answer = Console.ReadKey();

            Console.Write('\r' + new string(' ', Console.WindowWidth) + '\r'); // Очистка строки

            if (answer.Key != ConsoleKey.Y)
                return;
        }

        // Записывает оглавление в выходной файл
        WriteContents(contents!, outputFile);
    }

    // Принцип работы метода получения оглавления из файла
    // 
    // Сначала он открывает stream и сохраняет весь текст из файла в строковую переменную
    // Если в ходе открытия stream возникли ошибки, возвращает соответствующий результат
    // Также и при отсутствии текста во входном файле
    // Если всё прошло хорошо, то ищет в полученном тексте все заголовки в локальной функции SearchContents
    private static Result GetContents(FileInfo file, int lvlLimit)
    {
        string text;
        try
        {
            using var streamFile = file.OpenText();
            text = streamFile.ReadToEnd();
        }
        catch(Exception ex)
        {
            return new Result(null, "Ошибка:\n" + ex.Message);
        }

        if(string.IsNullOrEmpty(text))
            return new Result(null, "Ошибка: Файл \"" + file.Name + "\" не содержит текста!");

        var result = SearchContents(text, lvlLimit);
        return result;

        // Принцип работы локальной функции с поиском заголовков:
        // 
        // Для поиска заголовков она использует регулярное выражение,
        // Которое использует построчный анализ на соответствие паттерну
        // Все найденные совпадения обрабатываются:
        // В начале строки добавляется табуляция
        // После которой следует элемент списка с оформлением заголовка в формате MarkDown
        // Все найденные строки возвращаются из функции,
        // Ну или информация об ошибках, если они возникли
        static Result SearchContents(string text, int lvlLimit)
        {
            var matches = Regex.Matches(text,
                                    $"^(?<lvl>#{{1,{lvlLimit}}})\\s(?<content>.*?)\\r?$",
                                    RegexOptions.Compiled | RegexOptions.Multiline);

            var output = new StringBuilder();
            foreach (Match match in matches)
            {
                int lvl = match.Groups["lvl"].Value.Length;
                string tab = new string(' ', (lvl - 1) * 4);

                string content = match.Groups["content"].Value;

                output.AppendLine(tab + "- [" + content + "]()");
            }

            return new Result(output.ToString(), null);
        }
    }

    // Метод создания нового файла:
    // 
    // Также открывается stream для нового файла
    // По которому составленное оглавление записывается
    // Если создание текстового файла прошло успешно,
    // Появится соответствующее уведомление
    // В противном случае будет выведен текст ошибки
    private static void WriteContents(string contents, FileInfo outputFile)
    {
        try
        {
            using var streamFile = outputFile.CreateText();

            streamFile.Write(contents);
            streamFile.Flush();

            Console.WriteLine("Файл \"" + outputFile.Name + "\" успешно создан!");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Ошибка:\n" + ex.Message);
        }
    }
}

// Запись для возврата результата из метода
internal record Result(string? Text, string? ErrorMess);