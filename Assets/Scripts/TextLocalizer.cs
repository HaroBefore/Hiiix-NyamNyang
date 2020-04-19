using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    private Text _text;
    public Text Text
    {
        get
        {
            if (_text == null)
            {
                _text = GetComponent<Text>();
            }

            return _text;
        }
    }
#if UNITY_EDITOR
    [OnValueChanged(nameof(OnStringIndexChanged))]
#endif
    [SerializeField]
    private int stringIndex;

    public int StringIndex => stringIndex;

#if UNITY_EDITOR
    private void OnStringIndexChanged()
    {
        strText = StringDataObject.GetStringData(stringIndex);
    }

    [HideLabel] [Multiline] [ReadOnly] [SerializeField]
    private string strText;
#endif
    
    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        ReloadText();
    }

    public void ReloadText()
    {
        string str = StringDataObject.GetStringData(stringIndex);
        if (!string.IsNullOrEmpty(str) && str != "NULL")
        {
            Text.text = str;
        }
    }

    public void SetStringIndex(int index)
    {
        stringIndex = index;
    }
    
#if UNITY_EDITOR
    public void ReloadStrText()
    {
        strText = StringDataObject.GetStringData(stringIndex);
    }
#endif
}