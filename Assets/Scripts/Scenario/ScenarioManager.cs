using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public enum ScenarioType {
    NONE = -1,
    Boss,
    Event,
    Nyang,
}

public class ScenarioManager : MonoBehaviour {

    public static ScenarioManager instance;

    public GameObject toozaPopup;

    public float sceneFadeTime;
    public float scriptingDelayTime;
    public float twinklingDelayTime_On;
    public float twinklingDelayTime_Off;

    public GameObject textBackgroundPrefab;
    private int textCount;

    private Scenario currentScenario;
    private GameObject currentImage;
    private ScenarioQueue scenarioQueue;
    public ScenarioType lastScenarioType;

    public GameObject scene;
    public GameObject texts;
    public Text sceneScript;
    private string originScript;
    public GameObject sceneTwinklingIcon;
    public Image FadeMask;

    private Dictionary<string, Scenario> scenarioDic;

    private IEnumerator iScripting;
    private bool isScripting;
    private IEnumerator iTwinkling;
    private bool isTwinkling;

    private bool isClickSkip;


    private GameObject topScriptBoard;
    private GameObject midScriptBoard;
    private GameObject botScriptBoard;
    private string scriptText;
    private bool changeImageSwitch;
    private bool subFadeSwitch;
    private bool day00_waitSwitch;
    private bool day00_toozaSwitch;
    void Awake() {
        if (!instance) instance = this;
        scenarioQueue = new ScenarioQueue();
        LoadScenarioPrefab();
        FindObjectOfType<TimeManager>().RegisterCallback_OnDayChanged(CheckScenarioCondition_Day);

        // 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyamNyangScenario") == 1049)) {
            lastScenarioType = ScenarioType.NONE;
            PlayerPrefs.SetInt("LastScenario", (int)(lastScenarioType));
            PlayerPrefs.SetInt("NyamNyangScenario", 1049);
        }
        else {
            lastScenarioType = (ScenarioType)PlayerPrefs.GetInt("LastScenario");
        }
    }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (isClickSkip) {
                if (isScripting) {
                    sceneScript.text = originScript;
                    StopCoroutine(iScripting);
                    isScripting = false;
                }
                if (isTwinkling) {
                    sceneTwinklingIcon.SetActive(false);
                    StopCoroutine(iTwinkling);
                    isTwinkling = false;
                }
            }
        }
    }


    #region Scenario Condition Manager

    private void CheckScenarioCondition_Day(int day) {
        //if (day == 1) AddScenario(scenarioDic["Day01"]);
        //if (day == 4) AddScenario(scenarioDic["NewIngredient"]);
        //if (day == 3) AddScenario(scenarioDic["BugNyang"]);
        //else if (day % 4 == 3) AddScenario(scenarioDic["BugNyang"]);
        if (day == 3) AddScenario(scenarioDic["Day03_BugNyang"]);
        else if (day == 7) AddScenario(scenarioDic["Day07_BugNyang"]);
        else if (day == 11) AddScenario(scenarioDic["Day11_BugNyang"]);
        else if (day % 4 == 3) AddScenario(scenarioDic["BugNyang"]);

        if (day == 0) AddScenario(scenarioDic["Day00"]);
        else if (day == 1) AddScenario(scenarioDic["Day01"]);
        else if (day == 2) AddScenario(scenarioDic["Day02_BuffNyang"]);
        else if (day == 4) AddScenario(scenarioDic["Day04"]);
        else if (day == 5) AddScenario(scenarioDic["Day05"]);
        else if (day == 8) AddScenario(scenarioDic["Day08"]);
        else if (day == 12) AddScenario(scenarioDic["Day12_NewIngredient"]);
        else if (day == 14) AddScenario(scenarioDic["Day14"]);
        else if (day == 15) AddScenario(scenarioDic["Day15_DalNyang"]);
        else if (day == 16) AddScenario(scenarioDic["Day16"]);
        else if (day == 20) AddScenario(scenarioDic["Day20"]);
        else if (day == 24) AddScenario(scenarioDic["Day24"]);
        else if (day == 28) AddScenario(scenarioDic["Day28"]);
        else if (day == 29) AddScenario(scenarioDic["Day29_DalNyang"]);
        else if (day == 30) AddScenario(scenarioDic["Day30_NewIngredient"]);
    }

    private void CheckScenarioCondition() {

    }

    #endregion

    #region Scenario Manager

    public void LoadScenarioPrefab() {
        scenarioDic = new Dictionary<string, Scenario>();

        Scenario[] objs = Resources.LoadAll<Scenario>("Prefabs/Scenario/ScenarioPack/") as Scenario[];

        for (int i = 0; i < objs.Length; i++)
            scenarioDic.Add(objs[i].scenarioName, objs[i]);
    }

    public void AddScenario(Scenario scenario) {
        scenarioQueue.Enqueue(scenario);
    }
    public bool IsEmpty() {
        return scenarioQueue.IsEmpty();
    }
    public void PlayScenario() {
        TimeManager.Instance.Resume();
        AudioManager.Instance?.PlayBGM(AudioManager.Instance?.background_minigame);
        SetScenario();
        lastScenarioType = currentScenario.type;
        PlayerPrefs.SetInt("LastScenario", (int)lastScenarioType);
        if (currentScenario.sceneCount == 0) {
            CloseScenario();
            return;
        }
        if (currentScenario.type == ScenarioType.Nyang) StartCoroutine(IFadeOn_First());
        else StartCoroutine(IFadeOn());
    }
    private void CloseScenario() {
        currentScenario.OnCloseScenario();
        if (!IsEmpty()) {
            PlayScenario();
            return;
        }
        if (lastScenarioType == ScenarioType.Nyang) TimeManager.Instance.GameStartOrContinue();

        FadeMask.gameObject.SetActive(false);
    }
    #endregion

    #region Scenario Scene Player
    public void FadeMaskDeactive() {
        FadeMask.gameObject.SetActive(false);
    }
    // FadeOn: X -> SceneBackground(Black)
    private IEnumerator IFadeOn_First() {
        ResetScene();
        scene.SetActive(true);
        FadeMask.gameObject.SetActive(false);
        StartCoroutine(IPlayScenario());
        yield return null;
    }
    private IEnumerator IFadeOn() {
        FadeMask.gameObject.SetActive(true);
        Color color = new Color(44f / 255f, 41f / 255f, 42f / 255f, 0);
        float curTime = 0;
        while (curTime < 1) {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, curTime / sceneFadeTime);
            FadeMask.color = color;
            yield return null;
        }
        yield return null;
        ResetScene();
        scene.SetActive(true);
        FadeMask.gameObject.SetActive(false);
        ResultManager.instance.CloseResultPanel();
        StartCoroutine(IPlayScenario());
    }
    private IEnumerator IPlayScenario() {
        int currentSceneNumber = 0;
        textCount = 0;
        while (currentSceneNumber < currentScenario.sceneCount) {
            if (currentScenario.type == ScenarioType.Nyang && currentSceneNumber == 13 && day00_toozaSwitch) break;
            if (currentScenario.type == ScenarioType.Nyang && currentSceneNumber == 10) {
                yield return StartCoroutine(SetScene(currentSceneNumber));
                isClickSkip = true;
                StartCoroutine(iScripting);
                yield return new WaitUntil(() => !isScripting);
                SetToozaPopup(true);
                day00_waitSwitch = true;
                yield return new WaitUntil(() => !day00_waitSwitch);
                isClickSkip = false;
                if (day00_toozaSwitch) currentSceneNumber = 11;
                else currentSceneNumber = 13;
                SetToozaPopup(false);
            }
            else {
                yield return StartCoroutine(SetScene(currentSceneNumber));
                isClickSkip = true;
                StartCoroutine(iScripting);
                yield return new WaitUntil(() => !isScripting);
                StartCoroutine(iTwinkling);
                yield return new WaitUntil(() => !isTwinkling);
                isClickSkip = false;
                currentSceneNumber++;
            }
        }
        StartCoroutine(IFadeOff());
    }
    // FadeOff: SceneBackground(Black) -> X
    private IEnumerator IFadeOff() {
        FadeMask.gameObject.SetActive(true);
        Color color = new Color(238f / 255f, 241f / 255f, 243f / 255f, 0);
        float curTime = 0;
        while (curTime < 1) {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, curTime / sceneFadeTime);
            FadeMask.color = color;
            yield return null;
        }
        yield return null;
        scene.SetActive(false);
        CloseScenario();
    }
    #endregion

    #region Scenario Settings
    private void SetScenario() {
        currentScenario = scenarioQueue.Dequeue();
    }
    private void ResetScene() {
        sceneTwinklingIcon.transform.SetParent(scene.transform);
        if (currentImage) Destroy(currentImage.gameObject);
        for (int i = 0; i < texts.transform.childCount; i++)
            Destroy(texts.transform.GetChild(i).gameObject);
        topScriptBoard = null;
        midScriptBoard = null;
        botScriptBoard = null;
        changeImageSwitch = false;
        subFadeSwitch = false;
    }

    private IEnumerator SetScene(int sceneNumber) {
        yield return StartCoroutine(SetImage(sceneNumber));
        SetScript(sceneNumber);
        if (subFadeSwitch) yield return StartCoroutine(SubFade(true));
        // 코루틴 설정.
        iScripting = Scripting();
        iTwinkling = Twinkling();
    }

    private IEnumerator SetImage(int sceneNumber) {
        // 이미지 전환이 일어나면 FadeOut.
        if (currentScenario.sceneImage[sceneNumber] != null) {
            if (sceneNumber != 0)
                yield return StartCoroutine(SubFade(false));
            if (currentImage) Destroy(currentImage);
            currentImage = Instantiate(currentScenario.sceneImage[sceneNumber]);
            currentImage.transform.SetParent(scene.transform);
            currentImage.GetComponent<RectTransform>().localPosition = new Vector2(0, 500);
            currentImage.GetComponent<RectTransform>().localScale = Vector3.one;
            changeImageSwitch = true;
        }
    }
    private void SetScript(int sceneNumber) {
        Color color1 = currentScenario.sceneTextBoardColors_1[sceneNumber];
        Color color2 = currentScenario.sceneTextBoardColors_2[sceneNumber];

        GameObject newText = Instantiate(textBackgroundPrefab);
        newText.transform.SetParent(texts.transform);
        newText.GetComponent<RectTransform>().localScale = Vector3.one;
        newText.transform.GetChild(0).GetComponent<Text>().text = currentScenario.sceneScripts_name[sceneNumber];
        sceneScript = newText.transform.GetChild(1).GetComponent<Text>();
        sceneScript.text = "";// currentScenario.sceneScripts[sceneNumber];
        scriptText = currentScenario.sceneScripts[sceneNumber];
        sceneTwinklingIcon.transform.SetParent(newText.transform);
        sceneTwinklingIcon.GetComponent<RectTransform>().localPosition = new Vector2(401, -83);

        if (changeImageSwitch) {
            Destroy(topScriptBoard);
            Destroy(midScriptBoard);
            Destroy(botScriptBoard);
            topScriptBoard = null;
            midScriptBoard = null;
            botScriptBoard = null;
            changeImageSwitch = false;
        }
        if (!topScriptBoard) {
            topScriptBoard = newText;
            topScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -90);
        }
        else if (!midScriptBoard) {
            midScriptBoard = newText;
            midScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -420);
        }
        else if (!botScriptBoard) {
            botScriptBoard = newText;
            botScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -750);
        }
        else {
            Destroy(topScriptBoard);
            topScriptBoard = midScriptBoard;
            topScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -90);
            midScriptBoard = botScriptBoard;
            midScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -420);
            botScriptBoard = newText;
            botScriptBoard.GetComponent<RectTransform>().localPosition = new Vector2(0, -750);
        }

        if (topScriptBoard) topScriptBoard.GetComponent<Image>().color = color1;
        if (midScriptBoard) midScriptBoard.GetComponent<Image>().color = color2;
        if (botScriptBoard) botScriptBoard.GetComponent<Image>().color = color1;

        textCount++;
    }


    private IEnumerator SubFade(bool isIn) {
        FadeMask.gameObject.SetActive(true);
        Color color = new Color(44f / 255f, 41f / 255f, 42f / 255f, (isIn) ? 1 : 0);
        float curTime = 0;
        while (curTime < 1) {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp((isIn) ? 1 : 0, (isIn) ? 0 : 1, curTime / (sceneFadeTime / 3));
            FadeMask.color = color;
            yield return null;
        }
        if (isIn) FadeMask.gameObject.SetActive(false);
        subFadeSwitch = (isIn) ? false : true;
    }

    #endregion

    #region Coroutines
    // Scripting: 글자를 한 글자씩 출력. 클릭시 스킵(즉시 전체출력)
    private IEnumerator Scripting() {
        isScripting = true;
        originScript = scriptText;
        builder = new StringBuilder(scriptText);
        builder.Remove(0, builder.Length);
        builder.Append(scriptText);
        int endIndex = 0;
        sceneScript.text = "";
        while (isScripting && sceneScript.text != originScript) {
            sceneScript.text = builder.ToString(0, endIndex);
            yield return new WaitForSeconds(scriptingDelayTime);
            if (endIndex >= builder.Length) {
                endIndex = builder.Length;
                break;
            }
            ++endIndex;
        }
        isScripting = false;
    }
    // Twinkling: 아이콘을 반짝인다. 클릭시 스킵.
    private IEnumerator Twinkling() {
        isTwinkling = true;
        while (isTwinkling) {
            sceneTwinklingIcon.SetActive(true);
            yield return new WaitForSeconds(twinklingDelayTime_On);
            sceneTwinklingIcon.SetActive(false);
            yield return new WaitForSeconds(twinklingDelayTime_Off);
        }
        isTwinkling = false;
    }
    #endregion

    #region OneOneGoodText
    StringBuilder builder = null;
    bool bPlay = false;

    private void ScriptingText(string _script) {
        builder = new StringBuilder(_script);
        builder.Remove(0, builder.Length);
        builder.Append(_script);
        iScripting = OneOneTextGood(scriptingDelayTime);
        OneOneTextGoodPlay();
    }
    private void OneOneTextGoodPlay() {
        bPlay = true;
        StartCoroutine(iScripting);
    }
    private void OneOneTextGoodStop(bool bForce = false) {
        bPlay = false;
        if (bForce)
            StopCoroutine(iScripting);
    }
    private IEnumerator OneOneTextGood(float delay) {
        int endIndex = 0;
        while (bPlay) {
            sceneScript.text = builder.ToString(0, endIndex);
            yield return new WaitForSeconds(delay);
            if (endIndex >= builder.Length) {
                endIndex = builder.Length;
                OneOneTextGoodStop();
                break;
            }
            ++endIndex;
        }
    }
    #endregion


    public void SetToozaPopup(bool isActivate) {
        toozaPopup.SetActive(isActivate);
    }
    public void Tooza() {
        IAPManager.instance.PurchaseItem_RemoveAds();
        if (AdsManager_Admob.instance.IsRemoveAds) {
            day00_waitSwitch = false;
            day00_toozaSwitch = true;
        }
    }
    public void NoTooza() {
        day00_waitSwitch = false;
        day00_toozaSwitch = false;
    }
}