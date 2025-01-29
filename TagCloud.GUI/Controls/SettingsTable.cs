using TagCloud.CloudLayout;
using TagCloud.ImageGeneration;

namespace TagCloudGUI.Controls;

public class SettingsTable : TableLayoutPanel
{
    public SettingsTable(IVisualizationProvider visualizationProvider, IEnumerable<IColorProvider> colorPickers,
        IEnumerable<ILayoutProvider> layoutProviders, TagCloudConfigurationForm parentForm, SettingTextType textType)
    {
        Dock = DockStyle.Fill;
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 348));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 523));

        Controls.Add(new FirstColumn(visualizationProvider, colorPickers, layoutProviders, parentForm, textType), 0, 0);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 1, 0);
        Controls.Add(new SecondColumn(parentForm), 2, 0);
    }
}