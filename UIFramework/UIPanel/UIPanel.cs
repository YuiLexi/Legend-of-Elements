/// <summary>
/// UIPanel�࣬�����������ֶΣ�UIPanel�����ͣ�ö�٣���UIPanel·��
/// </summary>
public class UIPanel
{
    public UIPanelType _uIPanelType;//UIģ�������
    public string _uIPanelPath;//UIģ���·��

    //���췽��
    public UIPanel(UIPanelType uIPanelType, string uIPanelPath)
    {
        this._uIPanelType = uIPanelType;
        this._uIPanelPath = uIPanelPath;
    }

    public UIPanel()
    { }
}