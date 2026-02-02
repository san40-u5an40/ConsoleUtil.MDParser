// Опции для парсинга контента
public record ContentOptions(string Text, int Limit, bool IsAnchorsContains) : IOptions;