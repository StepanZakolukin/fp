using System.Drawing;
using TagCloud.CloudLayout;
using TagCloud.TextProcessing;

namespace TagCloud.ImageGeneration;

public interface IVisualizationProvider
{
    public ISettingsProvider<VisualizationSettingsDto> SettingsProvider { get; }
    public IUserInputProvider UserInputProvider { get; }
    public Bitmap CreateImage();
}