using FluentAssertions;
using TagCloud.ReadingFiles;

namespace TagCloud.Tests;

[TestFixture]
public class ReaderPickerTests
{
    private readonly ReaderPicker _readerProvider = new([ new TxtReader() ]);
    private readonly string _pathToFileFolder = Path.Combine(Directory.GetCurrentDirectory(), "TestsFiles");
    
    [Test]
    public void Constructor_Null_ThrowArgumentNullException()
    {
        var initialization = () =>  new ReaderPicker(null);
        
        initialization.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetSupportedExtensions_CorrectNumberOfFormats()
    {
        var expected = new List<string> { ".txt" };
        
        var actual = _readerProvider.GetSupportedExtensions();
        
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetReader_PngFile_ThrowException()
    {
        var path = Path.Combine(_pathToFileFolder, "Morozko.png");
        
        var call = () => _readerProvider.GetReader(path);
        
        call.Should().Throw<Exception>();
    }

    [Test]
    public void GetReader_CorrectFileExtension_NotThrow()
    {
        var path = Path.Combine(_pathToFileFolder, "Morozko.txt");
        
        var call = () => _readerProvider.GetReader(path);
        
        call.Should().NotThrow();
    }

    [Test]
    public void GetReader_CorrectPath_СorrectFunction()
    {
        var reader = new TxtReader();
        var path = Path.Combine(_pathToFileFolder, "Morozko.txt");
        var expected = () => reader.ReadTextLineByLine(path);
        
        var actual = _readerProvider.GetReader(path);
        
        actual().Should().BeEquivalentTo(expected());
    }
    
    [Test]
    public void GetReader_UnExistingFile_ThrowFileNotFoundException()
    {
        var calling = () => _readerProvider.GetReader("UnExistingFile.txt");

        calling.Should().Throw<FileNotFoundException>();
    }
}