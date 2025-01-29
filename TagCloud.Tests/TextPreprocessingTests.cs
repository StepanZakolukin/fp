using System.Collections.Immutable;
using FluentAssertions;
using TagCloud.ReadingFiles;
using TagCloud.TextProcessing;
using TagCloudGUI.Controls;

namespace TagCloud.Tests;

[TestFixture]
public class TextPreprocessingTests
{
    private readonly TextPreprocessing textPreprocessing = new();
    private readonly ImmutableArray<string> testLines;
    private readonly Dictionary<string, int> frequencyDictionary = new()
    {
        { "привет", 5 },
        { "морозный", 7 },
        { "быстрый", 3 },
        { "я", 20 },
        { "человек", 2},
        { "отчаянно", 8},
    };
    private readonly HashSet<char> russianAlphabet = [];

    public TextPreprocessingTests()
    {
        for (var symbol = 'а'; symbol <= 'я'; symbol++)
            russianAlphabet.Add(symbol);
        russianAlphabet.Add('ё');
        
        var lines = TxtReaderTests.CreateArrayOfWords(frequencyDictionary);
        var random = new Random();
        random.Shuffle(lines);
        testLines = [..lines];
    }

    [TestCase(ContentStructure.Literary)]
    [TestCase(ContentStructure.ListOfWords)]
    public void PerformPreprocessing_Text_AllCharactersMustBeInLowercase(ContentStructure typeOfContent)
    {
        var lines = new[] { "привет", "ПрИвЕт", "Привет", "ПРИВЕТ" };

        var result = textPreprocessing.PerformPreprocessing(
            () => lines,
            ContentStructure.Literary);

        CheckCharactersOfWords(result, symbol => char.IsLower(symbol) || symbol == '-');
    }

    [TestCase(ContentStructure.Literary)]
    public void PerformPreprocessing_Text_OnlyRussianLettersShouldRemainInWords(ContentStructure typeOfContent)
    {
        var lines = new[] { "python?", "java!", "C#", "языки-", "программирования", "пriveт", "из-за" };
        
        var result = textPreprocessing.PerformPreprocessing(
            () => lines,
            typeOfContent);
        
        CheckCharactersOfWords(result, symbol => russianAlphabet.Contains(symbol) || symbol == '-');
    }

    private void CheckCharactersOfWords(IEnumerable<WordInfo> words, Func<char, bool> check)
    {
        foreach (var wordInfo in words)
            wordInfo.Word.All(check).Should().BeTrue();
    }

    [TestCase(ContentStructure.Literary)]
    [TestCase(ContentStructure.ListOfWords)]
    public void PerformPreprocessing_Text_CorrectWordCount(ContentStructure typeOfContent)
    {
        var result = textPreprocessing.PerformPreprocessing(
            () => testLines,
            typeOfContent);
        
        result.All(wordInfo => frequencyDictionary[wordInfo.Word] == wordInfo.NumberInText).Should().BeTrue();
    }
}