using System.Drawing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.ImageGeneration;
using TagCloud.ReadingFiles;
using TagCloud.TextProcessing;
using TagCloudGUI.Controls;

namespace TagCloud.Tests;

[TestFixture]
public class VisualizationCloudLayoutTests
{
    private readonly IWordsProvider wordsProvider;
    private readonly IReaderProvider readerProvider;
    private readonly IVisualizationProvider visualizationProvider;
    private readonly HashSet<string> partOfSpeechForFiltering =
    [
        "местоимение-прилагательное",
        "союз",
        "междометие",
        "частица",
        "предлог",
        "местоимение-существительное",
    ];

    public VisualizationCloudLayoutTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IReader, TxtReader>();
        services.AddSingleton<IReaderProvider, ReaderPicker>();
        services.AddSingleton<IWordsProvider, TextPreprocessing>();
        services.AddSingleton<IColorProvider, ColorPicker>();
        services.AddSingleton<IUserInputProvider, UserInputProvider>();
        services.AddSingleton<IVisualizationProvider, VisualizationCloudLayout>();
        services.AddSingleton<ISettingsProvider<VisualizationSettingsDto>, VisualizationSettings>();
        var imageSize = new Size(1080, 1080);
        services.AddSingleton<VisualizationSettingsDto>(_ => new VisualizationSettingsDto(
            imageSize,
            new FontFamily("Arial"),
            1f));
        services.AddTransient<ILayoutProvider>(_ => new CircularCloud(new Point(imageSize.Width / 2, imageSize.Height / 2)));

        var provider = services.BuildServiceProvider();
        
        wordsProvider = provider.GetService<IWordsProvider>();
        readerProvider = provider.GetService<IReaderProvider>();
        visualizationProvider = provider.GetService<IVisualizationProvider>();
    }
    
    [Test]
    public void VisualizationCloudLayout_ChangeSettings_SettingsShouldChange()
    {
        const float cloudCompressionRatio = 1.5f;
        var imageSize = new Size(1920, 540);
        var fontFamily = new FontFamily("Calibri");
        
        visualizationProvider.SettingsProvider.Settings.ImageSize = imageSize;
        visualizationProvider.SettingsProvider.Settings.FontFamily = fontFamily;
        visualizationProvider.SettingsProvider.Settings.CloudCompressionRatio = cloudCompressionRatio;
        
        visualizationProvider.SettingsProvider.Settings.ImageSize.Should().Be(imageSize);
        visualizationProvider.SettingsProvider.Settings.FontFamily.Should().Be(fontFamily);
        visualizationProvider.SettingsProvider.Settings.CloudCompressionRatio.Should().Be(cloudCompressionRatio);
    }
    
    [TestCase("Morozko.txt", "Morozko.jpeg",
        ContentStructure.Literary, 2.8f)]
    [TestCase("GeeseAndSwans.txt", "GeeseAndSwans.png",
        ContentStructure.Literary, 3.1f)]
    [TestCase("CheckingCount.txt", "CheckingCount.bmp",
        ContentStructure.ListOfWords, 0.9f)]
    public void CreateImage_ImageSizeMustMatchSettings(string fileName, string imageName,
        ContentStructure structure, float cloudCompressionRatio)
    {
        var sourceFile = Path.Combine("TestsFiles", fileName);
        var words = wordsProvider.PerformPreprocessing(
            readerProvider.GetReader(sourceFile), structure);
        visualizationProvider.UserInputProvider.Words = words
            .Where(info => !partOfSpeechForFiltering.Contains(info.PartOfSpeach));
        visualizationProvider.SettingsProvider.Settings.CloudCompressionRatio = cloudCompressionRatio;
        
        visualizationProvider.SettingsProvider.Settings.ImageSize = new Size(1080, 1080);
        CheckSizeMatching(imageName);
        visualizationProvider.SettingsProvider.Settings.ImageSize = new Size(1280, 720);
        CheckSizeMatching(imageName);
    }

    private void CheckSizeMatching(string imageName)
    {
        var image = GenerateImage(imageName);
        image.Size.Should().Be(visualizationProvider.SettingsProvider.Settings.ImageSize);
    }

    private Bitmap GenerateImage(string imageName)
    {
        imageName = $"({visualizationProvider.SettingsProvider.Settings.ImageSize.Width}" +
                    $"x{visualizationProvider.SettingsProvider.Settings.ImageSize.Height})" +
                    $"{imageName}";
        var pathToImage =  $"../../../Images/{imageName}";

        var image = visualizationProvider.CreateImage();
        image.Save(pathToImage);

        return image;
    }
}