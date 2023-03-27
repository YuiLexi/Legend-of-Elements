using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public void Awake()
    {
        UIManager.Instance.PushPanel(UIPanelType.GameMainMenuPanel);
    }
}