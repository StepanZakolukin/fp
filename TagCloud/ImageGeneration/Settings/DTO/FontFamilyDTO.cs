using System.Drawing;
using System.Drawing.Text;

namespace TagCloud.ImageGeneration.Settings.DTO;

public class FontFamilyDto : CrrectnessChecker
{
    private readonly HashSet<string> _availableFontFamilies = new InstalledFontCollection().Families
        .Select(family => family.Name)
        .ToHashSet();

    private string _name;

    public FontFamily GetValueOrThrow()
    {
        if (IsCorrect)
            return new FontFamily(_name);
        throw new InvalidOperationException();
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            if (value is "" or null)
                ChangeValue(false, "Значение не должно быть пустым");
            else if (!_availableFontFamilies.Contains(value))
                ChangeValue(false, $"Шрифт {value} не найден");
            else  ChangeValue(true);
        }
    }

    public FontFamilyDto(string fontName)
    {
        if (fontName is "" or null)
            throw new ArgumentException("Значение не должно быть пустым", nameof(fontName));
        if (!_availableFontFamilies.Contains(fontName))
            throw new ArgumentException($"Шрифт {fontName} не найден в списке системных шрифтов");
        
        Name = fontName;
    }
}