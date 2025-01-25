using System.Drawing;
using ErrorHandling;
using TagCloud.ImageGeneration.Settings;

namespace TagCloud.ImageGeneration;

public interface IVisualizationProvider
{
    public Result<Bitmap> CreateImage();
    public RenderingSettings Settings { get; }
}