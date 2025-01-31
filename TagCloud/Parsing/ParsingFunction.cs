using ErrorHandling;

namespace TagCloud.Parsing;

public delegate Result<WordInfo[]> ParsingFunction(Func<IEnumerable<string>> getTextLineByLine);