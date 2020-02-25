using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Ingredient : MonoBehaviour {

    [Header("재료 번호 및 이름")]
    // 재료 번호. (401~499: Meat, 501~599: Powder, 601~699: Sauce)
    public int index;
    // 재료 이름.
    public string ingredientName;
    [Header("재료 타입")]
    // 재료 종류.
    public IngredientType type;
    [Header("재료 아이콘: 재료 상자, 레시피")]
    // 재료 아이콘.  
    public Sprite sprite_Icon;
    [Header("재료 상점 아이콘 (갈비만)")]
    public Sprite sprite_Market;
    [Header("재료 상점 팝업 아이콘")]
    public Sprite sprite_Market_Popup;
    // 재료(갈비) 이미지.
    [Header("조리중 음식 7단계 + 소스 2개 + 오버쿡 (갈비만)")]
    public Sprite[] sprites;
    // 갈비에 따른 가루,소스 위치 정보.
    [Header("갈비에 따른 [0]가루, [1]소스 위치 정보.")]
    public Vector2[] positions;
    // 재료(가루, 소스) 색상 코드.
    [Header("해당 재료 색상 코드 (파우더, 소스만)")]
    public Color color;
    // 재료 가격.
    [Header("재료 가격")]
    public int price;

    // 재료 사용 가능 여부.
    private bool isAvailable;
    public bool IsAvailable {
        get { return isAvailable; }
        set {
            isAvailable = value;
            PlayerPrefs.SetInt("Ingredient_" + index + "_isAvailable", value ? 1 : 0);
        }
    }

    void Awake() {
        SetData();
    }
    
    public void SetData() {
        IsAvailable = (PlayerPrefs.GetInt("Ingredient_" + index + "_isAvailable") == 1);
    }


}