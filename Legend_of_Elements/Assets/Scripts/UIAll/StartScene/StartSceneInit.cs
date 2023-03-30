using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartSceneInit : MonoBehaviour
{
    private GameObject _mainPanel;
    private GameObject _isExitPanel;
    private GameObject _loginPanel;

    public void LoginButton()
    {
        _loginPanel.SetActive(false);
        _mainPanel.SetActive(true);
        Destroy(_loginPanel);
    }

    public void StartGameButton()
    {
        SceneManager.LoadSceneAsync("Scenes/MyScene", LoadSceneMode.Single);
    }

    public void OnClickExitButton()
    {
        _isExitPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }

    public void OnclickReturnButton()
    {
        _isExitPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }

    public void OnClickExitGame()
    {
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _mainPanel = transform.Find("MainPanel").gameObject as GameObject;
        _isExitPanel = transform.Find("IsExit").gameObject as GameObject;
        _loginPanel = transform.Find("LoginPanel").gameObject as GameObject;

        _loginPanel.SetActive(true);
        _mainPanel.SetActive(false);
        _isExitPanel.SetActive(false);
    }
}