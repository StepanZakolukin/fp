namespace TagCloudGUI.Controls;

public class TagCloudTreeView : TableLayoutPanel
{
    private readonly TagCloudLabel heading;

    protected readonly TreeView TreeView = new()
    {
        Dock = DockStyle.Fill,
        BorderStyle = BorderStyle.None,
        CheckBoxes = true,
        ShowLines = false
    };

    protected TagCloudConfigurationForm ParentForm;

    public TagCloudTreeView(string heading, TagCloudConfigurationForm parentForm)
    {
        ParentForm = parentForm;
        Dock = DockStyle.Fill;
        this.heading = new TagCloudLabel(heading);
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 270));

        Controls.Add(this.heading, 0, 0);
        Controls.Add(TreeView, 0, 1);
    }

    public IEnumerable<string> GetSelectedValues()
    {
        for (var i = 0; i < TreeView.Nodes.Count; i++)
            if (TreeView.Nodes[i].Checked)
                yield return TreeView.Nodes[i].Text;
    }
}