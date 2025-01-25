using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;
using TagCloudGUI.Layout.FirstColumn;

namespace TagCloudGUI.Layout;

public class SettingsTable : TagCloudTableLayoutPanel
{
    public SettingsTable(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm, SettingTextType textType)
    {
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 348));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 523));

        Controls.Add(new FirstColumn.FirstColumn(visualizationProvider, parentForm), 0, 0);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 1, 0);
        Controls.Add(new SecondColumn.SecondColumn(parentForm), 2, 0);
    }
}