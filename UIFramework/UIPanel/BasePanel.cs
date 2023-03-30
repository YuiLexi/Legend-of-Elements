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

    //��UIģ��
    public void OnStart()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    //��ͣUIģ��
    public void OnPause()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = false;
    }

    //��������UIģ��
    public void OnResume()
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    //�˳�UIģ��
    public void OnExit()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ��������Ѱ�Ұ�ť
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