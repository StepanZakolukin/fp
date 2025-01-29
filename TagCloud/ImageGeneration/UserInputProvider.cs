using TagCloud.CloudLayout;
using TagCloud.TextProcessing;

namespace TagCloud.ImageGeneration;

public class UserInputProvider : IUserInputProvider
{
    public UserInputProvider(IColorProvider colorProvider, ILayoutProvider layoutProvider)
    {
        ColorProvider = colorProvider;
        LayoutProvider = layoutProvider;
    }
    
    private IEnumerable<WordInfo>? words;

    public IEnumerable<WordInfo>? Words
    {
        get => words;
        set => words = value ?? throw new ArgumentNullException(nameof(value));
    }

    private IColorProvider colorProvider;
    public IColorProvider ColorProvider
    {
        get => colorProvider;
        set => colorProvider = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    private ILayoutProvider layoutProvider;
    public ILayoutProvider LayoutProvider
    {
        get => layoutProvider;
        set => layoutProvider = value ?? throw new ArgumentNullException(nameof(value));
    }
}