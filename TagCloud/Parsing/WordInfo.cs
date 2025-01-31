using System.Collections.Immutable;
using ErrorHandling;

namespace TagCloud.Parsing;

public record WordInfo
{
    public static ImmutableHashSet<string> PartsOfSpeech { get; } =
    [
        "прилагательное",
        "наречие",
        "местоименное наречие",
        "числительное-прилагательное",
        "местоимение-прилагательное",
        "часть композита - сложного слова",
        "союз",
        "междометие",
        "числительное",
        "частица",
        "предлог",
        "существительное",
        "местоимение-существительное",
        "глагол",
        "нет данных"
    ];

    private WordInfo(string word, string partOfSpeach, int numberInText)
    {
        Word = word;
        PartOfSpeach = partOfSpeach;
        NumberInText = numberInText;
    }
    
    public static Result<WordInfo> Create(string word, int numberInText)
    {
        return Result.Ok(new WordInfo(word, "нет данных", numberInText));
    }

    public static Result<WordInfo> Create(string word, string partOfSpeach, int numberInText)
    {
        if (!PartsOfSpeech.Contains(partOfSpeach))
            return Result.Fail<WordInfo>($"неизвестная чаcть речи: {partOfSpeach}");
        return Result.Ok(new WordInfo(word, partOfSpeach, numberInText));
    }

    public string Word { get; }
    public string PartOfSpeach { get; }
    public int NumberInText { get; }
}