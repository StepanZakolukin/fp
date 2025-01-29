using System.Collections.Immutable;
using TagCloud.ImageGeneration.Settings.DTO;

namespace TagCloud.ImageGeneration.Settings;

public class RenderingSettings : ICrrectnessChecker
{
    public ImageSizeDto ImageSize { get; }
    public FontFamilyDto FontFamily { get; }
    public WordsListDto WordsList { get; }
    public CompressionRatioDto CompressionRatio { get; }
    public LayoutAlgorithmDto LayoutAlgorithm { get; }
    public ColoringAlgorithmDto ColoringAlgorithm { get; }
    
    private readonly ImmutableArray<ICrrectnessChecker> _arrayFields;
    
    public RenderingSettings(ImageSizeDto imageSize, FontFamilyDto fontFamily, CompressionRatioDto compressionRatio,
        LayoutAlgorithmDto layoutAlgorithm, ColoringAlgorithmDto coloringAlgorithm, WordsListDto wordsList)
    {
        ImageSize = imageSize ?? throw new ArgumentNullException(nameof(imageSize));
        FontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
        CompressionRatio = compressionRatio ?? throw new ArgumentNullException(nameof(compressionRatio));
        LayoutAlgorithm = layoutAlgorithm ?? throw new ArgumentNullException(nameof(layoutAlgorithm));
        ColoringAlgorithm = coloringAlgorithm ?? throw new ArgumentNullException(nameof(coloringAlgorithm));
        WordsList = wordsList ?? throw new ArgumentNullException(nameof(wordsList));
        _arrayFields = ImmutableArray.CreateRange<ICrrectnessChecker>([
            ImageSize,
            FontFamily,
            CompressionRatio,
            LayoutAlgorithm,
            ColoringAlgorithm,
            WordsList ]);
        foreach (var field in _arrayFields)
            field.ValueChanged += FieldValueHasChanged;
    }

    private void FieldValueHasChanged(ICrrectnessChecker _, string message)
    {
        if (IsCorrect == _arrayFields.All(checker => checker.IsCorrect)) return;
        IsCorrect = !IsCorrect;
        ValueChanged?.Invoke(this, string.Empty);
    }
    
    public event Action<ICrrectnessChecker, string>? ValueChanged;
    public bool IsCorrect { get; private set; }
}