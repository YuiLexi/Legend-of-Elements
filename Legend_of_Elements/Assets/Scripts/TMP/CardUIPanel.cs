using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUIPanel : MonoBehaviour
{
    private GameObject _dark;
    private GameObject _cD_ing;

    [SerializeField] private float _waitTime;
    [SerializeField] private int _sunNum;
    private float _timer;
    private bool _isTimer = true;
    private bool _isEnable = false;

    [SerializeField] private GameObject _cardPrefab;
    private GameObject _currentCard;

    /// <summary>
    /// 转换坐标系，将屏幕坐标转化为世界坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3 TranslateSceneToWorld(Vector3 position)
    {
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraPosition.x, cameraPosition.y, 0);
    }

    public void OnBeginDrag(BaseEventData data)
    {
        if (ParameterUI.SunNumber >= _sunNum && _isEnable)
        {
            _timer = 0;
            _isTimer = false;
            PointerEventData pointerEventData = data as PointerEventData;
            _currentCard = Instantiate(_cardPrefab);
            _currentCard.transform.position = TranslateSceneToWorld(pointerEventData.position);
        }
    }

    public void OnDrag(BaseEventData data)
    {
        if (ParameterUI.SunNumber >= _sunNum && _isEnable)
        {
            _timer = 0;
            _isTimer = false;
            if (_currentCard == null)
                return;
            PointerEventData pointerEventData = data as PointerEventData;
            _currentCard.transform.position = TranslateSceneToWorld(pointerEventData.position);
        }
    }

    public void OnEndDrag(BaseEventData data)
    {
        if (ParameterUI.SunNumber >= _sunNum && _isEnable)
        {
            _isTimer = true;
            if (_currentCard == null)
                return;
            PointerEventData pointerEventData = data as PointerEventData;
            Collider2D[] collider2Ds = Physics2D.OverlapPointAll(TranslateSceneToWorld(pointerEventData.position));//获取鼠标所在位置的所有碰撞体

            //
            foreach (var collider in collider2Ds)
            {
                if (collider.tag == "CardCell" && collider.transform.childCount == 0)
                {
                    _currentCard.transform.SetParent(collider.transform, false);
                    _currentCard.transform.localPosition = new Vector3(0, 0, 0);

                    ParameterUI.SunNumber -= _sunNum;
                    _isEnable = false;

                    _currentCard = null;
                    break;
                }
            }

            if (_currentCard != null)
            {
                GameObject.Destroy(_currentCard);
                _currentCard = null;
                _timer = _waitTime;
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _timer = 0;
        _dark = transform.Find("Dark").gameObject;
        _cD_ing = transform.Find("CD_ing").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isTimer)
            _timer += Time.deltaTime;
        UpdataCD();
        UpdataDark();
    }

    private void UpdataCD()
    {
        float per = Mathf.Clamp(_timer / _waitTime, 0.0f, 1.0f);
        _cD_ing.GetComponent<Image>().fillAmount = 1 - per;
        if (_cD_ing.GetComponent<Image>().fillAmount <= 0.0f)
            _isEnable = true;
    }

    private void UpdataDark()
    {
        if (_cD_ing.GetComponent<Image>().fillAmount <= 0.0f && ParameterUI.SunNumber >= _sunNum)
        {
            _dark.SetActive(false);
        }
        else
        {
            _dark.SetActive(true);
        }
    }
}