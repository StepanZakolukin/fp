using System.Drawing.Text;
using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public sealed class FontSettings : DropdownList
{
    private static readonly IEnumerable<string> FontFamilies = new InstalledFontCollection().Families
        .Select(family => family.Name);

    private readonly IVisualizationProvider _visualizationProvider;

    public FontSettings(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm)
        : base("Шрифт:", FontFamilies, parentForm)
    {
        Dock = DockStyle.Fill;
        DropDownList.SelectedItem = visualizationProvider.Settings.FontFamily.Name;
        _visualizationProvider = visualizationProvider;
        TextHasBeenChanged += FontIsSelected;
        visualizationProvider.Settings.FontFamily.ValueChanged += (_, message) =>
        {
            ErrorMessage.Text = message;
        };
    }

    private void FontIsSelected(object? sender, EventArgs args)
    {
        _visualizationProvider.Settings.FontFamily.Name = SelectedText;
    }
}