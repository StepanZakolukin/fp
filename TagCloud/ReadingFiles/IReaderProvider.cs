namespace TagCloud.ReadingFiles;

public interface IReaderProvider
{
    public IEnumerable<string> GetSupportedExtensions();
    public Func<IEnumerable<string>> GetReader(string pathToFile);
}