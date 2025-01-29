using ErrorHandling;

namespace TagCloud.ReadingFiles;

public interface IReaderProvider
{
    public IEnumerable<string> GetSupportedExtensions();
    public Result<Func<IEnumerable<string>>> GetReader(string pathToFile);
}