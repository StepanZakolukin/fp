using ErrorHandling;

namespace TagCloud.Parsing;

public class ParserProvider : CrrectnessChecker, IParserProvider
{
    private readonly Dictionary<string, IParser> _parsers;
    
    private string _selectedParser;
    public string SlectedParser
    {
        get => _selectedParser;
        set
        {
            _selectedParser = value;
            if (_selectedParser is "" or null)
                ChangeValue(false, "Значение не должно быть пустым");
            else if (!_parsers.ContainsKey(_selectedParser))
                ChangeValue(false, "Тип содержания не найден");
            else ChangeValue(true);
        }
    }

    public IEnumerable<string> GetTypesParsers => _parsers.Keys;

    public Func<Func<IEnumerable<string>>, Result<WordInfo[]>>? GetParser()
    {
        return IsCorrect ? _parsers[_selectedParser].Parse : null; 
    }

    public ParserProvider(IEnumerable<IParser> parsers, IParser defaultParser)
    {
        ArgumentNullException.ThrowIfNull(parsers);
        _parsers = parsers.ToDictionary(parser => parser.TypeOfParsing, parser => parser);
        ArgumentNullException.ThrowIfNull(defaultParser);
        if (!_parsers.ContainsKey(defaultParser.TypeOfParsing))
            throw new AggregateException($"{nameof(defaultParser)} должен быть указан в {nameof(parsers)}");
        SlectedParser = defaultParser.TypeOfParsing;
    }
}