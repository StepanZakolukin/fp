using TagCloud.ImageGeneration;
using TagCloud.Parsing;
using TagCloud.ReadingFiles;
using TagCloudGUI.Controls;
using TagCloudGUI.Layout;
using TagCloudGUI.Layout.FirstColumn;

namespace TagCloudGUI;

public partial class TagCloudConfigurationForm : Form
{
    public IParserProvider ParserProvider { get; }
    public IReaderProvider ReaderProvider { get; }
    public IVisualizationProvider VisualizationProvider { get; }

    public TagCloudConfigurationForm(
        IParserProvider parserProvider,
        IReaderProvider readerProvider,
        IVisualizationProvider visualizationProvider)
    {
        InitializeComponent();
        ParserProvider = parserProvider;
        ReaderProvider = readerProvider;
        VisualizationProvider = visualizationProvider;
        Font = new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel);
        var table = new TagCloudTableLayoutPanel();
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 676));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 14));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        var textTypeSettings = new SettingTextType(this);
        table.Controls.Add(
            new SettingsTable(visualizationProvider, this, textTypeSettings)
            {
                Dock = DockStyle.Fill
            }, 0, 0);
        table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 1);
        table.Controls.Add(new PushButtonPanel(visualizationProvider, this), 0, 2);

        Controls.Add(table);
    }
}