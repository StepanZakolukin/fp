using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public class ImageSizeSettings : TagCloudTableLayoutPanel
{
    private static readonly Padding ItemsMargin = new(0, 0, 0, 5);

    private readonly TagCloudLabel _heightLabel = new("Высота:");

    private readonly TagCloudTextBox _heightTextBox = new();

    private readonly TagCloudLabel _heightUnitsOfMeasurement = new("px.");

    private readonly IVisualizationProvider _visualizationProvider;

    private readonly TagCloudLabel _widthLabel = new("Ширина:") { Margin = ItemsMargin };

    private readonly TagCloudTextBox _widthTextBox = new();

    private readonly TagCloudLabel _widthUnitsOfMeasurement = new("px.") { Margin = ItemsMargin };

    private readonly ErrorInformation _errorMessage = new();
    public TagCloudLabel Heading { get; } = new("Размеры изображения:");

    public ImageSizeSettings(IVisualizationProvider visualizationProvider)
    {
        Margin = new Padding(0, 0, 0, 8);
        _widthTextBox.Text = visualizationProvider.Settings.ImageSize.Width.ToString();
        _heightTextBox.Text = visualizationProvider.Settings.ImageSize.Height.ToString();
        this._visualizationProvider = visualizationProvider;
        RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        Controls.Add(Heading, 0, 0);
        Controls.Add(CreateNestedTable(), 0, 1);
        Controls.Add(_errorMessage, 0, 2);

        _heightTextBox.TextChanged += HeightHasChanged;
        _widthTextBox.TextChanged += WidthHasChanged;
        visualizationProvider.Settings.ImageSize.ValueChanged += (_, message) =>
        {
            _errorMessage.Text = message;
        };
    }

    private void WidthHasChanged(object? sender, EventArgs args)
    {
        _visualizationProvider.Settings.ImageSize.Width = _widthTextBox.Text;
    }
    
    private void HeightHasChanged(object? sender, EventArgs args)
    {
        _visualizationProvider.Settings.ImageSize.Height = _heightTextBox.Text;
    }

    private TagCloudTableLayoutPanel CreateNestedTable()
    {
        var table = new TagCloudTableLayoutPanel { Margin = new Padding(0, 0, 0, 5)};
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66));

        table.Controls.Add(_widthLabel, 0, 0);
        table.Controls.Add(_widthTextBox, 1, 0);
        table.Controls.Add(_widthUnitsOfMeasurement, 2, 0);

        table.Controls.Add(_heightLabel, 0, 1);
        table.Controls.Add(_heightTextBox, 1, 1);
        table.Controls.Add(_heightUnitsOfMeasurement, 2, 1);

        return table;
    }
}