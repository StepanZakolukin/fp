using ErrorHandling;

namespace TagCloud.Parsing;

public class WordListParser : IParser
{
    private const string PartOfSpeach = "нет данных";
    public string TypeOfParsing => "Список слов (по одному в строке)";
    
    public Result<WordInfo[]> Parse(Func<IEnumerable<string>> getTextLineByLine)
    {
        var countingDictionary = new Dictionary<string, int>();

        foreach (var line in getTextLineByLine())
        {
            if (line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length > 1)
                return Result.Fail<WordInfo[]>("Формат содержания файла не соответствует настройкам." +
                                               " Ожидалось, что в файле будут слова по одному в строке");
            var word = line.Trim().ToLower();
            if (word == string.Empty) continue;
            if (!countingDictionary.TryAdd(word, 1))
                countingDictionary[word]++;
        }
        
        var result =  countingDictionary
            .Select(pair => new WordInfo(pair.Key, PartOfSpeach, pair.Value))
            .ToArray();
        
        return result.Length == 0 ? Result.Fail<WordInfo[]>("Файл оказался пустым") : Result.Ok(result);
    }
}