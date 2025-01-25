using ErrorHandling;

namespace TagCloud.Parsing;

public interface IParser
{
    public string TypeOfParsing { get; }
    public Result<WordInfo[]> Parse(Func<IEnumerable<string>> getTextLineByLine);
}