using System.Collections.Immutable;

namespace TagCloud.ReadingFiles;

public interface IReader
{
    public ImmutableHashSet<string> AvailableExtensions { get; }
    public IEnumerable<string> ReadTextLineByLine(string pathToFile);
}