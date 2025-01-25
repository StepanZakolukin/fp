using FluentAssertions;
using TagCloud.ImageGeneration.Settings.DTO;

namespace TagCloud.Tests.SettingsTests;

[TestFixture]
public class ImageSizeDtoTests
{
    private readonly ImageSizeDto _imageSize = new(1080, 1080);

    [TestCase("540", "200")]
    [TestCase("100", "1080")]
    public void ImageSizeDto_PositiveValues(string width, string height)
    {
        _imageSize.Width = width;
        _imageSize.Height = height;

        _imageSize.IsCorrect.Should().BeTrue();
        _imageSize.Width.Should().Be(width);
        _imageSize.Height.Should().Be(height);
    }

    [TestCase("-100", "100")]
    [TestCase("100", "-100")]
    [TestCase("0", "100")]
    [TestCase("100", "0")]
    public void ImageSizeDto_NotPositiveValues_ValueMustBeIncorrect(string width, string height)
    {
        CheckForInaccuracy(width, height);
    }

    [TestCase("hmm", "100")]
    [TestCase("100", "hmm")]
    [TestCase("hmm", "nnh")]

    public void ImageSizeDto_NotNumber_ValueMustBeIncorrect(string width, string height)
    {
        CheckForInaccuracy(width, height);
    }
    
    [TestCase(1080, 1920)]
    public void GetValueOrThrow_CorrectValue_NotThrowException(float width, float height)
    {
        _imageSize.Width = width.ToString();
        _imageSize.Height = height.ToString();
        var action = () => _imageSize.GetValueOrThrow();
        
        action.Should().NotThrow();
    }
    
    [TestCase("1920", "-1")]
    [TestCase("-1", "1920")]
    [TestCase("1080", "hh")]
    [TestCase("-h", "1080")]
    public void GetValueOrThrow_IncorrectValue_ThrowException(string width, string height)
    {
        _imageSize.Width = width;
        _imageSize.Height = height;
        var action = () => _imageSize.GetValueOrThrow();
        
        action.Should().Throw<InvalidOperationException>();
    }

    private void CheckForInaccuracy(string width, string height)
    {
        _imageSize.Width = width;
        _imageSize.Height = height;
        
        _imageSize.IsCorrect.Should().BeFalse();
        _imageSize.Width.Should().Be(width);
        _imageSize.Height.Should().Be(height);
    }
}