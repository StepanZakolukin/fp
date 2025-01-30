using ErrorHandling;

namespace TagCloud.Parsing;

public interface IParserProvider : ICrrectnessChecker
{
    public string SlectedParser { get; set; }
    public IEnumerable<string> GetTypesParsers { get; }
    public Func<Func<IEnumerable<string>>, Result<WordInfo[]>>? GetParser();
}