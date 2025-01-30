using System.Collections.Immutable;

namespace TagCloud.ReadingFiles;

public class TxtReader : IReader
{
    public ImmutableHashSet<string> SupportedExtensions { get; } = ImmutableHashSet.Create<string>(".txt");

    public IEnumerable<string> ReadTextLineByLine(string pathToFile)
    {
        var fileExtension = Path.GetExtension(pathToFile);
        if (!SupportedExtensions.Contains(fileExtension))
            throw new IOException($"{nameof(TxtReader)} не поддерживает {fileExtension} формат файлов");
        if (!Path.Exists(pathToFile))
            throw new FileNotFoundException($"Файл {pathToFile} не существует или поврежден");

        using var reader = new StreamReader(pathToFile);
        while (reader.ReadLine() is { } line)
            yield return line.Trim();
    }
}