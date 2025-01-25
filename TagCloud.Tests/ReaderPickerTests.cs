using FluentAssertions;
using TagCloud.ReadingFiles;

namespace TagCloud.Tests;

[TestFixture]
public class ReaderPickerTests
{
    private readonly ReaderPicker readerProvider = new([ new TxtReader() ]);
    private readonly string pathToFileFolder = Path.Combine(Directory.GetCurrentDirectory(), "TestsFiles");
    
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
        
        var actual = readerProvider.GetSupportedExtensions();
        
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetReader_PngFile_ThrowException()
    {
        var path = Path.Combine(pathToFileFolder, "Morozko.png");
        
        var status = readerProvider.GetReader(path);
        
        status.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void GetReader_CorrectFileExtension_NotThrow()
    {
        var path = Path.Combine(pathToFileFolder, "Morozko.txt");
        
        var call = () => readerProvider.GetReader(path);
        
        call.Should().NotThrow();
    }

    [Test]
    public void GetReader_CorrectPath_СorrectFunction()
    {
        var reader = new TxtReader();
        var path = Path.Combine(pathToFileFolder, "Morozko.txt");
        var expected = () => reader.ReadTextLineByLine(path);
        
        var actual = readerProvider.GetReader(path).GetValueOrThrow();
        
        actual().Should().BeEquivalentTo(expected());
    }
    
    [Test]
    public void GetReader_UnExistingFile_ThrowFileNotFoundException()
    {
        var status = readerProvider.GetReader("UnExistingFile.txt");

        status.IsSuccess.Should().BeFalse();
    }
}