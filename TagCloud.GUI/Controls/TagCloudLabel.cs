namespace TagCloudGUI.Controls;

public sealed class TagCloudLabel : Label
{
    public TagCloudLabel(string text)
    {
        Text = text;
        Dock = DockStyle.Fill;
        Margin = new Padding(0, 0, 0, 14);
    }
}