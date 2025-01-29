using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.SecondColumn;

public class WordFilter : TagCloudTreeView
{
    public WordFilter(TagCloudConfigurationForm parentForm)
        : base("Исключить слова:", parentForm)
    {
        PushButtonPanel.TextIsUploaded += FillTreeView;
    }

    private void FillTreeView()
    {
        var words = ParentForm.VisualizationProvider.Settings.WordsList.Value
            .Select(wordInfo => wordInfo.Word);
        TreeView.Nodes.Clear();
        foreach (var word in words)
            TreeView.Nodes.Add(word);
    }
}