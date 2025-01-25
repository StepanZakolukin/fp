using System.Collections.Immutable;
using FluentAssertions;
using TagCloud.Parsing;
using TagCloud.TextProcessing;

namespace TagCloud.Tests;

[TestFixture]
public class LiteraryTextParserTests
{
    private readonly LiteraryTextParser _literaryTextParser = new();
    private ImmutableArray<string> _testLines;
    
    private readonly HashSet<char> _russianAlphabet = [];

    [SetUp]
    public void SetUp()
    {
        for (var symbol = 'а'; symbol <= 'я'; symbol++)
            _russianAlphabet.Add(symbol);
        _russianAlphabet.Add('ё');

        var generator = new GeneratingTestData();
        _testLines = [..generator.Shuffle(generator.CreateArrayOfWords(GeneratingTestData.FrequencyDictionary))];
    }
    
    [TestCase("привет", "ПрИвЕт", "Привет", "ПРИВЕТ")]
    public void PerformPreprocessing_Text_AllCharactersMustBeInLowercase(params string[] lines)
    {
        var result = _literaryTextParser.Parse(() => lines);

        result.IsSuccess.Should().BeTrue();
        CheckCharactersOfWords(result.GetValueOrThrow(), symbol => char.IsLower(symbol) || symbol == '-');
    }

    [TestCase("python?", "java!", "C#", "языки-", "программирования", "пriveт", "из-за")]
    public void PerformPreprocessing_Text_OnlyRussianLettersShouldRemainInWords(params string[] lines)
    {
        var result = _literaryTextParser.Parse(() => lines);

        result.IsSuccess.Should().BeTrue();
        CheckCharactersOfWords(result.GetValueOrThrow(), symbol => _russianAlphabet.Contains(symbol) || symbol == '-');
    }

    private void CheckCharactersOfWords(IEnumerable<WordInfo> words, Func<char, bool> check)
    {
        foreach (var wordInfo in words)
            wordInfo.Word.All(check).Should().BeTrue();
    }

    [Test]
    public void PerformPreprocessing_Text_CorrectWordCount()
    {
        var result = _literaryTextParser.Parse(() => _testLines);
        
        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow()
            .All(wordInfo => GeneratingTestData.FrequencyDictionary[wordInfo.Word] == wordInfo.NumberInText)
            .Should()
            .BeTrue();
    }
}