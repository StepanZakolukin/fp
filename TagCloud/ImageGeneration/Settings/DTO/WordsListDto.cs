using TagCloud.TextProcessing;

namespace TagCloud.ImageGeneration.Settings.DTO;

public class WordsListDto : ICrrectnessChecker
{
    public bool IsCorrect { get; private set; }
    public event Action<ICrrectnessChecker, string>? ValueChanged;

    private IEnumerable<WordInfo>? _value;
    public IEnumerable<WordInfo>? Value
    {
        get => _value;
        set
        {
            _value = value;
            IsCorrect = value is not null && value.Any();
            ValueChanged?.Invoke(this, IsCorrect ? string.Empty : "Загрузите слова");
        }
    }
}