using System.Collections.Immutable;
using ErrorHandling;

namespace TagCloud.ReadingFiles;

public class TxtReader : IReader
{
    public ImmutableHashSet<string> SupportedExtensions { get; } = [ ".txt" ];

    public IEnumerable<string> ReadTextLineByLine(string pathToFile)
    {
        using var reader = new StreamReader(pathToFile);
        while (reader.ReadLine() is { } line)
            yield return line.Trim();
    }

    public ActionStatus PerformFileReadValidation(string pathToFile)
    {
        var fileExtension = Path.GetExtension(pathToFile);
        if (!SupportedExtensions.Contains(fileExtension))
            ActionStatus.Fail($"{nameof(TxtReader)} не поддерживает {fileExtension} формат файлов");
        if (!Path.Exists(pathToFile))
            ActionStatus.Fail($"Файл {pathToFile} не существует или поврежден");

        try
        {
            foreach (var _ in ReadTextLineByLine(pathToFile))
            {
            }
        }
        catch (Exception ex)
        {
            return ActionStatus.Fail($"Ошибка чтения файла: {ex.Message}");
        }
        
        return ActionStatus.Ok();
    }
}