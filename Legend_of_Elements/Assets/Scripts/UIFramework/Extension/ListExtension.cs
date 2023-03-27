using System.Collections;
using System.Collections.Generic;

public static class ListExtension
{
    /// <summary>
    /// ��鵱ǰUIPanel�б����Ƿ��и�����UIPanel���ͣ�����з���UIPanel�����û�У�����null
    /// </summary>
    /// <param name="list">��ǰUIPanel�б�</param>
    /// <param name="uIPanelType">����UIPanel����</param>
    /// <returns></returns>
    public static UIPanel SearchPanelForType(this List<UIPanel> list, UIPanelType uIPanelType)
    {
        foreach (var item in list)
        {
            if (item._uIPanelType == uIPanelType)
                return item;
        }
        return null;
    }
}