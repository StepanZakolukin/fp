using ErrorHandling;

namespace TagCloud.ReadingFiles;

public class ReaderPicker : IReaderProvider
{
    private readonly Dictionary<string, IReader> _readers = new();

    public ReaderPicker(IEnumerable<IReader> readers)
    {
        ArgumentNullException.ThrowIfNull(readers);
        foreach (var reader in readers)
        foreach (var extension in reader.AvailableExtensions)
            _readers[extension] = reader;
    }

    public IEnumerable<string> GetSupportedExtensions() => _readers.Keys;

    public Result<Func<IEnumerable<string>>> GetReader(string pathToFile)
    {
        var fileExtension = Path.GetExtension(pathToFile);
        if (!_readers.TryGetValue(fileExtension, out var reader))
            return Result.Fail<Func<IEnumerable<string>>>($"Не найден подходящий {nameof(IReader)} для файла с расширением {fileExtension}");
        if (!Path.Exists(pathToFile))
            return Result.Fail<Func<IEnumerable<string>>>($"Файл {pathToFile} не существует или поврежден");
        return Result.Ok<Func<IEnumerable<string>>>(() => reader.ReadTextLineByLine(pathToFile));
    }
}