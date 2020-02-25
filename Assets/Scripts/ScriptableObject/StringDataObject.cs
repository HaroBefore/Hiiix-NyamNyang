using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum LanguageType
{
    None = -1,
    Korean,
    English,
    Japanese,
    ChineseGan,
    ChineseBun,
    French,
    End
}

[CreateAssetMenu(fileName = nameof(StringDataObject), menuName = nameof(StringDataObject))]
public class StringDataObject : ScriptableObject {
    private static StringDataObject _instance;

    public static StringDataObject Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<StringDataObject>(nameof(StringDataObject));
            }
            return _instance;
        }
    }
    
    [LabelText("언어")]
    public LanguageType languageType = LanguageType.None;

    [LabelText("출력해 볼 인덱스")]
    [PropertyOrder(0)]
    public int index;

    [PropertyOrder(1)]
    [LabelText("인덱스 출력")]
    [Button]
    public void PrintIndex()
    {
        Debug.Log(StringDataObject.GetStringData(index));
    }

    [Space]
    [PropertyOrder(2)]
    public List<StringDataMap> korean = new List<StringDataMap>();
    [PropertyOrder(2)]
    public List<StringDataMap> english = new List<StringDataMap>();
    [PropertyOrder(2)]
    public List<StringDataMap> japanese = new List<StringDataMap>();
    [PropertyOrder(2)]
    public List<StringDataMap> chineseGan = new List<StringDataMap>();
    [PropertyOrder(2)]
    public List<StringDataMap> chineseBun = new List<StringDataMap>();
    [PropertyOrder(2)]
    public List<StringDataMap> french = new List<StringDataMap>();

    
    [Serializable]
    public class StringDataMap
    {
        public StringDataMap(int key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public int key;
        [Multiline]
        public string value;
    }

    public static string GetStringData(int key)
    {
        List<StringDataMap> language = null;
        switch (Instance.languageType)
        {
            case LanguageType.None:
                break;
            case LanguageType.Korean:
                language = Instance.korean;
                break;
            case LanguageType.English:
                language = Instance.english;
                break;
            case LanguageType.Japanese:
                language = Instance.japanese;
                break;
            case LanguageType.ChineseGan:
                language = Instance.chineseGan;
                break;
            case LanguageType.ChineseBun:
                language = Instance.chineseBun;
                break;
            case LanguageType.French:
                language = Instance.french;
                break;
            default:
                break;
        }

        foreach (var item in language)
        {
            if(item.key == key)
            {
                return item.value;
            }
        }
        return "NULL";
    }

    public static void NextLanguage()
    {
        int language = (int)Instance.languageType;
        language++;
        language %= (int) LanguageType.End;
        Instance.languageType = (LanguageType) language;
    }
}
