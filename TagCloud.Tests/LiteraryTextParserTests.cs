using System.Collections.Immutable;
using FluentAssertions;
using TagCloud.Parsing;

namespace TagCloud.Tests;

[TestFixture]
public class LiteraryTextParserTests
{
    private readonly LiteraryTextParser _literaryTextParser = new();
    private readonly ImmutableArray<string> _testLines;
    private readonly Dictionary<string, int> _frequencyDictionary = new()
    {
        { "привет", 5 },
        { "морозный", 7 },
        { "быстрый", 3 },
        { "я", 20 },
        { "человек", 2},
        { "отчаянно", 8},
    };
    private readonly HashSet<char> _russianAlphabet = [];

    public LiteraryTextParserTests()
    {
        for (var symbol = 'а'; symbol <= 'я'; symbol++)
            _russianAlphabet.Add(symbol);
        _russianAlphabet.Add('ё');
        
        var lines = TxtReaderTests.CreateArrayOfWords(_frequencyDictionary);
        var random = new Random();
        random.Shuffle(lines);
        _testLines = [..lines];
    }
    
    [Test]
    public void PerformPreprocessing_Text_AllCharactersMustBeInLowercase()
    {
        var lines = new[] { "привет", "ПрИвЕт", "Привет", "ПРИВЕТ" };

        var result = _literaryTextParser.Parse(() => lines);

        result.IsSuccess.Should().BeTrue();
        CheckCharactersOfWords(result.GetValueOrThrow(), symbol => char.IsLower(symbol) || symbol == '-');
    }

    [Test]
    public void PerformPreprocessing_Text_OnlyRussianLettersShouldRemainInWords()
    {
        var lines = new[] { "python?", "java!", "C#", "языки-", "программирования", "пriveт", "из-за" };
        
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
        result.GetValueOrThrow().All(wordInfo => _frequencyDictionary[wordInfo.Word] == wordInfo.NumberInText).Should().BeTrue();
    }
}