using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour {

    public static RouletteManager instance;

    // 베팅 단위.
    public int bettingBy;
    // 배율.
    public float rate01;
    public float rate02;
    public float rate03;
    public float rate04;
    public float rate05;
    // 배율 확률.
    public float[] rateProbability;
    // 배율 각도.
    public float[] rateAngle;

    // 룰렛 보드.
    public GameObject rouletteBoard;
    // 머니 보드.
    public GameObject moneyBoard;
    // 버튼.
    public GameObject pushButton;
    public GameObject pushButton_Pushed;
    // 팝업.
    public GameObject popup;
    // 판돈.
    private int money;

    // 룰렛이 멈췄는지 여부.
    private bool isStopped;

    // 룰렛이 돌아가는 중인지 여부.
    private bool isPlaying;

    // 결과 배율.
    private float resultRate;

    void Awake() {
        if (!instance) instance = this;
    }

    // SetRoulette: 룰렛 게임 초기 설정.
    private void SetRoulette() {
        // 룰렛 보드 초기화.
        rouletteBoard.transform.GetChild(1).rotation = Quaternion.identity;
        // 머니 보드 초기화.
        moneyBoard.transform.GetChild(0).GetComponent<Text>().text = "0";
        // 버튼 초기화.
        pushButton.SetActive(true);
        pushButton_Pushed.SetActive(false);
        // 스위치를 일단 끈다.
        isStopped = false;
        isPlaying = false;
    }


    // BettingUp/Down: 가진 돈을 차감하여 베팅한다.
    public void BettingUp(int amount = 1) {
        int betMoney = bettingBy * amount;
        if (isPlaying) return;
        if (GoldManager.instance.CurrentGold >= betMoney) {
            GoldManager.instance.CurrentGold -= betMoney;
            money += betMoney;
            moneyBoard.transform.GetChild(0).GetComponent<Text>().text = money.ToString();
        }
    }
    public void BettingDown(int amount = 1) {
        int betMoney = bettingBy * amount;
        if (isPlaying) return;
        if(money % betMoney != 0) {
            int decreaseMoney = (money % betMoney);
            GoldManager.instance.CurrentGold += decreaseMoney;
            money -= decreaseMoney;
            moneyBoard.transform.GetChild(0).GetComponent<Text>().text = money.ToString();
        }
        else if (money >= betMoney) {
            GoldManager.instance.CurrentGold += betMoney;
            money -= betMoney;
            moneyBoard.transform.GetChild(0).GetComponent<Text>().text = money.ToString();
        }
    }
    // BettingAllin: 올인한다.
    public void BettingAllin() {
        if (isPlaying) return;

        AudioManager.instance?.Play(AudioManager.instance.button01);
        InputManager.instance.AsdadSwitch();
        money += GoldManager.instance.CurrentGold;
        GoldManager.instance.CurrentGold = 0;
        moneyBoard.transform.GetChild(0).GetComponent<Text>().text = money.ToString();
        //PlayRoulette();
    }
    
    // PlayRoulette: 룰렛을 돌린다.
    public void PlayRoulette() {
        if (money == 0) return;
        isPlaying = true;
        pushButton.SetActive(false);
        pushButton_Pushed.SetActive(true);
        StartCoroutine(Roulette2());
        AudioManager.instance?.Play(AudioManager.instance.push_button, 1.75f);
        FindObjectOfType<AchievementManager>().RoulettePlayCount();
    }


    private IEnumerator Roulette2() {
        // 룰렛 초기 세팅.
        SetRoulette();
        // 룰렛이 설 곳을 정한다.
        float resultAngle = 0;
        int pop = Random.Range(0, 100);
        if (pop < rateProbability[0]) { resultRate = rate01; resultAngle = rateAngle[0]; }
        else if (pop < rateProbability[0] + rateProbability[1]) { resultRate = rate02; resultAngle = rateAngle[1]; }
        else if (pop < rateProbability[0] + rateProbability[1] + rateProbability[2]) { resultRate = rate03; resultAngle = rateAngle[2]; }
        else if (pop < rateProbability[0] + rateProbability[1] + rateProbability[2] + rateProbability[3]) { resultRate = rate04; resultAngle = rateAngle[3]; }
        else { resultRate = rate05; resultAngle = rateAngle[4]; }

        // 룰렛이 돌아가는 시간을 정한다.
        //float playTime = Random.Range(2, 10);
        float playTime = 2.2f;
        // 룰렛 회전 수를 정한다.
        float playRotCount = Random.Range(3, 10);

        // 총 회전 각도.
        float rotAngle = playRotCount * 360 + resultAngle;
        AudioManager.instance?.Play(AudioManager.instance.roulette, 2.2f);
        // 룰렛 돌리기를 시작한다.
        Transform board = rouletteBoard.transform.GetChild(1);
        float per = 0;
        float t = 0;
        while (!isStopped) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / playTime;
            per = 0.25f * Mathf.Log((timePer + Mathf.Exp(-4))) + 1;
            board.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, rotAngle, per));
            if (per >= 1) isStopped = true;
            yield return null;
        }
        board.localRotation = Quaternion.Euler(0, 0, resultAngle);
        yield return new WaitForSeconds(2.0f);
        GameOver(resultRate);
        if (resultRate == 0) FindObjectOfType<AchievementManager>().Roulette0Count();
    }
    // Roulette: 룰렛 진행. -> 사용안함...
    private IEnumerator Roulette() {
        // 룰렛 초기 세팅.
        SetRoulette();
        // 룰렛이 돌아가는 시간을 정한다.
        float time = Random.Range(2, 10);
        float turnTime = 0;
        // 처음 각속도를 정한다.
        float velocity = Random.Range(72, 361);
        // 룰렛 돌리기를 시작한다.
        while (!isStopped) {
            // ** 룰렛 속도 조절 나중에 수정... 지금은 정확히 time 초 만큼 돌아가는 것이 아님.
            Transform t = rouletteBoard.transform.GetChild(1);
            velocity = Mathf.Lerp(velocity, 0, turnTime / time);
            float rot = (t.localRotation).eulerAngles.z + velocity;
            rouletteBoard.transform.GetChild(1).transform.localRotation = Quaternion.Euler(0, 0, rot);
            turnTime += TimeManager.instance.deltaTime;
            if (velocity < 1) {
                isStopped = true;
                break;
            }
            yield return null;
        }
        float resultRot = rouletteBoard.transform.GetChild(1).localRotation.eulerAngles.z;
        while (resultRot < 0) resultRot += 360;
        if (resultRot > 360) resultRot -= 360 * (int)(resultRot / 360);
        if (0 <= resultRot && resultRot < 72) {
            resultRate = rate01;
        }
        else if (resultRot < 144) {
            resultRate = rate02;
        }
        else if (resultRot < 216) {
            resultRate = rate03;
        }
        else if (resultRot < 288) {
            resultRate = rate04;
        }
        else {
            resultRate = rate05;
        }
        yield return new WaitForSeconds(2.0f);
        // 룰렛을 종료하고, 결과를 전달한다.
        GameOver(resultRate);
    }

    // GameOver: 팝업을 띄우고, 돈을 지급한다.
    private void GameOver(float resultRate) {
        // 수익 전달.
        ResultManager.instance.SetIncomeRoulette((int)(money * (resultRate - 1)));
        // 돈 계산.
        money = (int)(money * resultRate);
        // 돈 지급.
        GoldManager.instance.CurrentGold += money;
        // 팝업 띄우기.
        popup.SetActive(true);
        popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "x " + resultRate.ToString();
        popup.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = money.ToString();
        // 판돈 초기화.
        money = 0;
    }

    // ResumeGame: 팝업창에서 버튼을 눌러 내일 장사를 시작.
    public void ResumeGame() {
        isPlaying = false;
        // 팝업을 닫는다.
        popup.SetActive(false);
        // UIManager를 통해 결과창을 보인다.
        UIManager.instance.ChangeSceneRouletteToResult();;
    }
}