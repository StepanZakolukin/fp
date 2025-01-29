namespace TagCloudGUI.Controls;

public sealed class SecondColumn : TableLayoutPanel
{
    private readonly TagCloudConfigurationForm parentForm;
    private readonly PartOfSpeechFilter partOfSpeechFilter;
    private readonly WordFilter wordFilter;

    public SecondColumn(TagCloudConfigurationForm parentForm)
    {
        Dock = DockStyle.Fill;
        this.parentForm = parentForm;
        wordFilter = new WordFilter(parentForm);
        partOfSpeechFilter = new PartOfSpeechFilter(parentForm);
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        for (var i = 0; i < 2; i++)
            RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        Controls.Add(partOfSpeechFilter, 0, 0);
        Controls.Add(wordFilter, 0, 1);
        parentForm.SetupIsFinished += FilterData;
    }

    private void FilterData()
    {
        var excludedWords = wordFilter.GetSelectedValues().ToHashSet();
        var excludedPartsOfSpeech = partOfSpeechFilter.GetSelectedValues().ToHashSet();
        parentForm.FilterWords = parentForm.Words
            .Where(word => !excludedPartsOfSpeech.Contains(word.PartOfSpeach) && !excludedWords.Contains(word.Word));
    }
}