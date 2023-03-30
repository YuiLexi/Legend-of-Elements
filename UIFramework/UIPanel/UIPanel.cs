/// <summary>
/// UIPanel类，该类有两个字段：UIPanel的类型（枚举）、UIPanel路径
/// </summary>
public class UIPanel
{
    public UIPanelType _uIPanelType;//UI模板的类型
    public string _uIPanelPath;//UI模板的路径

    //构造方法
    public UIPanel(UIPanelType uIPanelType, string uIPanelPath)
    {
        this._uIPanelType = uIPanelType;
        this._uIPanelPath = uIPanelPath;
    }

    public UIPanel()
    { }
}