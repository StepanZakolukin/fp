namespace TagCloudGUI.Controls;

public class WordFilter : TagCloudTreeView
{
    public WordFilter(TagCloudConfigurationForm parentForm)
        : base("Исключить слова:", parentForm)
    {
        parentForm.TextIsUploaded += FillTreeView;
    }

    private void FillTreeView()
    {
        var words = ParentForm.Words.Select(wordInfo => wordInfo.Word);
        TreeView.Nodes.Clear();
        foreach (var word in words)
            TreeView.Nodes.Add(word);
    }
}