namespace TagCloudGUI.Controls;

public sealed class ErrorInformation : Label
{
    public ErrorInformation()
    {
        ForeColor = Color.Red;
        Dock = DockStyle.Fill;
        Margin = new Padding(0);
        Padding = new Padding(0);
        BorderStyle = BorderStyle.Fixed3D;
        Font = new Font(Font.FontFamily, 12, FontStyle.Bold, GraphicsUnit.Pixel);
    }
}