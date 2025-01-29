using System.Drawing.Text;
using TagCloud.ImageGeneration;

namespace TagCloudGUI.Controls;

public class FontSettings : DropdownList
{
    private static readonly IEnumerable<string> FontFamilies = new InstalledFontCollection().Families
        .Select(family => family.Name);

    private readonly IVisualizationProvider visualizationProvider;

    public FontSettings(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm)
        : base("Шрифт:", FontFamilies, parentForm)
    {
        Dock = DockStyle.Fill;
        DropDownList.SelectedItem = visualizationProvider.SettingsProvider.Settings.FontFamily.Name;
        this.visualizationProvider = visualizationProvider;
        DropDownList.SelectedIndexChanged += FontIsSelected;
    }

    private void FontIsSelected(object? sender, EventArgs args)
    {
        var dropdownList = sender as ComboBox;
        visualizationProvider.SettingsProvider.Settings.FontFamily = new FontFamily(dropdownList.SelectedItem.ToString());
    }
}