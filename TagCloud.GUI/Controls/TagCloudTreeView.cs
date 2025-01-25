namespace TagCloudGUI.Controls;

public class TagCloudTreeView : TagCloudTableLayoutPanel
{
    private readonly TagCloudLabel _heading;

    protected readonly TreeView TreeView = new()
    {
        Dock = DockStyle.Fill,
        Margin = new Padding(0),
        Padding = new Padding(0),
        BorderStyle = BorderStyle.None,
        CheckBoxes = true,
        ShowLines = false
    };

    protected readonly TagCloudConfigurationForm ParentForm;

    public TagCloudTreeView(string heading, TagCloudConfigurationForm parentForm)
    {
        Dock = DockStyle.Fill;
        ParentForm = parentForm;
        Margin = new Padding(0, 0, 0, 8);
        this._heading = new TagCloudLabel(heading);
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 270));

        Controls.Add(this._heading, 0, 0);
        Controls.Add(TreeView, 0, 1);
    }

    public IEnumerable<string> GetSelectedValues()
    {
        for (var i = 0; i < TreeView.Nodes.Count; i++)
            if (TreeView.Nodes[i].Checked)
                yield return TreeView.Nodes[i].Text;
    }
}