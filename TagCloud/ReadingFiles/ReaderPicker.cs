namespace TagCloud.ReadingFiles;

public class ReaderPicker : IReaderProvider
{
    private readonly Dictionary<string, IReader> readers = new();
    
    public ReaderPicker(IEnumerable<IReader> readers)
    {
        ArgumentNullException.ThrowIfNull(readers);
        foreach (var reader in readers)
            foreach (var extension in reader.AvailableExtensions)
                this.readers[extension] = reader;
    }

    public IEnumerable<string> GetSupportedExtensions() => readers.Keys;

    public Func<IEnumerable<string>> GetReader(string pathToFile)
    {
        var fileExtension = Path.GetExtension(pathToFile);
        if (!readers.TryGetValue(fileExtension, out var reader))
            throw new Exception($"Не найден подходящий {nameof(IReader)} для файла с расширением {fileExtension}");
        if (!Path.Exists(pathToFile))
            throw new FileNotFoundException($"Файл {pathToFile} не существует или поврежден");
        return () => reader.ReadTextLineByLine(pathToFile);
    }
}