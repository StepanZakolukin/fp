namespace TagCloud.ImageGeneration.Settings.DTO;

public class LayoutAlgorithmDto : CrrectnessChecker
{
    private readonly Dictionary<string, ILayoutProvider> _layoutAlgorithms;
    public IEnumerable<string> NamesOfLayoutAlgorithms => _layoutAlgorithms.Keys;

    public ILayoutProvider GetValueOrThrow()
    {
        if (IsCorrect)
            return _layoutAlgorithms[_selectedAlgorithm];
        throw new InvalidOperationException();
    }

    private string _selectedAlgorithm;
    public string SelectedAlgorithm {
        get => _selectedAlgorithm;
        set
        {
            _selectedAlgorithm = value;
            if (_selectedAlgorithm is "" or null)
                ChangeValue(false, "Значение не должно быть пустым");
            else if (!_layoutAlgorithms.ContainsKey(value))
                ChangeValue(false, "Алгоритм раскладки не найден");
            else ChangeValue(true);
        }
    }

    public LayoutAlgorithmDto(IEnumerable<ILayoutProvider> layoutAlgorithms, ILayoutProvider defaultAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(layoutAlgorithms);
        _layoutAlgorithms = layoutAlgorithms.ToDictionary(lp => lp.Name, lp => lp);
        ArgumentNullException.ThrowIfNull(defaultAlgorithm);
        if (!_layoutAlgorithms.ContainsKey(defaultAlgorithm.Name))
            throw new ArgumentException($"{nameof(defaultAlgorithm)} должен быть указан в {nameof(layoutAlgorithms)}");
        SelectedAlgorithm = defaultAlgorithm.Name;
    }
}