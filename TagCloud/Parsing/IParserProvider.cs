using ErrorHandling;

namespace TagCloud.Parsing;

public delegate Result<WordInfo[]> ParsingFunction(Func<IEnumerable<string>> getTextLineByLine);

public interface IParserProvider : ICrrectnessChecker
{
    public string SlectedParser { get; set; }
    public IEnumerable<string> GetTypesParsers { get; }
    public ParsingFunction? GetParser();
}