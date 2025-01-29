namespace TagCloud.TextProcessing;

public record WordInfo
{
    private readonly HashSet<string> partsOfSpeech =
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

    public WordInfo(string word, string partOfSpeach, int numberInText)
    {
        Word = word;
        if (!partsOfSpeech.Contains(partOfSpeach))
            throw new ArgumentException("Некорректная чаcть речи", nameof(partOfSpeach));
        PartOfSpeach = partOfSpeach;
        NumberInText = numberInText;
    }

    public string Word { get; }
    public string PartOfSpeach { get; }
    public int NumberInText { get; }
}