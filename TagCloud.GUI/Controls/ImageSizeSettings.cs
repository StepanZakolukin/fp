using TagCloud.CloudLayout;
using TagCloud.ImageGeneration;

namespace TagCloudGUI.Controls;

public class ImageSizeSettings : TableLayoutPanel
{
    private static readonly Padding ItemsMargin = new(0, 0, 0, 14);

    private readonly TagCloudLabel heightLabel = new("Высота:");

    private readonly TextBox heightTextBox = new()
    {
        Dock = DockStyle.Fill,
        Margin = ItemsMargin
    };

    private readonly TagCloudLabel heightUnitsOfMeasurement = new("px.");
    private readonly IEnumerable<ILayoutProvider> layoutProviders;

    private readonly IVisualizationProvider visualizationProvider;

    private readonly TagCloudLabel widthLabel = new("Ширина:");

    private readonly TextBox widthTextBox = new()
    {
        Dock = DockStyle.Fill,
        Margin = ItemsMargin
    };

    private readonly TagCloudLabel widthUnitsOfMeasurement = new("px.");

    public ImageSizeSettings(IVisualizationProvider visualizationProvider, IEnumerable<ILayoutProvider> layoutProviders)
    {
        Dock = DockStyle.Fill;
        widthTextBox.Text = visualizationProvider.SettingsProvider.Settings.ImageSize.Width.ToString();
        heightTextBox.Text = visualizationProvider.SettingsProvider.Settings.ImageSize.Height.ToString();
        this.visualizationProvider = visualizationProvider;
        this.layoutProviders = layoutProviders;
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 66.66F));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        Controls.Add(Heading, 0, 0);
        Controls.Add(CreateNestedTable(), 0, 1);

        heightTextBox.TextChanged += ProcessImageSizeChange;
        widthTextBox.TextChanged += ProcessImageSizeChange;
    }

    public TagCloudLabel Heading { get; } = new("Размеры изображения:");

    private void ProcessImageSizeChange(object? sender, EventArgs args)
    {
        if (int.TryParse(heightTextBox.Text, out var height) && height > 0 &&
            int.TryParse(widthTextBox.Text, out var width) && width > 0)
        {
            visualizationProvider.SettingsProvider.Settings.ImageSize = new Size(width, height);
            heightTextBox.BackColor = Color.White;
            widthTextBox.BackColor = Color.White;
        }
        else
        {
            heightTextBox.BackColor = Color.Red;
            widthTextBox.BackColor = Color.Red;
        }
    }

    private TableLayoutPanel CreateNestedTable()
    {
        var table = new TableLayoutPanel { Dock = DockStyle.Fill };
        for (var i = 0; i < 2; i++)
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66));

        table.Controls.Add(widthLabel, 0, 0);
        table.Controls.Add(widthTextBox, 1, 0);
        table.Controls.Add(widthUnitsOfMeasurement, 2, 0);

        table.Controls.Add(heightLabel, 0, 1);
        table.Controls.Add(heightTextBox, 1, 1);
        table.Controls.Add(heightUnitsOfMeasurement, 2, 1);

        return table;
    }
}