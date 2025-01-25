namespace TagCloud;

public interface ICrrectnessChecker
{
    public bool IsCorrect { get; }
    public event Action<ICrrectnessChecker, string>? ValueChanged;
}