using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIPanelControl : MonoBehaviour
{
    private float _waitTime;
    private float _currentTime;

    private Transform _cardPanel;
    private GameObject _unable;
    private GameObject _chillDown;
    private bool _isCD;
    private int _costNum;

    // Start is called before the first frame update
    private void Start()
    {
        _isCD = true;
        _currentTime = 0;
        _waitTime = 5;
        _costNum = 50;

        _unable = transform.Find("Unable").gameObject;
        _chillDown = transform.Find("CD").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isCD)
            _currentTime += Time.deltaTime;
        UpdataProgess();
    }

    private void UpdataProgess()
    {
        if (!_isCD)
        {
            if (OverAllVariable.SunNum < _costNum)
            {
                _unable.SetActive(true);
            }
            else
            {
                _unable.SetActive(false);
            }
        }
        else
        {
            float per = 1 - Mathf.Clamp(_currentTime / _waitTime, 0, 1);
            _chillDown.GetComponent<Image>().fillAmount = per;
        }
        if (_chillDown.GetComponent<Image>().fillAmount == 0)
        {
            _isCD = false;
            _currentTime = 0;
        }
    }
}