using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    public static ResultManager instance;

    [Header("Result")]
    public GameObject ResultPanel;
    public Text dayCount;
    public Text income_Text;
    public Text incomeMinus_Text;
    public Text incomeRoulette_Text;
    public Text incomeTotal_Text;
    private int income;
    private int incomeMinus;
    private int incomeRoulette;
    private int incomeTotal;

    [Header("BossResult")]
    public GameObject bossResultPanel;
    public Text dayCount_Boss;
    public Text incomeBossText;
    public Text incomeTipText;
    public Text repairCostText;
    public Text incomeBossTotalText;
    private int incomeBoss;
    private int incomeTip;
    private int repairCost;
    private int incomeBossTotal;

    public bool isBoss;

    void Awake() {
        if (!instance) instance = this;
    }

    public void OpenResult() {
        AudioManager.Instance?.Play(AudioManager.Instance.close, 2.5f);
        UIManager.instance.Main_Scene.SetActive(false);
        UIManager.instance.Main_Objects.SetActive(false);
        UIManager.instance.Main_UI.SetActive(false);
        UIManager.instance.Calender.SetActive(false);
        ResultPanel.SetActive(true);
        dayCount.text = TimeManager.Instance.Day.ToString();
        SetIncomeTotal();   // AM, PM, Roulette은 각 장사가 끝나면 미리 호출하여 처리되어있게 함.
    }
    public void CloseResult() {
        GoldManager.instance.Income = 0;
        GoldManager.instance.IncomeMinus = 0;
        // 메인 게임 시작: 오전 장사로.
        isBoss = false;
        // TODO 타이틀로
    }
    public void CloseResultPanel() {
        ResultPanel.SetActive(false);
        UIManager.instance.Main_Scene.SetActive(true);
        UIManager.instance.Main_Objects.SetActive(true);
        UIManager.instance.Main_UI.SetActive(true);
        UIManager.instance.Calender.SetActive(true);
        isBoss = false;
    }

    #region setIncome...
    public void SetIncome(int income) {
        this.income = income;
        income_Text.text = "+" + income.ToString();
    }
    public void SetIncomeMinus(int income) {
        incomeMinus = income;
        incomeMinus_Text.text = "-" + income.ToString();
    }
    public void SetIncomeRoulette(int income) {
        incomeRoulette = income;
        if (income >= 0) incomeRoulette_Text.text = "+" + income.ToString();
        else incomeRoulette_Text.text = income.ToString();
    }
    public void SetIncomeTotal() {
        incomeTotal = income - incomeMinus + incomeRoulette;
        incomeTotal_Text.text = incomeTotal.ToString();
    }

    public void SetIncomeBoss(int income) {
        incomeBoss = income;
        incomeBossText.text = income.ToString();
    }
    public void SetIncomeTip(int income) {
        incomeTip = income;
        incomeTipText.text = income.ToString();
    }
    public void SetRepairCost(int cost) {
        repairCost = -cost;
        repairCostText.text = cost.ToString();
    }
    public void SetBossIncomeTotal() {
        incomeBossTotal = incomeBoss + incomeTip + repairCost;
        incomeBossTotalText.text = incomeBossTotal.ToString();
    }
    #endregion
}