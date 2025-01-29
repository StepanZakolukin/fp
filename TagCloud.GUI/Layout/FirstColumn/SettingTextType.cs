using TagCloudGUI.Controls;

namespace TagCloudGUI.Layout.FirstColumn;

public sealed class SettingTextType : DropdownList
{
    public SettingTextType(TagCloudConfigurationForm parentForm) : base(
        "Структура содержания файла",
        parentForm.ParserProvider.GetTypesParsers,
        parentForm)
    {
        Dock = DockStyle.Fill;
        DropDownList.SelectedText = parentForm.ParserProvider.SlectedParser;
        TextHasBeenChanged += TextTypeIsSelected;
        ParentForm.ParserProvider.ValueChanged += (_, message) =>
        {
            ErrorMessage.Text = message;
        };
    }

    private void TextTypeIsSelected(object? sender, EventArgs e)
    {
        var status = ParentForm.ParserProvider.SlectedParser = SelectedText;
    }
}