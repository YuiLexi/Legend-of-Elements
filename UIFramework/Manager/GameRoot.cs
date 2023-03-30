using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public void Start()
    {
        UIManager.Instance.PushPanel(UIPanelType.GameMainMenuPanel);
    }
}