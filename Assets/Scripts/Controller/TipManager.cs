using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TipType {
    NONE = -1,
    Buff,
    CatList,
    Cook,
    Option
}

public class TipManager : MonoBehaviour {

    public static TipManager instance;

    [Header("팁이 얼마나 오래 떠있을것인지")]
    public float TipTalkTicTok;
    private float tipTalkTicTok;
    [Header("Tip Sprites")]
    public Sprite[] tipSprites;
    [Header("TipObject")]
    public GameObject tip;
    [Header("CookTipObjects")]
    public GameObject cookTip;
    public GameObject cookTip_hand;



    private IEnumerator Coroutine_cookTipAnimation;
    private Action cbOnChangedTipType;

    private bool isTipTalking;
    private TipType currentTipType;
    private TipType CurrentTipType {
        get { return currentTipType; }
        set {
            currentTipType = value;
            cbOnChangedTipType();
        }
    }


    private bool isTipOn;

    void Awake() {
        if (!instance) instance = this;

        cbOnChangedTipType += OnTipTypeChanged;
    }

    void Update() {
        TipTalk();
    }


    public void TipTalk() {
        if (!isTipOn) return;
        if (!isTipTalking) return;
        if (tipTalkTicTok < TipTalkTicTok) {
            tipTalkTicTok += TimeManager.instance.deltaTime;
            return;
        }
        CloseTip();
        tipTalkTicTok = 0;
    }

    public void ShowTip(TipType type) {
        if (!isTipOn) {
            AudioManager.instance?.Play(AudioManager.instance.tipnyang, 1f);
            tip.SetActive(true);
            tip.transform.GetChild(0).GetComponent<Image>().sprite = tipSprites[(int)type];
            tip.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            tip.transform.GetChild(0).gameObject.SetActive(true);
            isTipOn = true;
            isTipTalking = true;
            CurrentTipType = type;
        }
    }

    public void CloseTip(TipType type = TipType.NONE) {
        if ((type != TipType.NONE) && (CurrentTipType != type)) return;
        //if (type == TipType.Buff) TimeManager.instance.isBuffTipOn = false;
        else if (type == TipType.Cook) CloseCookTip();
        tip.SetActive(false);
        isTipOn = false;
        tipTalkTicTok = 0;
        isTipTalking = false;
    }

    public void HideTip() {
        if (isTipOn) {
            tip.SetActive(false);
            isTipTalking = false;
        }
    }
    public void UnhideTip() {
        if (isTipOn) {
            tip.SetActive(true);
            isTipTalking = true;
        }
    }


    private void OnTipTypeChanged() {
        if (CurrentTipType == TipType.Cook) {
            ShowCookTip();
        }
    }


    public void TipButton() {
        if (CurrentTipType == TipType.Buff) UIManager.instance.OpenBuffPopup();
        else if(CurrentTipType==TipType.Option) {
            UIManager.instance.OpenOption();
            OptionManager.instance.ApplicationRemoveAds();
        }
    }

    #region CookTip
    private void ShowCookTip() {
        cookTip.SetActive(true);
        StartCookTipAnimation();
    }
    private void CloseCookTip() {
        cookTip.SetActive(false);
        StopCookTipAnimation();
    }
    private void StartCookTipAnimation() {
        Coroutine_cookTipAnimation = CookTipAnimation();
        StartCoroutine(Coroutine_cookTipAnimation);
    }
    private void StopCookTipAnimation() {
        StopCoroutine(Coroutine_cookTipAnimation);
    }
    private IEnumerator CookTipAnimation() {
        RectTransform rt = cookTip_hand.GetComponent<RectTransform>();
        Vector2 pos1 = new Vector2(0, -33.8f);
        Vector2 pos2 = new Vector2(0, -77.2f);
        float animationTime = 0.5f;

        float per = 0;
        float perTime = 0;
        while (true) {
            while (per < 1) {
                perTime += Time.deltaTime;
                per = perTime / animationTime;
                rt.localPosition = Vector2.Lerp(pos1, pos2, per);
                yield return null;
            }
            yield return new WaitForSeconds(animationTime);
            rt.localPosition = pos1;
            per = 0;
            perTime = 0;
        }
    }
    #endregion
}