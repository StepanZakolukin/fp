using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public class ColorSettings : DropdownList
{
    public ColorSettings(TagCloudConfigurationForm parentForm) : base(
        "Алгоритм расцветки слов:",
        parentForm.VisualizationProvider.Settings.ColoringAlgorithm.NamesOfColoringAlgorithms,
        parentForm)
    {
        DropDownList.SelectedItem = parentForm.VisualizationProvider.Settings.ColoringAlgorithm.SelectedAlgorithm;
        TextHasBeenChanged += ColoringAlgorithmsIsSelected;
        parentForm.VisualizationProvider.Settings.ColoringAlgorithm.ValueChanged += (_, message) =>
        {
            ErrorMessage.Text = message;
        };
    }

    private void ColoringAlgorithmsIsSelected(object? sender, EventArgs e)
    {
        ParentForm.VisualizationProvider.Settings.ColoringAlgorithm.SelectedAlgorithm = SelectedText;
    }
}