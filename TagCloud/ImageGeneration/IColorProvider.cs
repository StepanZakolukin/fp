using System.Drawing;
using TagCloud.Parsing;

namespace TagCloud.ImageGeneration;

public interface IColorProvider
{
    public string Name { get; }
    public Color GetColorForWord(WordInfo word);
}