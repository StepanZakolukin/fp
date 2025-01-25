using System.Drawing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.ImageGeneration;
using TagCloud.ImageGeneration.Settings;
using TagCloud.ImageGeneration.Settings.DTO;

namespace TagCloud.Tests.SettingsTests;

[TestFixture]
public class RenderingSettingsTests
{
    private RenderingSettings _settings;
    private LayoutAlgorithmDto _layoutAlgorithmDto;
    private ColoringAlgorithmDto _coloringAlgorithmDto;
    private WordsListDto _wordsListDto;
    
    [SetUp]
    public void PrepareEnvironment()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IColorProvider, ColorPicker>();
        services.AddSingleton<ILayoutProvider, CircularCloud>();
        services.AddSingleton<LayoutAlgorithmDto>();
        services.AddSingleton<ColoringAlgorithmDto>();
        
        var partialSupplier = services.BuildServiceProvider();
        _layoutAlgorithmDto = partialSupplier.GetRequiredService<LayoutAlgorithmDto>();
        _coloringAlgorithmDto = partialSupplier.GetRequiredService<ColoringAlgorithmDto>();
        _wordsListDto = new WordsListDto();
        services.AddSingleton<RenderingSettings>(_ => new RenderingSettings(
            new ImageSizeDto(1080, 1080),
            new FontFamilyDto("Arial"),
            new CompressionRatioDto(5f),
            _layoutAlgorithmDto,
            _coloringAlgorithmDto,
            _wordsListDto));
        var provider = services.BuildServiceProvider();
        
        _settings = provider.GetRequiredService<RenderingSettings>();
    }

    [TestCase(-100, 100, "Arial", 5)]
    [TestCase(100, -100, "Arial", 5)]
    [TestCase(100, 0, "Arial", 5)]
    [TestCase(0, 100, "Arial", 5)]
    [TestCase(100, 100, "mmm", 5)]
    [TestCase(100, 100, "Arial", 0)]
    [TestCase(100, 100, "Arial", 11)]
    public void VisualizationSettingsDto_IncorrectInitialization_ThrowsException(
        int width, int height,
        string fontName,
        float coefficient)
    {
        var calling = () => new RenderingSettings(
            new ImageSizeDto(width, height),
            new FontFamilyDto(fontName),
            new CompressionRatioDto(coefficient),
            _layoutAlgorithmDto,
            _coloringAlgorithmDto,
            _wordsListDto);

        calling.Should().Throw<ArgumentException>();
    }
    
    [TestCase(100, 100, "Arial", 5)]
    public void VisualizationSettingsDto_CorrectInitialization_NotThrow(
        int width, int height,
        string fontName,
        float coefficient)
    {
        var calling = () => new RenderingSettings(
            new ImageSizeDto(width, height),
            new FontFamilyDto(fontName),
            new CompressionRatioDto(coefficient),
            _layoutAlgorithmDto,
            _coloringAlgorithmDto,
            _wordsListDto);

        calling.Should().NotThrow();
    }

    [TestCase(0)]
    [TestCase(-1000f)]
    [TestCase(1000f)]
    [TestCase(10.01f)]
    public void SetValueCloudCompressionRatio_IncorrectValue(float coefficient)
    {
        var expected = _settings.CompressionRatio.Value;
        _settings.CompressionRatio.Value = coefficient.ToString();

        _settings.CompressionRatio.IsCorrect.Should().BeFalse();
        _settings.CompressionRatio.Value.Should().NotBe(expected);
    }
    
    [TestCase(2)]
    [TestCase(10f)]
    [TestCase(0.1f)]
    public void SetValueCloudCompressionRatio_CorrectValue(float coefficient)
    {
        _settings.CompressionRatio.Value = coefficient.ToString();

        _settings.CompressionRatio.IsCorrect.Should().BeTrue();
        _settings.CompressionRatio.Value.Should().Be(coefficient.ToString());
    }

    [TestCase(540, 200)]
    [TestCase(100, 1080)]
    public void SetValueImageSize_PositiveValues(int width, int height)
    {
        _settings.ImageSize.Width = width.ToString();
        _settings.ImageSize.Height = height.ToString();
        
        _settings.ImageSize.IsCorrect.Should().BeTrue();
        _settings.ImageSize.GetValueOrThrow().Should().Be(new Size(width, height));
    }
    
    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void SetValueImageSize_NotPositiveValues(int width, int height)
    {
        _settings.ImageSize.Width = width.ToString();
        _settings.ImageSize.Height = height.ToString();
        var calling = () => _settings.ImageSize.GetValueOrThrow();
        
        _settings.ImageSize.IsCorrect.Should().BeFalse();
        _settings.ImageSize.Width.Should().Be(width.ToString());
        _settings.ImageSize.Height.Should().Be(height.ToString());
        calling.Should().Throw<InvalidOperationException>();
    }
}