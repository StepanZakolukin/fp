using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.SecondColumn;

public sealed class SecondColumn : TagCloudTableLayoutPanel
{
    private readonly TagCloudConfigurationForm _parentForm;
    private readonly PartOfSpeechFilter _partOfSpeechFilter;
    private readonly WordFilter _wordFilter;

    public static readonly ErrorInformation ErrorMessage = new(); 
    
    public SecondColumn(TagCloudConfigurationForm parentForm)
    {
        _parentForm = parentForm;
        _wordFilter = new WordFilter(parentForm);
        _partOfSpeechFilter = new PartOfSpeechFilter(parentForm);
        
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 316));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 324));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        Controls.Add(_partOfSpeechFilter, 0, 0);
        Controls.Add(_wordFilter, 0, 1);
        Controls.Add(ErrorMessage, 0, 2);
        
        PushButtonPanel.SetupIsFinished += FilterData;
    }

    private void FilterData()
    {
        var excludedWords = _wordFilter.GetSelectedValues().ToHashSet();
        var excludedPartsOfSpeech = _partOfSpeechFilter.GetSelectedValues().ToHashSet();
        _parentForm.VisualizationProvider.Settings.WordsList.Value = _parentForm.VisualizationProvider.Settings.WordsList.Value?
            .Where(word => !excludedPartsOfSpeech.Contains(word.PartOfSpeach) && !excludedWords.Contains(word.Word));
    }
}