namespace TagCloud.ImageGeneration;

public interface ISettingsProvider<out TSettings>
{
    public TSettings Settings { get; }
}