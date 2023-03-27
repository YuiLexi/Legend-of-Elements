using System.Collections;
using System.Collections.Generic;

public static class ListExtension
{
    /// <summary>
    /// 检查当前UIPanel列表内是否有给定的UIPanel类型，如果有返回UIPanel；如果没有，返回null
    /// </summary>
    /// <param name="list">当前UIPanel列表</param>
    /// <param name="uIPanelType">定的UIPanel类型</param>
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