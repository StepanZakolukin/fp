namespace TagCloudGUI.Controls;

public class DropdownList : TableLayoutPanel
{
    private readonly TagCloudLabel heading;
    public event Action<object?, EventArgs> SelectedIndexChanged;
    public object? SelectedItem { get; private set; }
    protected readonly TagCloudConfigurationForm ParentForm;

    public DropdownList(string heading, IEnumerable<string> list, TagCloudConfigurationForm parentForm)
    {
        Dock = DockStyle.Fill;
        ParentForm = parentForm;
        this.heading = new TagCloudLabel(heading);

        DropDownList.Items.AddRange(list.ToArray());

        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        Controls.Add(this.heading, 0, 0);
        Controls.Add(DropDownList, 0, 1);

        DropDownList.SelectedIndexChanged += SelectionHasBeenChanged;
    }

    private void SelectionHasBeenChanged(object? sender, EventArgs args)
    {
        SelectedItem = DropDownList.SelectedItem;
        SelectedIndexChanged?.Invoke(sender, args);
    }

    protected ComboBox DropDownList { get; } = new()
    {
        Margin = new Padding(0, 0, 0, 14),
        Dock = DockStyle.Fill
    };
}