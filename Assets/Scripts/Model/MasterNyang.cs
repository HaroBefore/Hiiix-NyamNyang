using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterNyang : MonoBehaviour {

    public static MasterNyang instance;

    public GameObject back;

    public Sprite[] tanningBackSprite;  // 0:NONE 1:Rare 2:WellDone 3:Over
    public Sprite normalSprite;
    public Sprite buffSprite;

    
    void Awake() {
        if (!instance) instance = this;

        SetTanningBack(PlayerPrefs.GetInt("MasterNyang_BackState"));
    }

    public void SetTanningBack(int state) {
        PlayerPrefs.SetInt("MasterNyang_BackState", state);
        back.GetComponent<SpriteRenderer>().sprite = tanningBackSprite[state];
    }

    public void Buff(bool isBuff) {
        this.GetComponent<SpriteRenderer>().sprite = (isBuff) ? buffSprite : normalSprite;
        back.GetComponent<RectTransform>().localPosition = new Vector2(0, (isBuff) ? 0.32f: -0.19f);
    }
}