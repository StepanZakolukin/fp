using System.Drawing;
using TagCloud.TextProcessing;

namespace TagCloud.ImageGeneration;

public interface IColorProvider
{
    public string Name { get; }
    public Color GetColorForWord(WordInfo word);
}