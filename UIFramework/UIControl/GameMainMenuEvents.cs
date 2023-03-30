using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainMenuEvents : BasePanel
{
    public void OnClickGameSettingButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.GameSettingPanel);
    }
}