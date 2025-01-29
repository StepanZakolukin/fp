using TagCloud.CloudLayout;
using TagCloud.ImageGeneration;
using TagCloud.ReadingFiles;
using TagCloud.TextProcessing;
using TagCloudGUI.Controls;

namespace TagCloudGUI;

public partial class TagCloudConfigurationForm : Form
{
    private bool everythingIsPrepared;
    public IEnumerable<WordInfo>? FilterWords;
    private readonly IVisualizationProvider visualizationProvider;
    public IWordsProvider WordsProvider { get; }
    public IReaderProvider ReaderProvider { get; }

    public TagCloudConfigurationForm(
        IWordsProvider wordsProvider,
        IReaderProvider readerProvider,
        IEnumerable<IColorProvider> colorPickers,
        IVisualizationProvider visualizationProvider,
        IEnumerable<ILayoutProvider> layoutProviders)
    {
        InitializeComponent();
        WordsProvider = wordsProvider;
        ReaderProvider = readerProvider;
        this.visualizationProvider = visualizationProvider;
        Font = new Font("Arial", 22, FontStyle.Regular, GraphicsUnit.Pixel);
        var table = new TableLayoutPanel { Dock = DockStyle.Fill };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 662));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        var textTypeSettings = new SettingTextType(this);
        table.Controls.Add(
            new SettingsTable(visualizationProvider, colorPickers, layoutProviders, this, textTypeSettings)
            {
                Dock = DockStyle.Fill
            }, 0, 0);
        table.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, 1);
        table.Controls.Add(new PushButtonPanel(visualizationProvider, this, textTypeSettings), 0, 2);

        Controls.Add(table);
    }

    public IEnumerable<WordInfo>? Words
    {
        get => visualizationProvider.UserInputProvider.Words;
        set
        {
            visualizationProvider.UserInputProvider.Words = value;
            TextIsUploaded?.Invoke();
            DataHasBeenUpdated?.Invoke(CheckCorrectnessOfData());
        }
    }

    public IColorProvider? ColorPicker
    {
        get => visualizationProvider.UserInputProvider.ColorProvider;
        set
        {
            visualizationProvider.UserInputProvider.ColorProvider = value;
            DataHasBeenUpdated?.Invoke(CheckCorrectnessOfData());
        }
    }

    public ILayoutProvider? LayoutProvider
    {
        get => visualizationProvider.UserInputProvider.LayoutProvider;
        set
        {
            visualizationProvider.UserInputProvider.LayoutProvider = value;
            DataHasBeenUpdated?.Invoke(CheckCorrectnessOfData());
        }
    }

    public bool EverythingIsPrepared
    {
        get => everythingIsPrepared;
        set
        {
            everythingIsPrepared = value;
            if (value) SetupIsFinished?.Invoke();
        }
    }

    public event Action? SetupIsFinished;
    public event Action<bool>? DataHasBeenUpdated;
    public event Action? TextIsUploaded;

    private bool CheckCorrectnessOfData()
    {
        return Words is not null && ColorPicker is not null && LayoutProvider is not null;
    }
}