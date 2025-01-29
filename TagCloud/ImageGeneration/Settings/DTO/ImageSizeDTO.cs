using System.Drawing;

namespace TagCloud.ImageGeneration.Settings.DTO;

public class ImageSizeDto : ICrrectnessChecker
{
    public bool IsCorrect => _widthIsCorrect && _heightIsCorrect;
    public event Action<ICrrectnessChecker, string>? ValueChanged;
    private const string ErrorMessage = "Размеры должны быть положительными";

    public Size GetValueOrThrow()
    {
        if (IsCorrect)
            return new Size(int.Parse(_width), int.Parse(_height));
        throw new InvalidOperationException();
    } 

    private bool _widthIsCorrect;
    private bool _heightIsCorrect;

    private string _width;
    public string Width
    {
        get => _width;
        set
        {
            _width = value;
            if (int.TryParse(value, out var number))
            {
                _widthIsCorrect = number > 0;
                ValueChanged?.Invoke(this, _widthIsCorrect ? string.Empty : ErrorMessage);
            }
            else
            {
                _widthIsCorrect = false;
                ValueChanged?.Invoke(this, "Оба параметра должны быть целыми числами");
            }
        }
    }
    
    private string _height;
    public string Height
    {
        get => _height;
        set
        {
            _height = value;
            if (int.TryParse(value, out var number))
            {
                _heightIsCorrect = number > 0;
                ValueChanged?.Invoke(this, _heightIsCorrect ? string.Empty : ErrorMessage);
            }
            else
            {
                _heightIsCorrect = false;
                ValueChanged?.Invoke(this, "Оба параметра должны быть целыми числами");
            }
        }
    }

    public ImageSizeDto(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentException("Ширина должна быть положительной", nameof(width));
        if (height <= 0)
            throw new ArgumentException("Высота должна быть положительной", nameof(height));
        
        Width = width.ToString();
        Height = height.ToString();
    }
}