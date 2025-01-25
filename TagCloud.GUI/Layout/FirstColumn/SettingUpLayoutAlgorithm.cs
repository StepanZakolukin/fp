using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public class SettingUpLayoutAlgorithm : DropdownList
{
    public SettingUpLayoutAlgorithm(TagCloudConfigurationForm parentForm) : base(
        "Алгоритм генерации раскладки:",
        parentForm.VisualizationProvider.Settings.LayoutAlgorithm.NamesOfLayoutAlgorithms,
        parentForm)
    {
        DropDownList.SelectedItem = parentForm.VisualizationProvider.Settings.LayoutAlgorithm.SelectedAlgorithm;
        TextHasBeenChanged += LayoutProviderIsSelected;
        parentForm.VisualizationProvider.Settings.LayoutAlgorithm.ValueChanged += (_, message) =>
        {
            ErrorMessage.Text = message;
        };
    }

    private void LayoutProviderIsSelected(object? sender, EventArgs e)
    {
        ParentForm.VisualizationProvider.Settings.LayoutAlgorithm.SelectedAlgorithm = SelectedText;
    }
}