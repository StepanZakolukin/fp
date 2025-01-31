namespace TagCloud.Parsing;

public interface IParserProvider : ICrrectnessChecker
{
    public string SlectedParser { get; set; }
    public IEnumerable<string> GetTypesParsers { get; }
    public ParsingFunction? GetParser();
}
