using TagCloud;
using TagCloud.ImageGeneration;
using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout;

public sealed class PushButtonPanel : TagCloudTableLayoutPanel
{
    public static event Action? SetupIsFinished;
    public static event Action? TextIsUploaded;
    
    private readonly TagCloudButton _cloudGenerationButton = new()
    {
        Text = "Сгенерировать",
        Width = 220
    };

    private readonly TagCloudConfigurationForm _parentForm;

    private readonly TagCloudButton _textUploadTagCloudButton = new()
    {
        Text = "Загрузить текст",
        Width = 230
    };

    private readonly IVisualizationProvider _visualizationProvider;

    public PushButtonPanel(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm)
    {
        this._parentForm = parentForm;
        _visualizationProvider = visualizationProvider;
        
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));

        Controls.Add(_textUploadTagCloudButton, 0, 0);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 1, 0);
        Controls.Add(_cloudGenerationButton, 2, 0);
        
        _textUploadTagCloudButton.Click += SelectFile;
        _cloudGenerationButton.Click += GenerateImage;
        _cloudGenerationButton.Enabled = false;
        _textUploadTagCloudButton.Enabled = parentForm.ParserProvider.IsCorrect;
        
        parentForm.ParserProvider.ValueChanged += ContentTypeChanged;
        _visualizationProvider.Settings.ValueChanged += SettingsCorrectnessStatusChanged;
    }

    private void SettingsCorrectnessStatusChanged(ICrrectnessChecker settings, string _)
    {
        _cloudGenerationButton.Enabled = settings.IsCorrect;
    }

    private void ContentTypeChanged(ICrrectnessChecker parserProvider, string __)
    {
        _textUploadTagCloudButton.Enabled = parserProvider.IsCorrect;
    }

    private void SelectFile(object? sender, EventArgs e)
    {
        var extensions = _parentForm.ReaderProvider.GetSupportedExtensions()
            .Select(extension => $"*{extension}")
            .ToArray();
        var openFileDialog = new OpenFileDialog
        {
            Filter = "(" + string.Join(", ", extensions) + ")|" + string.Join(";", extensions),
            RestoreDirectory = true
        };

        if (openFileDialog.ShowDialog() != DialogResult.OK) return;
        var filePath = openFileDialog.FileName;
        var requestResult = _parentForm.ReaderProvider.GetReader(filePath);
        if (!requestResult.IsSuccess)
        {
            SecondColumn.SecondColumn.ErrorMessage.Text = requestResult.Error;
            return;
        }
        var reader = requestResult.GetValueOrThrow();
        var parser = _parentForm.ParserProvider.GetParser();
        var status = parser(reader);
        if (status.IsSuccess)
        {
            _visualizationProvider.Settings.WordsList.Value = status.GetValueOrThrow();
            TextIsUploaded?.Invoke();
        }
        SecondColumn.SecondColumn.ErrorMessage.Text = status.Error;
    }

    private void GenerateImage(object? sender, EventArgs e)
    {
        var filePath = GetPathToSave();
        if (filePath == null) return;
        
        SetupIsFinished?.Invoke();
        
        var status = _visualizationProvider.CreateImage();
        if (status.IsSuccess)
            status.GetValueOrThrow().Save(filePath);
        SecondColumn.SecondColumn.ErrorMessage.Text = status.Error;
    }

    private string? GetPathToSave()
    {
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Изображение (*.png, *.jpeg, *.bmp)|*.png;*.jpeg;*.bmp";
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        saveFileDialog.Title = "Сохранение файла";

        return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : null;
    }
}