using TagCloud.CloudLayout;

namespace TagCloudGUI.Controls;

public class SettingUpLayoutAlgorithm : DropdownList
{
    private readonly Dictionary<string, ILayoutProvider> layoutProviders = new();

    public SettingUpLayoutAlgorithm(IEnumerable<ILayoutProvider> layoutProviders, TagCloudConfigurationForm parentForm)
        : base("Алгоритм генерации раскладки:", layoutProviders.Select(provider => provider.Name), parentForm)
    {
        foreach (var provider in layoutProviders)
            this.layoutProviders[provider.Name] = provider;
        DropDownList.SelectedItem = parentForm.LayoutProvider?.Name;
        DropDownList.SelectedIndexChanged += LayoutProviderIsSelected;
    }

    private void LayoutProviderIsSelected(object? sender, EventArgs e)
    {
        var dropdownList = sender as ComboBox;
        ParentForm.LayoutProvider = layoutProviders[dropdownList.SelectedItem.ToString()];
    }
}