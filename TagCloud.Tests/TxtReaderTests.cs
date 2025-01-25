using FluentAssertions;
using TagCloud.ReadingFiles;

namespace TagCloud.Tests;

[TestFixture]
public class TxtReaderTests
{
    private readonly TxtReader _reader = new();
    private readonly string _pathToFileFolder = Path.Combine(Directory.GetCurrentDirectory(), "TestsFiles");
    
    [Test]
    public void ReadTextLineByLine_UnExistingFile_ThrowFileNotFoundException()
    {
        var calling = () => _reader.ReadTextLineByLine("UnExistingFile.txt").ToArray();

        calling.Should().Throw<FileNotFoundException>();
    }

    [Test]
    public void ReadTextLineByLine_EmptyFile_EmptyCollectionOfWords()
    {
        var pathToFile = Path.Combine(_pathToFileFolder, "EmptyFile.txt");
        
        var actual = _reader.ReadTextLineByLine(pathToFile);
        
        actual.Should().BeEmpty();
    }
    
    [Test]
    public void ReadTextLineByLine_Text_CorrectWordCount()
    {
        var pathToFile = Path.Combine(_pathToFileFolder, "CheckingCount.txt");
        var generator = new GeneratingTestData();
        var lines = generator.Shuffle(generator.CreateArrayOfWords(GeneratingTestData.FrequencyDictionary));
        File.WriteAllLines(pathToFile, lines);
        
        var result = _reader.ReadTextLineByLine(pathToFile);
        
        result
            .All(line => GeneratingTestData.FrequencyDictionary[line] == result.Count(l => l == line))
            .Should()
            .BeTrue();
    }
    
    [TestCase("Morozko.png")]
    [TestCase("EmptyFile.doc")]
    public void ReadTextLineByLine_UnsuitableFormat_ThrowIOException(string filename)
    {
        var pathToFile = Path.Combine(_pathToFileFolder, filename);
        
        var calling = () => _reader.ReadTextLineByLine(pathToFile).ToArray();

        calling.Should().Throw<IOException>();
    }
}