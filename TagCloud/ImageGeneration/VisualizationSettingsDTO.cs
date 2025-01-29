using System.Drawing;

namespace TagCloud.ImageGeneration;

public class VisualizationSettingsDto(Size imageSize, FontFamily fontFamily, float cloudCompressionRatio)
{
    public Size ImageSize { get; set; } = imageSize;
    public FontFamily FontFamily { get; set; } = fontFamily;
    
    private float cloudCompressionRatio = cloudCompressionRatio;
    public float CloudCompressionRatio
    {
        get => cloudCompressionRatio;
        set
        {
            if (value < 0.1 || value > 10.001)
                throw new ArgumentException("Должно быть больше 0.09, но меньше или равно 10", nameof(value));

            cloudCompressionRatio = value;
        }
    }
}