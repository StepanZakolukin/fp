using System.Drawing;
using ErrorHandling;
using TagCloud.ImageGeneration.Settings;

namespace TagCloud.ImageGeneration;

public class VisualizationCloudLayout(
    RenderingSettings settings)
    : IVisualizationProvider
{
    private float _coefficient;
    public RenderingSettings Settings { get; } = settings;

    public Result<Bitmap> CreateImage()
    {
        if (!Settings.WordsList.IsCorrect)
            return Result.Fail<Bitmap>("Загрузите слова для генерации изображения");
        if (!Settings.IsCorrect)
            return Result.Fail<Bitmap>("Значения настроек некорректны");
        
        var imageSize = Settings.ImageSize.GetValueOrThrow();
        Settings.LayoutAlgorithm.GetValueOrThrow().ResetLayout();
        var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
        Settings.LayoutAlgorithm.GetValueOrThrow().Center = center;
        var numberOUniqueWords = Settings.WordsList.Value.Sum(wordInfo => wordInfo.NumberInText);
        _coefficient = imageSize.Height * Settings.CompressionRatio.GetValueOrThrow() / numberOUniqueWords;
        var image = new Bitmap(imageSize.Width, imageSize.Height);
        var status = DrawСloudOfWords(Graphics.FromImage(image));

        return status.IsSuccess ? Result.Ok(image) : Result.Fail<Bitmap>(status.Error);
    }

    private ActionStatus DrawСloudOfWords(Graphics graphics)
    {
        var rectOfCanvas = new RectangleF(Point.Empty, Settings.ImageSize.GetValueOrThrow());
        
        foreach (var word in Settings.WordsList.Value)
        {
            var color = Settings.ColoringAlgorithm.GetColorProvider.GetColorForWord(word);
            var height = word.NumberInText * _coefficient;
            var font = new Font(Settings.FontFamily.GetValueOrThrow(), height, GraphicsUnit.Pixel);
            var size = graphics.MeasureString(word.Word, font);
            var rectOfWord = Settings.LayoutAlgorithm.GetValueOrThrow().PutNextRectangle(size);
            if (!rectOfCanvas.Contains(rectOfWord))
                return ActionStatus.Fail("Не удалось уместить все слова на холст");

            graphics.DrawString(word.Word, font, new SolidBrush(color), rectOfWord);
        }
        
        return ActionStatus.Ok();
    }
}