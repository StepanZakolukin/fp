using Microsoft.Extensions.DependencyInjection;
using TagCloud.CloudLayout;
using TagCloud.ImageGeneration;
using TagCloud.ReadingFiles;
using TagCloud.TextProcessing;

namespace TagCloudGUI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();
        services.AddSingleton<IReader, TxtReader>();
        services.AddSingleton<IReaderProvider, ReaderPicker>();
        services.AddSingleton<IColorProvider, ColorPicker>();
        services.AddSingleton<Form, TagCloudConfigurationForm>();
        services.AddSingleton<IWordsProvider, TextPreprocessing>();
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
        var form = provider.GetService<Form>();
        Application.Run(form);
    }
}