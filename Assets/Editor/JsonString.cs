#if UNITY_EDITOR

using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Windows;

public class JsonString : Editor
{
    public class StringTable
    {
        public int Index;
        public string Korean;
        public string Engilish;
        public string Japanese;
        public string ChineseGan;
        public string ChineseBun;
        public string French;
    }

    static List<StringTable> itemList = new List<StringTable>();


    [MenuItem("Window/LaserZone/JsonTool/ReloadJsonString", priority = 1)]
    public static void ReloadJson()
    {
        LoadJson();

        for (int i = 0; i < itemList.Count; i++)
        {
            StringSetting(itemList[i]);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Reload JsonString Done");
    }

    private static void StringSetting(StringTable data)
    {
        //string stringDirectoryPath = "Assets/Resources/Prefabs/Lasers";
    }

    private static void LoadJson()
    {
        string jsonString = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Json/stringtable.json");

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        ParseJsonItem(jsonData);
        AssetDatabase.SaveAssets();
    }

    private static void ParseJsonItem(JsonData jsonData)
    {
        AssetDatabase.StartAssetEditing();

        int tmp = 0;
        itemList.Clear();
        StringDataObject dataObject = StringDataObject.Instance;
        dataObject.korean.Clear();
        dataObject.english.Clear();
        dataObject.japanese.Clear();
        dataObject.chineseGan.Clear();
        dataObject.chineseBun.Clear();
        dataObject.french.Clear();

        for (int i = 0; i < jsonData.Count; i++)
        {
            StringTable stringTable = new StringTable();

            int.TryParse(jsonData[i]["Index"].ToString(), out tmp);
            stringTable.Index = tmp;
            IDictionary dic = jsonData[i] as IDictionary;
            if (dic.Contains("Korean"))
            {
                stringTable.Korean = jsonData[i]["Korean"].ToString();
            }
            if(dic.Contains("English"))
            {
                stringTable.Engilish = jsonData[i]["English"].ToString();
            }

            if (dic.Contains("Japanese"))
            {
                stringTable.Japanese = jsonData[i]["Japanese"].ToString();
            }

            if (dic.Contains("ChineseGan"))
            {
                stringTable.ChineseGan = jsonData[i]["ChineseGan"].ToString();
            }

            if (dic.Contains("ChineseBun"))
            {
                stringTable.ChineseBun = jsonData[i]["ChineseBun"].ToString();
            }

            if (dic.Contains("French"))
            {
                stringTable.French = jsonData[i]["French"].ToString();
            }

            itemList.Add(stringTable);
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            dataObject.korean.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].Korean));
            dataObject.english.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].Engilish));
            dataObject.japanese.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].Japanese));
            dataObject.chineseGan.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].ChineseGan));
            dataObject.chineseBun.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].ChineseBun));
            dataObject.french.Add(new StringDataObject.StringDataMap(itemList[i].Index, itemList[i].French));
        }

        EditorUtility.SetDirty(dataObject);
        AssetDatabase.StopAssetEditing();
    }
}

#endif