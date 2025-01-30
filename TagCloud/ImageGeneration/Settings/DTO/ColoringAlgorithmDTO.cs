namespace TagCloud.ImageGeneration.Settings.DTO;

public class ColoringAlgorithmDto : CrrectnessCheckerBase
{
    private readonly Dictionary<string, IColorProvider> _coloringAlgorithms;
    public IEnumerable<string> NamesOfColoringAlgorithms => _coloringAlgorithms.Keys;
    public IColorProvider? GetColorProvider => IsCorrect ? _coloringAlgorithms[_selectedAlgorithm] : null;
    
    private string _selectedAlgorithm;
    public string SelectedAlgorithm {
        get => _selectedAlgorithm;
        set
        {
            _selectedAlgorithm = value;
            if (_selectedAlgorithm is "" or null)
                ChangeValue(false, "Значение не должно быть пустым");
            else if (!_coloringAlgorithms.ContainsKey(value))
                ChangeValue(false, "Алгоритм расцветки не найден");
            else ChangeValue(true);
        }
    }
    
    public ColoringAlgorithmDto(IEnumerable<IColorProvider> coloringAlgorithms, IColorProvider defaultAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(coloringAlgorithms);
        _coloringAlgorithms = coloringAlgorithms.ToDictionary(cp => cp.Name, cp => cp);
        ArgumentNullException.ThrowIfNull(defaultAlgorithm);
        if (!_coloringAlgorithms.ContainsKey(defaultAlgorithm.Name))
            throw new ArgumentException($"{nameof(defaultAlgorithm)} должен быть указан в {nameof(coloringAlgorithms)}");
        SelectedAlgorithm = defaultAlgorithm.Name;
    }
}