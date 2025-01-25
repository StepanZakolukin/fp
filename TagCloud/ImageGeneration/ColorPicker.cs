using System.Drawing;
using TagCloud.Parsing;

namespace TagCloud.ImageGeneration;

public class ColorPicker : IColorProvider
{
    public string Name => "Однотонный красный";

    public Color GetColorForWord(WordInfo word)
    {
        return Color.Red;
    }
}