using System.Drawing;

namespace TagCloud.ImageGeneration;

public interface ILayoutProvider
{
    public string Name { get; }
    public Point Center { get; set; }
    public void ResetLayout();
    public RectangleF PutNextRectangle(SizeF rectangleSize);
}