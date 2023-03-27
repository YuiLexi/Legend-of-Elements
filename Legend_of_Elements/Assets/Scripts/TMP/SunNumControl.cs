using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SunNumControl : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _textMeshPro.text = Convert.ToString(ParameterUI.SunNumber);
    }
}