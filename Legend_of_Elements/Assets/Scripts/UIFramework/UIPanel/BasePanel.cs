using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Button _closeButton;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    //打开UI模板
    public void OnStart()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    //暂停UI模板
    public void OnPause()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = false;
    }

    //重新启动UI模板
    public void OnResume()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    //退出UI模板
    public void OnExit()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 根据名字寻找按钮
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Button FindButton(string name)
    {
        Button closeButton = null;
        foreach (var item in GetComponentsInChildren<Button>())
        {
            if (item.name == name)
                closeButton = item;
        }
        return closeButton;
    }
}