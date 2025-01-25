using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.SecondColumn;

public class PartOfSpeechFilter : TagCloudTreeView
{
    public PartOfSpeechFilter(TagCloudConfigurationForm parentForm)
        : base("Исключить части речи:", parentForm)
    {
        Margin = new Padding(0, 0, 0, 8);
        PushButtonPanel.TextIsUploaded += FillTreeView;
    }

    private void FillTreeView()
    {
        var partsOfSpeech = ParentForm.VisualizationProvider.Settings.WordsList.Value
            .Select(wordInfo => wordInfo.PartOfSpeach)
            .ToHashSet();
        TreeView.Nodes.Clear();
        foreach (var partOfSpeech in partsOfSpeech)
            TreeView.Nodes.Add(partOfSpeech);
    }
}