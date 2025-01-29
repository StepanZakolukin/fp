namespace TagCloudGUI.Controls;

public class PartOfSpeechFilter : TagCloudTreeView
{
    public PartOfSpeechFilter(TagCloudConfigurationForm parentForm)
        : base("Исключить части речи:", parentForm)
    {
        TreeView.Margin = new Padding(0, 0, 0, 14);

        parentForm.TextIsUploaded += FillTreeView;
    }

    private void FillTreeView()
    {
        var partsOfSpeech = ParentForm.Words?
            .Select(wordInfo => wordInfo.PartOfSpeach)
            .ToHashSet();

        TreeView.Nodes.Clear();

        foreach (var partOfSpeech in partsOfSpeech)
            TreeView.Nodes.Add(partOfSpeech);
    }
}