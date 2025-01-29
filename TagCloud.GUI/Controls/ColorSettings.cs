using TagCloud.ImageGeneration;

namespace TagCloudGUI.Controls;

public class ColorSettings : DropdownList
{
    private readonly Dictionary<string, IColorProvider> coloringAlgorithms = new();

    public ColorSettings(IEnumerable<IColorProvider> coloringAlgorithms,
        TagCloudConfigurationForm parentForm) : base("Алгоритм расцветки слов:",
        coloringAlgorithms.Select(colorPicker => colorPicker.Name), parentForm)
    {
        foreach (var colorPicker in coloringAlgorithms)
            this.coloringAlgorithms[colorPicker.Name] = colorPicker;
        
        DropDownList.SelectedItem = parentForm.ColorPicker?.Name;
        DropDownList.SelectedIndexChanged += ColoringAlgorithmsIsSelected;
    }

    private void ColoringAlgorithmsIsSelected(object? sender, EventArgs e)
    {
        if (sender is ComboBox dropdownList)
            ParentForm.ColorPicker = coloringAlgorithms[dropdownList.SelectedItem.ToString()];
        else throw new ArgumentException($"Не подходящий тип данных, ожидался {nameof(ComboBox)}", nameof(sender));
    }
}