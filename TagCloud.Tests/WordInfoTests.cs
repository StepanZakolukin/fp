using FluentAssertions;
using TagCloud.Parsing;

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
    public void WordInfo_CorrectPartsOfSpeech()
    {
        const int count = 1;
        const string word = "привет";
        
        foreach (var elem in _partsOfSpeech)
        {
            var partOfSpeech = elem;
            var res = WordInfo.Create(word, partOfSpeech, count);
            res.IsSuccess.Should().BeTrue();
        }
    }
    
    [Test]
    public void WordInfo_IncorrectPartsOfSpeech()
    {
        const int count = 1;
        const string word = "привет";
        var incorrectPartsOfSpeech = new[] { "0", "999", "a", word, "причастие", "деепричастие", "слово" };
        
        foreach (var elem in incorrectPartsOfSpeech)
        {
            var partOfSpeech = elem;
            var res = WordInfo.Create(word, partOfSpeech, count);
            res.IsSuccess.Should().BeFalse();
        }
    }
}