using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deco : MonoBehaviour {

    [Header("꾸미기 아이템 번호 및 이름")]
    // 데코 번호. (1101 ~ 1199: Roof, 1201 ~ 1299: Stove, 1301 ~ 1399: Bulb, 1401 ~ 1499: Sign, 1501 ~ 1599: Accessory)
    public int index;
    // 데코 이름.
    public string decoName;
    [Header("꾸미기 아이템 타입")]
    // 데코 종류
    public DecoType type;
    [Header("꾸미기 아이템 아이콘")]
    // 데코 아이콘.  
    public Sprite sprite_Icon;
    [Header("꾸미기 아이템 적용 그림")]
    public Sprite sprite;
    [Header("꾸미기 아이템 가격")]
    public int price;

    // 데코 구입 여부.
    private bool isGet;
    public bool IsGet {
        get { return isGet; }
        set {
            isGet = value;
            PlayerPrefs.SetInt("Deco_" + index + "_isGet", value ? 1 : 0);

            if (value && IsApply) NyangCondition.Instance.ItemCondition(index);
        }
    }

    // 데코 적용 여부.
    private bool isApply;
    public bool IsApply {
        get { return isApply; }
        set {
            isApply = value;
            PlayerPrefs.SetInt("Deco_" + index + "_isApply", value ? 1 : 0);

            if (value && IsGet) NyangCondition.Instance.ItemCondition(index);
        }
    }

    // 데코 사용 가능 여부.
    private bool isAvailable;
    public bool IsAvailable {
        get { return isAvailable; }
        set {
            isAvailable = value;
            PlayerPrefs.SetInt("Deco_" + index + "_isAvailable", value ? 1 : 0);
        }
    }


    void Awake() {
        SetData();
    }

    public void SetData() {
        IsGet = (PlayerPrefs.GetInt("Deco_" + index + "_isGet") == 1);
        IsApply = (PlayerPrefs.GetInt("Deco_" + index + "_isApply") == 1);
        IsAvailable = (PlayerPrefs.GetInt("Deco_" + index + "_isAvailable") == 1);
        //Debug.Log("[Avail, Get, Apply] " + decoName + ": " + (IsAvailable ? "T" : "F") + "|" + (IsGet ? "T" : "F") + "|" + (IsApply ? "T" : "F"));
    }

}