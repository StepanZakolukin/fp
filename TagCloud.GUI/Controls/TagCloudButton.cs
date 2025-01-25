namespace TagCloudGUI.Controls;

public sealed class TagCloudButton : Button
{
    public TagCloudButton()
    {
        FlatStyle = FlatStyle.Flat;
        Padding = new Padding(0);
        Margin = new Padding(0);
        Height = 40;
        TextAlign = ContentAlignment.TopCenter;
        Font = new Font(Font.FontFamily, 20, FontStyle.Bold, GraphicsUnit.Pixel);
    }
}