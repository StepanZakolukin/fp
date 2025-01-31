using FluentAssertions;
using TagCloud.Parsing;

namespace TagCloud.Tests;

[TestFixture]
public class WordInfoTests
{
    [Test]
    public void WordInfo_CorrectPartsOfSpeech()
    {
        const int count = 1;
        const string word = "привет";
        
        foreach (var res in WordInfo.PartsOfSpeech
                     .Select(partOfSpeech => WordInfo.Create(word, partOfSpeech, count)))
        {
            res.IsSuccess.Should().BeTrue();
        }
    }
    
    [Test]
    public void WordInfo_IncorrectPartsOfSpeech()
    {
        const int count = 1;
        const string word = "привет";
        var incorrectPartsOfSpeech = new[] { "0", "999", "a", word, "причастие", "деепричастие", "слово" };
        
        foreach (var res in incorrectPartsOfSpeech
                     .Select(partOfSpeech => WordInfo.Create(word, partOfSpeech, count)))
        {
            res.IsSuccess.Should().BeFalse();
        }
    }
}