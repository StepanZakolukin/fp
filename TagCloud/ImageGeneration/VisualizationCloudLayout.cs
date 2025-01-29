using System.Drawing;

namespace TagCloud.ImageGeneration;

public class VisualizationCloudLayout(
    ISettingsProvider<VisualizationSettingsDto> settingsProvider,
    IUserInputProvider userInputProvider)
    : IVisualizationProvider
{
    private float coefficient;

    public ISettingsProvider<VisualizationSettingsDto> SettingsProvider { get; } = settingsProvider;

    public IUserInputProvider UserInputProvider { get; } = userInputProvider;

    public Bitmap CreateImage()
    {
        UserInputProvider.LayoutProvider.ResetLayout();
        var center = new Point(SettingsProvider.Settings.ImageSize.Width / 2, SettingsProvider.Settings.ImageSize.Height / 2);
        UserInputProvider.LayoutProvider.Center = center;
        var numberOUniqueWords = UserInputProvider.Words.Sum(wordInfo => wordInfo.NumberInText);
        coefficient = SettingsProvider.Settings.ImageSize.Height * SettingsProvider.Settings.CloudCompressionRatio / numberOUniqueWords;
        var image = new Bitmap(SettingsProvider.Settings.ImageSize.Width, SettingsProvider.Settings.ImageSize.Height);
        DrawСloudOfWords(Graphics.FromImage(image));

        return image;
    }

    private void DrawСloudOfWords(Graphics graphics)
    {
        foreach (var word in UserInputProvider.Words)
        {
            var color = UserInputProvider.ColorProvider.GetColorForWord(word);
            var height = word.NumberInText * coefficient;
            var font = new Font(SettingsProvider.Settings.FontFamily, height, GraphicsUnit.Pixel);
            var size = graphics.MeasureString(word.Word, font);
            var location = UserInputProvider.LayoutProvider.PutNextRectangle(size);

            graphics.DrawString(word.Word, font, new SolidBrush(color), location);
        }
    }
}