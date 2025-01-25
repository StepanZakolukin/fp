namespace TagCloud.ImageGeneration.Settings.DTO;

public class CompressionRatioDto : ICrrectnessChecker
{
    public const float MaxValue = 10f;
    public const float MinValue = 0.1f;
    public bool IsCorrect { get; private set; }
    public event Action<ICrrectnessChecker, string>? ValueChanged;
    
    private string _value;
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            if (float.TryParse(value, out var number))
            {
                IsCorrect = CheckCorrectness(number);
                ValueChanged?.Invoke(this, IsCorrect ? string.Empty : $"{MinValue} <= коэффициент <= {MaxValue}");
            }
            else
            {
                IsCorrect = false;
                ValueChanged?.Invoke(this, "Не является числом");
            }
        }
    }

    public CompressionRatioDto(float ratio)
    {
        if (!CheckCorrectness(ratio))
            throw new ArgumentException($"{MinValue} <= {nameof(ratio)} <= {MaxValue}");
        Value = $"{ratio}";
    }

    private bool CheckCorrectness(float coefficient)
    {
        return coefficient is >= MinValue - float.Epsilon and <= MaxValue + float.Epsilon;
    }

    public float GetValueOrThrow()
    {
        if (float.TryParse(_value, out var number) && CheckCorrectness(number))
            return number;
        throw new InvalidOperationException();
    }
}