using System.Drawing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.ImageGeneration;
using TagCloud.ImageGeneration.Settings;
using TagCloud.ImageGeneration.Settings.DTO;
using TagCloud.Parsing;
using TagCloud.ReadingFiles;

namespace TagCloud.Tests;

[TestFixture]
public class VisualizationCloudLayoutTests
{
    private const string TypeLiteraryText = "Литературный текст";
    private const string WordListType = "Список слов (по одному в строке)";
    private IParserProvider _parserProvider;
    private IReaderProvider _readerProvider;
    private IVisualizationProvider _visualizationProvider;
    private readonly HashSet<string> _partOfSpeechForFiltering =
    [
        "местоимение-прилагательное",
        "союз",
        "междометие",
        "частица",
        "предлог",
        "местоимение-существительное",
    ];
    
    [Test]
    public void VisualizationCloudLayout_ChangeSettings_SettingsShouldChange()
    {
        const float cloudCompressionRatio = 1.5f;
        var imageSize = new Size(1920, 540);
        var fontName = "Calibri";
        
        _visualizationProvider.Settings.ImageSize.Width = imageSize.Width.ToString();
        _visualizationProvider.Settings.ImageSize.Height = imageSize.Height.ToString();

        _visualizationProvider.Settings.FontFamily.Name = fontName;
        _visualizationProvider.Settings.CompressionRatio.Value = cloudCompressionRatio.ToString();
        
        _visualizationProvider.Settings.ImageSize.GetValueOrThrow().Should().Be(imageSize);
        _visualizationProvider.Settings.FontFamily.Name.Should().Be(fontName);
        _visualizationProvider.Settings.CompressionRatio.GetValueOrThrow().Should().Be(cloudCompressionRatio);
    }
    
    [TestCase("Morozko.txt", "Morozko.jpeg", TypeLiteraryText, 2.6f)]
    [TestCase("GeeseAndSwans.txt", "GeeseAndSwans.png", TypeLiteraryText, 2.8f)]
    [TestCase("CheckingCount.txt", "CheckingCount.bmp", WordListType, 0.4f)]
    public void CreateImage_ImageSizeMustMatchSettings(string fileName, string imageName,
        string structure, float cloudCompressionRatio)
    {
        var sourceFile = Path.Combine("TestsFiles", fileName);
        var reader =  _readerProvider.GetReader(sourceFile);
        _parserProvider.SlectedParser = structure;
        var parser = _parserProvider.GetParser();
        var preprocessingStatus = parser(reader);
        preprocessingStatus.IsSuccess.Should().BeTrue();
        var words = preprocessingStatus.GetValueOrThrow()
            .Where(info => !_partOfSpeechForFiltering.Contains(info.PartOfSpeach));
        _visualizationProvider.Settings.WordsList.Value = words;
        
        _visualizationProvider.Settings.CompressionRatio.Value = cloudCompressionRatio.ToString();
        
        _visualizationProvider.Settings.ImageSize.Width = "1080";
        _visualizationProvider.Settings.ImageSize.Height = "1080";
        CheckSizeMatching(imageName);
        _visualizationProvider.Settings.ImageSize.Width = "1280";
        _visualizationProvider.Settings.ImageSize.Height = "720";
        CheckSizeMatching(imageName);
    }

    private void CheckSizeMatching(string imageName)
    {
        var image = GenerateImage(imageName);
        image.Size.Should().Be(_visualizationProvider.Settings.ImageSize.GetValueOrThrow());
    }

    private Bitmap GenerateImage(string imageName)
    {
        imageName = $"({_visualizationProvider.Settings.ImageSize.Width}" +
                    $"x{_visualizationProvider.Settings.ImageSize.Height})" +
                    $"{imageName}";
        var pathToImage =  $"../../../Images/{imageName}";

        var image = _visualizationProvider.CreateImage().GetValueOrThrow();
        image.Save(pathToImage);

        return image;
    }

    [TestCase("CheckingCount.txt", "CheckingCount.bmp", WordListType, 10f)]
    public void CreateImage_FailedSettings_ImageWillNotBeGenerated(string fileName, string imageName,
        string structure, float cloudCompressionRatio)
    {
        var sourceFile = Path.Combine("TestsFiles", fileName);
        var reader =  _readerProvider.GetReader(sourceFile);
        _parserProvider.SlectedParser = structure;
        var parser = _parserProvider.GetParser();
        var preprocessingStatus = parser(reader);
        preprocessingStatus.IsSuccess.Should().BeTrue();
        _visualizationProvider.Settings.WordsList.Value = preprocessingStatus.GetValueOrThrow();
        
        var status = _visualizationProvider.CreateImage();
        
        status.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void CreateImage_WordsAreNotLoaded_ImageWillNotBeGenerated()
    {
        var status = _visualizationProvider.CreateImage();
        
        status.IsSuccess.Should().BeFalse();
    }

    [SetUp]
    public void PrepareEnvironment()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IReader, TxtReader>();
        services.AddSingleton<IReaderProvider, ReaderPicker>();
        services.AddSingleton<IParser, LiteraryTextParser>();
        services.AddSingleton<IColorProvider, ColorPicker>();
        services.AddSingleton<IParserProvider, ParserProvider>();
        services.AddSingleton<IParser, WordListParser>();
        services.AddSingleton<ILayoutProvider, CircularCloud>();
        services.AddSingleton<IVisualizationProvider, VisualizationCloudLayout>();
        services.AddSingleton<LayoutAlgorithmDto>();
        services.AddSingleton<ColoringAlgorithmDto>();
        services.AddSingleton<RenderingSettings>();
        
        var partialSupplier = services.BuildServiceProvider();
        services.AddSingleton<RenderingSettings>(_ => new RenderingSettings(
            new ImageSizeDto(1080, 1080),
            new FontFamilyDto("Arial"),
            new CompressionRatioDto(2f),
            partialSupplier.GetService<LayoutAlgorithmDto>(),
            partialSupplier.GetService<ColoringAlgorithmDto>(),
            new WordsListDto()));
        var provider = services.BuildServiceProvider();
        
        _parserProvider = provider.GetService<IParserProvider>();
        _readerProvider = provider.GetService<IReaderProvider>();
        _visualizationProvider = provider.GetService<IVisualizationProvider>();
    }
}