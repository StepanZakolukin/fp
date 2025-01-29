using TagCloud.CloudLayout;
using TagCloud.ImageGeneration;

namespace TagCloudGUI.Controls;

public class FirstColumn : TableLayoutPanel
{
    public FirstColumn(IVisualizationProvider visualizationProvider, IEnumerable<IColorProvider> colorPickers,
        IEnumerable<ILayoutProvider> layoutProviders, TagCloudConfigurationForm parentForm, SettingTextType textType)
    {
        Dock = DockStyle.Fill;
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 162));
        for (var i = 0; i < 4; i++)
            RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Controls.Add(new ImageSizeSettings(visualizationProvider, layoutProviders), 0, 0);
        Controls.Add(new FontSettings(visualizationProvider, parentForm), 0, 1);
        Controls.Add(new ColorSettings(colorPickers, parentForm), 0, 2);
        Controls.Add(new SettingUpLayoutAlgorithm(layoutProviders, parentForm), 0, 3);
        Controls.Add(textType, 0, 4);
        Controls.Add(new ConfiguringCloudCompressionRatio(visualizationProvider), 0, 5);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 6);
    }
}