using FluentAssertions;
using TagCloud.TextProcessing;

namespace TagCloud.Tests;

[TestFixture]
public class WordInfoTests
{
    private readonly HashSet<string> _partsOfSpeech =
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
        "глагол"
    ];
    
    [Test]
    public void WordInfo_CorrectPartsOfSpeech_NoExceptions()
    {
        const int count = 1;
        const string word = "привет";
        
        foreach (var elem in _partsOfSpeech)
        {
            var partOfSpeech = elem;
            var initialization = () => new WordInfo(word, partOfSpeech, count);
            initialization.Should().NotThrow();
        }
    }
    
    [Test]
    public void WordInfo_IncorrectPartsOfSpeech_ThrowArgumentExceptionExceptions()
    {
        const int count = 1;
        const string word = "привет";
        var incorrectPartsOfSpeech = new[] { "0", "999", "a", word, "причастие", "деепричастие", "слово" };
        
        foreach (var elem in incorrectPartsOfSpeech)
        {
            var partOfSpeech = elem;
            var initialization = () => new WordInfo(word, partOfSpeech, count);
            initialization.Should().Throw<ArgumentException>();
        }
    }
}