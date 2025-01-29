using System.Drawing;

namespace TagCloud.CloudLayout;

public interface ILayoutProvider
{
    public string Name { get; }
    public Point Center { get; set; }
    public void ResetLayout();
    public RectangleF PutNextRectangle(SizeF rectangleSize);
}