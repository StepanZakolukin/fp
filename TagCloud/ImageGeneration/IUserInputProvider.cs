using TagCloud.CloudLayout;
using TagCloud.TextProcessing;

namespace TagCloud.ImageGeneration;

public interface IUserInputProvider
{
    public IColorProvider ColorProvider { get; set; }
    public IEnumerable<WordInfo>? Words { get; set; }
    public ILayoutProvider LayoutProvider { get; set; }
}