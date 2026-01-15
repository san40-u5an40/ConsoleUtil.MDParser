internal static partial class ContentsMdGenerator
{
    // Просто создаёт объект FileInfo по указанному пути
    // Добавляя префикс "contents_"
    private static FileInfo GetOutputFileInfo(string sourcePath)
    {
        var sourceFile = new FileInfo(sourcePath);

        string directory = sourceFile.DirectoryName!;
        string name = sourceFile.Name;

        string outputPath = Path.Combine(directory, "contents_" + name);

        return new FileInfo(outputPath);
    }
}