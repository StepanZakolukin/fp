namespace TagCloudGUI.Controls;

public class SettingTextType : DropdownList
{
    public SettingTextType(TagCloudConfigurationForm parentForm) 
        : base("Структура содержания файла", ["Литературный текст", "Список слов (по одному в строке)"], parentForm)
    {
        Dock = DockStyle.Fill;
        DropDownList.SelectedIndex = 0;
    }
}