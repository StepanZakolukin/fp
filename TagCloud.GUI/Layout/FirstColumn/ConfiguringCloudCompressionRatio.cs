using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public class ConfiguringCloudCompressionRatio : TagCloudTableLayoutPanel
{
    private readonly TagCloudTextBox _coefficient = new();

    private readonly TagCloudLabel _heading = new("Коэф. сжатия облака:")
    {
        TextAlign = ContentAlignment.MiddleLeft
    };
    
    private readonly ErrorInformation _errorMessage = new();

    private readonly IVisualizationProvider _visualizationProvider;

    public ConfiguringCloudCompressionRatio(IVisualizationProvider visualizationProvider)
    {
        _visualizationProvider = visualizationProvider;
        _coefficient.Text = $"{Math.Round(visualizationProvider.Settings.CompressionRatio.GetValueOrThrow(), 2)}";
        _coefficient.TextChanged += CoefficientHasChanged;

        RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
        
        Controls.Add(CreateControlGrid(), 0, 0);
        Controls.Add(_errorMessage, 0, 1);

        visualizationProvider.Settings.CompressionRatio.ValueChanged += (_, message) =>
        {
            _errorMessage.Text = message;
        };
    }

    private TagCloudTableLayoutPanel CreateControlGrid()
    {
        var controlGrid = new TagCloudTableLayoutPanel
        {
            Margin = new Padding(0, 0, 0, 5)
        };
        
        controlGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        controlGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        controlGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 87));
        controlGrid.Controls.Add(_heading, 0, 0);
        controlGrid.Controls.Add(_coefficient, 1, 0);

        return controlGrid;
    }

    private void CoefficientHasChanged(object? sender, EventArgs e)
    {
        _visualizationProvider.Settings.CompressionRatio.Value = _coefficient.Text;
    }
}