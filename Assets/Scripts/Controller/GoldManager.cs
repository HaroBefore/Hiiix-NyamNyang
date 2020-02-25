using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public event Action<int> EventIncomeAmChanged = n => { };
    public event Action<int> EventIncomePmChanged = n => { };
    public event Action<int> EventTotalGoldChanged = n => { };

    public static GoldManager instance;

    private bool isBuff;
    public bool IsBuff {
        get { return isBuff; }
        set {
            isBuff = value;
            if (isBuff) {
                UIManager.instance.BuffOn();
                NyangManager.Instance.BuffNyang(true);
                MasterNyang.instance.Buff(true);
            }
            else {
                UIManager.instance.BuffOff();
                NyangManager.Instance.BuffNyang(false);
                MasterNyang.instance.Buff(false);
            }
        }
    }

    private int _incomeAm;
    public int IncomeAm {
        get { return _incomeAm; }
        set {
            _incomeAm = value;
            EventIncomeAmChanged(_incomeAm);
            TotalGold = _incomeAm + _incomePm;
        }
    }
    private int _incomePm;
    public int IncomePm {
        get { return _incomePm; }
        set {
            _incomePm = value;
            EventIncomePmChanged(_incomePm);
            TotalGold = _incomeAm + _incomePm;
        }
    }

    private int _totalGold;
    public int TotalGold {
        get { return _totalGold; }
        set {
            _totalGold = value;
            EventTotalGoldChanged(_totalGold);
            
            
            FindObjectOfType<UIManager>().Money.transform.GetChild(2).GetComponent<Text>().text = value.ToString();
            
            if (achievementManager == null)
            {
                return;
            }

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
    private float _tanningBonus = 1f;
    public float TanningBonus {
        get => _tanningBonus;
        set => _tanningBonus = value;
    }

    private AchievementManager achievementManager;

    void Awake() {

        if (!instance) instance = this;

        achievementManager = FindObjectOfType<AchievementManager>();
    }
  
    public float getFactor() {
        // 선탠 게임 보너스.
        //float TanningBonus = (TimeManager.Instance.timeType > TimeType.PMOpenTime) ? this.TanningBonus : 1;
        // 양심 보너스.
        float conscienceBonus = (CookManager.instance.cookFood.isRecipe) ? conscienceOfSeller : 0;
        // 버프보너스.
        float BuffBonus = (GoldManager.instance.IsBuff) ? buffBonus : 1;
        watasinopointowa = conscienceBonus * BuffBonus * TanningBonus;
        return watasinopointowa;
    }
}