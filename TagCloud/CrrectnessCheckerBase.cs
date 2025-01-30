namespace TagCloud;

public abstract class CrrectnessCheckerBase : ICrrectnessChecker
{
    public bool IsCorrect { get; private set; }
    public event Action<ICrrectnessChecker, string>? ValueChanged;
    
    protected void ChangeValue(bool isCorrect, string message = "")
    {
        IsCorrect = isCorrect;
        ValueChanged?.Invoke(this, message);
    }
}