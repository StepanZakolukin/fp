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
    private IParserProvider parserProvider;
    private IReaderProvider readerProvider;
    private IVisualizationProvider visualizationProvider;
    private readonly HashSet<string> partOfSpeechForFiltering =
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
        
        visualizationProvider.Settings.ImageSize.Width = imageSize.Width.ToString();
        visualizationProvider.Settings.ImageSize.Height = imageSize.Height.ToString();

        visualizationProvider.Settings.FontFamily.Name = fontName;
        visualizationProvider.Settings.CompressionRatio.Value = cloudCompressionRatio.ToString();
        
        visualizationProvider.Settings.ImageSize.GetValueOrThrow().Should().Be(imageSize);
        visualizationProvider.Settings.FontFamily.Name.Should().Be(fontName);
        visualizationProvider.Settings.CompressionRatio.GetValueOrThrow().Should().Be(cloudCompressionRatio);
    }
    
    [TestCase("Morozko.txt", "Morozko.jpeg", TypeLiteraryText, 2.6f)]
    [TestCase("GeeseAndSwans.txt", "GeeseAndSwans.png", TypeLiteraryText, 2.8f)]
    [TestCase("CheckingCount.txt", "CheckingCount.bmp", WordListType, 0.4f)]
    public void CreateImage_ImageSizeMustMatchSettings(string fileName, string imageName,
        string structure, float cloudCompressionRatio)
    {
        var sourceFile = Path.Combine("TestsFiles", fileName);
        var reader =  readerProvider.GetReader(sourceFile);
        parserProvider.SlectedParser = structure;
        var parser = parserProvider.GetParser();
        var preprocessingStatus = parser(reader);
        preprocessingStatus.IsSuccess.Should().BeTrue();
        var words = preprocessingStatus.GetValueOrThrow()
            .Where(info => !partOfSpeechForFiltering.Contains(info.PartOfSpeach));
        visualizationProvider.Settings.WordsList.Value = words;
        
        visualizationProvider.Settings.CompressionRatio.Value = cloudCompressionRatio.ToString();
        
        visualizationProvider.Settings.ImageSize.Width = "1080";
        visualizationProvider.Settings.ImageSize.Height = "1080";
        CheckSizeMatching(imageName);
        visualizationProvider.Settings.ImageSize.Width = "1280";
        visualizationProvider.Settings.ImageSize.Height = "720";
        CheckSizeMatching(imageName);
    }

    private void CheckSizeMatching(string imageName)
    {
        var image = GenerateImage(imageName);
        image.Size.Should().Be(visualizationProvider.Settings.ImageSize.GetValueOrThrow());
    }

    private Bitmap GenerateImage(string imageName)
    {
        imageName = $"({visualizationProvider.Settings.ImageSize.Width}" +
                    $"x{visualizationProvider.Settings.ImageSize.Height})" +
                    $"{imageName}";
        var pathToImage =  $"../../../Images/{imageName}";

        var image = visualizationProvider.CreateImage().GetValueOrThrow();
        image.Save(pathToImage);

        return image;
    }

    [TestCase("CheckingCount.txt", "CheckingCount.bmp", WordListType, 10f)]
    public void CreateImage_FailedSettings_ImageWillNotBeGenerated(string fileName, string imageName,
        string structure, float cloudCompressionRatio)
    {
        var sourceFile = Path.Combine("TestsFiles", fileName);
        var reader =  readerProvider.GetReader(sourceFile);
        parserProvider.SlectedParser = structure;
        var parser = parserProvider.GetParser();
        var preprocessingStatus = parser(reader);
        preprocessingStatus.IsSuccess.Should().BeTrue();
        visualizationProvider.Settings.WordsList.Value = preprocessingStatus.GetValueOrThrow();
        
        var status = visualizationProvider.CreateImage();
        
        status.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void CreateImage_WordsAreNotLoaded_ImageWillNotBeGenerated()
    {
        var status = visualizationProvider.CreateImage();
        
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
        
        parserProvider = provider.GetService<IParserProvider>();
        readerProvider = provider.GetService<IReaderProvider>();
        visualizationProvider = provider.GetService<IVisualizationProvider>();
    }
}