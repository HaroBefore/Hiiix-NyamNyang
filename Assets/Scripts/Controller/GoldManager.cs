using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour {

    public static GoldManager instance;

    [Header("버프 지속 시간")]
    public float BuffDuration;
    private float _buffDuration;
    private float buffDuration {
        get { return _buffDuration; }
        set {
            _buffDuration = value;
            PlayerPrefs.SetFloat("BuffDuration", value);
        }
    }

    private bool isBuff;
    public bool IsBuff {
        get { return isBuff; }
        set {
            isBuff = value;
            if (isBuff) {
                UIManager.instance.BuffOn();
                NyangManager.instance.BuffNyang(true);
                MasterNyang.instance.Buff(true);
            }
            else {
                UIManager.instance.BuffOff();
                NyangManager.instance.BuffNyang(false);
                MasterNyang.instance.Buff(false);
            }
        }
    }

    private int incomeAM;
    public int Income {
        get { return incomeAM; }
        set {
            incomeAM = value;
            PlayerPrefs.SetInt("IncomeAM", value);
        }
    }
    private int incomePM;
    public int IncomeMinus {
        get { return incomePM; }
        set {
            incomePM = value;
            PlayerPrefs.SetInt("IncomePM", value);
        }
    }
    private int incomeBoss;
    public int IncomeBoss {
        get { return incomeBoss; }
        set {
            incomeBoss = value;
            PlayerPrefs.SetInt("IncomeBoss", value);
        }
    }

    private int currentGold;
    public int CurrentGold {
        get { return currentGold; }
        set {
            currentGold = value;
           FindObjectOfType<UIManager>().Money.transform.GetChild(1).GetComponent<Text>().text = value.ToString();
            PlayerPrefs.SetInt("PlayerMoney", value);
            if (value >= 10000) achievementManager.Achievement_MoneySwag(10000);
            if (value >= 100000) achievementManager.Achievement_MoneySwag(100000);
            if (value >= 500000) achievementManager.Achievement_MoneySwag(500000);
        }
    }

    // 각종 버프/디버프로 인한 최종 가격 계수.
    public float watasinopointowa { get; set; }
    [Header("냥이들에게 얼마나 높은 가격으로 후려칠 것인가?")]
    // 냥이들에게 얼마나 높은 가격으로 후려칠 것인가?
    public float conscienceOfSeller;
    [Header("버프중일 때 보너스")]
    public float buffBonus;
    // 썬탠 미니게임으로 인한 보너스.
    private float tanningBonus;
    public float TanningBonus {
        get { return tanningBonus; }
        set {
            tanningBonus = value;
            PlayerPrefs.SetFloat("TanningBonus", value);
        }
    }

    private AchievementManager achievementManager;

    void Awake() {

        if (!instance) instance = this;

        achievementManager = FindObjectOfType<AchievementManager>();

        CurrentGold = PlayerPrefs.GetInt("PlayerMoney");
        Income = PlayerPrefs.GetInt("IncomeAM");
        IncomeMinus = PlayerPrefs.GetInt("IncomeMinus");
        IncomeBoss = PlayerPrefs.GetInt("IncomeBoss");
        TanningBonus = PlayerPrefs.GetFloat("TanningBonus");
    }

    void Start() {
        BuffDuration = 60;
        buffDuration = PlayerPrefs.GetFloat("BuffDuration");
        if (buffDuration > 0) IsBuff = true;
    }

    void Update() {
        BuffDurationDown();
    }

    public float getFactor() {
        // 선탠 게임 보너스.
        float TanningBonus = (TimeManager.instance.timeType > TimeType.PMOpenTime) ? this.TanningBonus : 1;
        // 양심 보너스.
        float conscienceBonus = (CookManager.instance.cookFood.isRecipe) ? conscienceOfSeller : 0;
        // 버프보너스.
        float BuffBonus = (GoldManager.instance.IsBuff) ? buffBonus : 1;
        watasinopointowa = conscienceBonus * BuffBonus * TanningBonus;
        return watasinopointowa;
    }



    private void BuffDurationDown() {
        if (!IsBuff) return;
        if(buffDuration < BuffDuration) {
            buffDuration += TimeManager.instance.deltaTime;
            return;
        }

        IsBuff = false;

        buffDuration = 0;
    }
    

}