namespace TagCloudGUI.Controls;

public class DropdownList : TagCloudTableLayoutPanel
{
    private readonly TagCloudLabel _heading;
    public event Action<object?, EventArgs>? TextHasBeenChanged;
    public string SelectedText { get; private set; }
    protected readonly TagCloudConfigurationForm ParentForm;
    public readonly ErrorInformation ErrorMessage = new();

    public DropdownList(string heading, IEnumerable<string> list, TagCloudConfigurationForm parentForm)
    {
        ParentForm = parentForm;
        _heading = new TagCloudLabel(heading);
        Margin = new Padding(0, 0, 0, 8);

        DropDownList.Items.AddRange(list.ToArray());

        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
        
        Controls.Add(_heading, 0, 0);
        Controls.Add(DropDownList, 0, 1);
        Controls.Add(ErrorMessage, 0, 2);

        DropDownList.TextChanged += SelectionHasBeenChanged;
    }

    private void SelectionHasBeenChanged(object? sender, EventArgs args)
    {
        SelectedText = DropDownList.Text;
        TextHasBeenChanged?.Invoke(sender, args);
    }

    protected ComboBox DropDownList { get; } = new()
    {
        Dock = DockStyle.Fill,
        Margin = new Padding(0, 0, 0, 5),
        Padding = new Padding(0),
    };
}