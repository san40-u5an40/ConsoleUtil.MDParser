internal static partial class ContentsMdGenerator
{
    // Просто создаёт объект FileInfo по указанному пути
    // Добавляя префикс "contents_"
    private static WriterResult GetOutputFileInfo(ContentInfo contentInfo)
    {
        var sourceFile = new FileInfo(contentInfo.ArgumentsInfo.Path);

        string directory = sourceFile.DirectoryName!;
        string name = sourceFile.Name;

        string outputPath = Path.Combine(directory, "contents_" + name);

        return WriterResult.CreateSuccess(new WriterInfo(contentInfo.Content, new FileInfo(outputPath)));
    }
}