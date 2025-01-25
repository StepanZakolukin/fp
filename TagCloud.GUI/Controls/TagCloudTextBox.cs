namespace TagCloudGUI.Controls;

public sealed class TagCloudTextBox : TextBox
{
    public TagCloudTextBox()
    {
        Dock = DockStyle.Fill;
        TextAlign = HorizontalAlignment.Center;
        Margin = new Padding(0);
        Padding = new Padding(0);
    }
}