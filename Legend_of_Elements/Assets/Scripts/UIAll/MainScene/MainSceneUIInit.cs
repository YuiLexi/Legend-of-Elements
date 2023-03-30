using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUIInit : MonoBehaviour
{
    public void OnClickReturnButton()
    {
        SceneManager.LoadSceneAsync("Scenes/StarScene", LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }
}