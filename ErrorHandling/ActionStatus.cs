namespace ErrorHandling;

public readonly struct ActionStatus(string error)
{
    public string Error { get; } = error;
    public bool IsSuccess => Error == null;
    
    public static ActionStatus Ok()
    {
        return new ActionStatus(null);
    }

    public static ActionStatus Fail(string e)
    {
        return new ActionStatus(e);
    }
}