using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingEvents : BasePanel
{
    public void OnClickCloseButton()
    {
        UIManager.Instance.PopPanel();
    }
}