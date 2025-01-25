using FluentAssertions;
using TagCloud.ReadingFiles;

namespace TagCloud.Tests;

[TestFixture]
public class TxtReaderTests
{
    private readonly TxtReader reader = new();
    private readonly string pathToFileFolder = Path.Combine(Directory.GetCurrentDirectory(), "TestsFiles");
    
    [Test]
    public void ReadTextLineByLine_UnExistingFile_ThrowFileNotFoundException()
    {
        var calling = () => reader.ReadTextLineByLine("UnExistingFile.txt").ToArray();

        calling.Should().Throw<FileNotFoundException>();
    }

    [Test]
    public void ReadTextLineByLine_EmptyFile_EmptyCollectionOfWords()
    {
        var pathToFile = Path.Combine(pathToFileFolder, "EmptyFile.txt");
        
        var actual = reader.ReadTextLineByLine(pathToFile);
        
        actual.Should().BeEmpty();
    }
    
    [Test]
    public void ReadTextLineByLine_Text_CorrectWordCount()
    {
        var pathToFile = Path.Combine(pathToFileFolder, "CheckingCount.txt");
        var frequencyDictionary = new Dictionary<string, int>
        {
            { "привет", 5 },
            { "морозный", 7 },
            { "быстрый", 3 },
            { "я", 20 },
            { "человек", 2},
            { "отчаянно", 8},
        };
        var lines = CreateArrayOfWords(frequencyDictionary);
        var random = new Random();
        random.Shuffle(lines);
        File.WriteAllLines(pathToFile, lines);
        
        var result = reader.ReadTextLineByLine(pathToFile);
        
        result.All(line => frequencyDictionary[line] == result.Count(l => l == line)).Should().BeTrue();
    }
    
    [TestCase("Morozko.png")]
    [TestCase("EmptyFile.doc")]
    public void ReadTextLineByLine_UnsuitableFormat_ThrowIOException(string filename)
    {
        var pathToFile = Path.Combine(pathToFileFolder, filename);
        
        var calling = () => reader.ReadTextLineByLine(pathToFile).ToArray();

        calling.Should().Throw<IOException>();
    }
    
    public static string[] CreateArrayOfWords(Dictionary<string, int> frequencyDictionary)
    {
        var list = new List<string>();
        foreach (var pair in frequencyDictionary)
            for (var i = 0; i < pair.Value; i++)
                list.Add(pair.Key);
        
        return list.ToArray();
    }
}