using Microsoft.Extensions.DependencyInjection;
using TagCloud.ImageGeneration;
using TagCloud.ImageGeneration.Settings;
using TagCloud.ImageGeneration.Settings.DTO;
using TagCloud.Parsing;
using TagCloud.ReadingFiles;

namespace TagCloudGUI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();
        services.AddSingleton<LayoutAlgorithmDto>();
        services.AddSingleton<IReader, TxtReader>();
        services.AddSingleton<ColoringAlgorithmDto>();
        services.AddSingleton<IParser, WordListParser>();
        services.AddSingleton<IParser, LiteraryTextParser>();
        services.AddSingleton<IColorProvider, ColorPicker>();
        services.AddSingleton<IParserProvider, ParserProvider>();
        services.AddSingleton<IReaderProvider, ReaderPicker>();
        services.AddTransient<ILayoutProvider, CircularCloud>();
        services.AddSingleton<Form, TagCloudConfigurationForm>();
        services.AddSingleton<IVisualizationProvider, VisualizationCloudLayout>();

        var partialSupplier = services.BuildServiceProvider();
        services.AddSingleton<RenderingSettings>(_ => new RenderingSettings(
            new ImageSizeDto(1080, 1080),
            new FontFamilyDto("Arial"),
            new CompressionRatioDto(2f),
            partialSupplier.GetService<LayoutAlgorithmDto>(),
            partialSupplier.GetService<ColoringAlgorithmDto>(),
            new WordsListDto()));

        var provider = services.BuildServiceProvider();
        var form = provider.GetService<Form>();
        Application.Run(form);
    }
}
