using TagCloud.ImageGeneration;
using TagCloud.ReadingFiles;

namespace TagCloudGUI.Controls;

public sealed class PushButtonPanel : TableLayoutPanel
{
    private static readonly Dictionary<string, ContentStructure> FileContentStructures = new()
    {
        ["Литературный текст"] = ContentStructure.Literary,
        ["Список слов (по одному в строке)"] = ContentStructure.ListOfWords,
    };
    
    private readonly TagCloudButton cloudGenerationButton = new()
    {
        Text = "Сгенерировать",
        Width = 220
    };

    private readonly TagCloudConfigurationForm parentForm;

    private readonly TagCloudButton textUploadTagCloudButton = new()
    {
        Text = "Загрузить текст",
        Width = 230
    };

    private readonly SettingTextType typeContent;
    private readonly IVisualizationProvider visualizationProvider;

    public PushButtonPanel(IVisualizationProvider visualizationProvider, TagCloudConfigurationForm parentForm, SettingTextType typeContent)
    {
        Dock = DockStyle.Fill;
        this.parentForm = parentForm;
        this.typeContent = typeContent;
        this.visualizationProvider = visualizationProvider;
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));

        Controls.Add(textUploadTagCloudButton, 0, 0);
        Controls.Add(new Panel { Dock = DockStyle.Fill }, 1, 0);
        Controls.Add(cloudGenerationButton, 2, 0);
        
        cloudGenerationButton.Enabled = false;
        textUploadTagCloudButton.Click += SelectFile;
        cloudGenerationButton.Click += GenerateImage;
        this.parentForm.DataHasBeenUpdated += correct => cloudGenerationButton.Enabled = correct;
        typeContent.SelectedIndexChanged += ContentTypeHasBeenChanged;
    }

    private void ContentTypeHasBeenChanged(object? sender, EventArgs args)
    {
        if (sender is ComboBox comboBox)
            textUploadTagCloudButton.Enabled = comboBox.SelectedIndex != -1;
        else throw new ArgumentException("sender is not of type ComboBox");
    }

    private void SelectFile(object? sender, EventArgs e)
    {
        var extensions = parentForm.ReaderProvider.GetSupportedExtensions()
            .Select(extension => $"*{extension}")
            .ToArray();
        var openFileDialog = new OpenFileDialog
        {
            Filter = "(" + string.Join(", ", extensions) + ")|" + string.Join(";", extensions),
            RestoreDirectory = true
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            var filePath = openFileDialog.FileName;
            parentForm.Words = parentForm.WordsProvider.PerformPreprocessing(
                parentForm.ReaderProvider.GetReader(filePath),
                FileContentStructures[typeContent.SelectedItem.ToString()]);
        }
    }

    private void GenerateImage(object? sender, EventArgs e)
    {
        var filePath = GetPathToSave();
        if (filePath == null) return;
        
        parentForm.EverythingIsPrepared = true;
        visualizationProvider.UserInputProvider.Words = parentForm.FilterWords;
        
        var image = visualizationProvider.CreateImage();
        image.Save(filePath);
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