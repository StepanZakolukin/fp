using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public class FirstColumn : TagCloudTableLayoutPanel
{
    public FirstColumn(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm)
    {
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 133));
        for (var i = 0; i < 4; i++)
            RowStyles.Add(new RowStyle(SizeType.Absolute, 98));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Controls.Add(new ImageSizeSettings(visualizationProvider), 0, 0);
        Controls.Add(new FontSettings(visualizationProvider, parentForm), 0, 1);
        Controls.Add(new ColorSettings(parentForm), 0, 2);
        Controls.Add(new SettingUpLayoutAlgorithm(parentForm), 0, 3);
        Controls.Add(new SettingTextType(parentForm), 0, 4);
        Controls.Add(new ConfiguringCloudCompressionRatio(visualizationProvider), 0, 5);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 6);
    }
}