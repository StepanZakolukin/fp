using System.Collections.Immutable;
using ErrorHandling;

namespace TagCloud.ReadingFiles;

public interface IReader
{
    public ImmutableHashSet<string> SupportedExtensions { get; }
    public IEnumerable<string> ReadTextLineByLine(string pathToFile);
    public ActionStatus PerformFileReadValidation(string pathToFile);
}