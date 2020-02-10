using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public event Action EventOpenResult = () => { };
    public event Action EventCloseResult = () => { };
    
    public static ResultManager instance;

    [Header("Result")]
    public GameObject ResultPanel;
    public Text dayCount;
    public Text incomeAm_Text;
    public Text incomePm_Text;
    public Text incomeTotal_Text;
    private int incomeAm;
    private int incomePm;
    private int incomeTotal;
    
    void Awake() {
        if (!instance) instance = this;
    }

    public void OpenResult() {
        AudioManager.Instance?.Play(AudioManager.Instance.close, 2.5f);

        EventOpenResult();
        
        ResultPanel.SetActive(true);
        dayCount.text = TimeManager.Instance.Day.ToString();
    }
    public void CloseResult(TimeType timeType) {
        GoldManager.instance.IncomeAm = 0;
        GoldManager.instance.IncomePm = 0;
        ResultPanel.SetActive(false);
        EventCloseResult();
    }

    #region setIncome...
    public void SetIncomeAm(int income) {
        this.incomeAm = income;
        incomeAm_Text.text = "+" + income.ToString();
    }
    public void SetIncomePm(int income) {
        this.incomePm = income;
        incomePm_Text.text = "+" + income.ToString();
    }
    
    public void SetIncomeTotal(int totalGold) {
        incomeTotal = totalGold;
        incomeTotal_Text.text = incomeTotal.ToString();
    }
    #endregion
}