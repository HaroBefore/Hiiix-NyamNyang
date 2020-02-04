using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TanningManager : MonoBehaviour {

    public static TanningManager instance;

    // 최대 시간. 이 시간의 38.7% ~ 64.5% 가 적합.
    public float TanningTime;
    private float tanningTime;

    public GameObject readyBox;
    public GameObject guage;
    public GameObject character;
    public GameObject resultBox;
    public GameObject heartBox;
    public GameObject pushText;
    public GameObject pushButton;
    public GameObject pushButton_Pushed;
    public GameObject popup;

    private int result; // 0:NONE, 1:Rare, 2:WELLDONE, 3:OVERCOOK
    private bool isStopped;

    // Tutorial.
    private bool isTutorial;
    public bool switch01 { get; set; }
    public bool switch02 { get; set; }
    public bool switch03 { get; set; }
    public bool switch04 { get; set; }
    public bool switch05 { get; set; }
    public bool switch06 { get; set; }
    public bool switch07 { get; set; }
    public bool switch08 { get; set; }

    void Awake() {
        if (!instance) instance = this;
    }

    // SetTanning: 썬탠 게임 초기 설정.
    private void SetTanning() {
        // 캐릭터 뒷모습을 보여준다.
        SetCharacter(false);
        character.transform.GetChild(3).gameObject.SetActive(false);
        // 노란 말풍선에 Ready.
        SetReadyBox(false);
        // 결과창 숨기기.
        SetResultBox(0);
        readyBox.SetActive(true);
        // 시간을 0으로.
        tanningTime = 0;
        // 게이지 초기화.
        SetTemperatureGuage(0);
        // 결과 초기화.
        result = 0;
        // 스위치를 일단 끈다.
        isStopped = false;
        pushButton.SetActive(true);
        pushText.SetActive(true);
        pushButton_Pushed.SetActive(false);
    }
    // StopTanning: Push 버튼을 누르거나, 온도가 끝까지 올라가면 호출되어 스위치를 연다.
    public void StopTanning() {
        AudioManager.instance?.Play(AudioManager.instance.push_button, 1.75f);
        if (isTutorial) switch07 = true;
        else isStopped = true;
        pushButton.SetActive(false);
        pushText.SetActive(false);
        pushButton_Pushed.SetActive(true);
    }

    // Tanning: 썬탠 게임 진행.
    private IEnumerator Tanning() {
        // 초기 세팅.
        SetTanning();
        // 1초 뒤에,
        yield return new WaitForSeconds(1.5f);
        // 노란 말풍선 변경.
        SetReadyBox(true);
        yield return new WaitForSeconds(0.2f);
        // 멈출 때까지 온도가 올라간다.
        AudioManager.instance?.PlayTanningGaugeUp();
        while (!isStopped) {
            tanningTime += TimeManager.Instance.deltaTime;
            SetTemperatureGuage(tanningTime);
            if (tanningTime >= TanningTime) {
                StopTanning();
                break;
            }
            yield return null;
        }
        AudioManager.instance?.StopTanningGaugeUp();
        // 결과를 계산한다.
        float percent = (tanningTime / TanningTime) * 100;
        if (percent < 38.7f) result = 1;
        else if (percent < 64.5f) result = 2;
        else result = 3;
        // 캐릭터 앞을 보여준다.
        SetCharacter(true);
        // 결과를 보여준다.
        SetResultBox(result);
        yield return new WaitForSeconds(2.5f);
        // 썬탠을 종료하고, 결과를 전달한다.
        GameOver(result);
        if (result == 2) FindObjectOfType<AchievementManager>().TanningSuccessCount();
    }

    // TanningTutorial: 썬탠 튜토리얼.
    private IEnumerator TanningTutorial() {
        // 초기 세팅.
        isTutorial = true;
        SetTanning();
        yield return new WaitUntil(() => switch01);
        SetReadyBox(true);
        while (!isStopped) {
            tanningTime += TimeManager.Instance.deltaTime;
            SetTemperatureGuage(tanningTime);
            if (tanningTime >= TanningTime / 2) {
                break;
            }
            yield return null;
        }
        switch01 = false;
        yield return new WaitUntil(() => switch07);
        isStopped = true;
        result = 2;
        // 캐릭터 앞을 보여준다.
        SetCharacter(true);
        // 결과를 보여준다.
        SetResultBox(result);

        switch02 = false;
        yield return new WaitUntil(() => switch03);
        result = 1;
        SetResultBox(result);
        SetCharacter(true);
        switch03 = false;
        yield return new WaitUntil(() => switch04);
        result = 3;
        SetResultBox(result);
        SetCharacter(true);
        switch04 = false;
        yield return new WaitUntil(() => switch05);
        result = 2;
        SetResultBox(result);
        SetCharacter(true);
        switch05 = false;
        yield return new WaitUntil(() => switch06);
        switch06 = false;
        isTutorial = false;
        yield return new WaitUntil(() => switch08);
        GameOver(result);
    }

    // GameStart: 게임 시작 명령.
    public void GameStart() {
        AudioManager.instance?.PlayBGM(AudioManager.instance.background_minigame);
        // 처음 실행시 - 튜토리얼
        if (!(PlayerPrefs.GetInt("NyamNyangTanning") == 1049)) {
            StartCoroutine(TanningTutorial());
            TutorialManager.instance.PlayTanningTutorial();
            PlayerPrefs.SetInt("NyamNyangTanning", 1049);
        }
        else {
            StartCoroutine(Tanning());
        }
    }
    
    // GameOver: 팝업을 띄우고, 오후 장사에 결과에 따른 버프/디버프를 전달한다.
    private void GameOver(int result) {
        // 팝업 띄우기.
        popup.SetActive(true);
        // 버프 전달.
        ReturnBuff(result);
    }

    // ResumeGame: 팝업창에서 버튼을 눌러 오후장사를 시작.
    public void ResumeGame() {
        // 팝업을 닫는다.
        popup.SetActive(false);
        // UIManager를 통해 메인 게임으로 돌아간다.
        UIManager.instance.ChangeSceneTanningToMain();
        // 썬탠을 끝낸다.
        TimeManager.Instance.isTanning = false;
    }

    private void ReturnBuff(int result) {
        switch (result) {
            case 1:
                // 판매액 버프 설정.
                GoldManager.instance.TanningBonus = 0.5f;
                break;
            case 2:
                // 판매액 버프 설정.
                GoldManager.instance.TanningBonus = 1.5f;
                break;
            case 3:
                // 판매액 버프 설정.
                GoldManager.instance.TanningBonus = 0.5f;
                break;

        }
    }


    // SetCharacter: 캐릭터 뒤를 보여줄 지 앞을 보여줄 지 결정. (true: 앞)
    private void SetCharacter(bool isFront) {
        character.transform.GetChild(1).gameObject.SetActive(!isFront);
        character.transform.GetChild(2).gameObject.SetActive(isFront);
        if (result == 2) {
            character.transform.GetChild(3).gameObject.SetActive(isFront);
            character.transform.GetChild(4).gameObject.SetActive(false);
        }
        else {
            character.transform.GetChild(4).gameObject.SetActive(false);
            character.transform.GetChild(4).gameObject.SetActive(isFront);
        }
        }
    // SetReadyBox: Ready를 보여줄 지, GO!를 보여줄 지 결정. (true: GO!)
    private void SetReadyBox(bool isGO) {
        readyBox.transform.GetChild(0).GetComponent<Text>().text = (isGO) ? "GO!" : "Ready";
    }
    // SetResultBox: 결과를 보여준다. (0: Deactive 1:RARE! 2:WELLDONE 3:OVERCOOK!)
    private void SetResultBox(int index) {
        Image boxImage = resultBox.transform.GetChild(0).GetComponent<Image>();
        RectTransform textRectTransform = resultBox.transform.GetChild(1).GetComponent<RectTransform>();
        Text text = resultBox.transform.GetChild(1).GetComponent<Text>();
        GameObject heart = resultBox.transform.GetChild(2).gameObject;

        readyBox.SetActive(false);
        SetHeartBox(false);
        heart.SetActive(false);
        resultBox.SetActive(!(index == 0));

        switch (index) {
            case 1:
                text.text = "RARE\n!";
                textRectTransform.localPosition = new Vector2(0, 0);
                boxImage.color = new Color(255f / 255f, 195f / 255f, 190f / 255f, 1);
                AudioManager.instance?.Play(AudioManager.instance.tanning_fail, 1.5f);
                break;
            case 2:
                text.text = "WELL\nDONE";
                textRectTransform.localPosition = new Vector2(0, 11);
                boxImage.color = new Color(202f / 255f, 160f / 255f, 207f / 255f, 1);
                AudioManager.instance?.Play(AudioManager.instance.tanning_success, 2f);
                heart.SetActive(true);
                SetHeartBox(true);
                break;
            case 3:
                text.text = "OVER\nCOOK\n!";
                textRectTransform.localPosition = new Vector2(0, 0);
                boxImage.color = new Color(255f / 255f, 195f / 255f, 190f / 255f ,1);
                AudioManager.instance?.Play(AudioManager.instance.tanning_fail, 1.5f);
                break;
        }

        MasterNyang.instance.SetTanningBack(index);
    }
    // SetHeartBox: 하트 박스를 보여준다. (true: Active)
    private void SetHeartBox(bool isWELLDONE) {
        heartBox.SetActive(isWELLDONE);
    }
    // SetTemperatureGuage: 온도 게이지를 설정한다. (f 초만큼 지났을 때로.)
    private void SetTemperatureGuage(float f) {
        guage.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(16, 35 + 155 * (f / TanningTime));
    }
    
    
}