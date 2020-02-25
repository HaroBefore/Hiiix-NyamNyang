using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{

    public event Action EventEndTutorial;
    
    public static TutorialManager instance;

    public GameObject MainScene;
    public GameObject MainObjects;
    public GameObject MainUI;
    public GameObject tutorialMainScene;
    public GameObject TanningScene;
    public GameObject RouletteScene;
    public GameObject shadow;

    public GameObject defaultTanningTipNyang;
    private GameObject previousScene;



    public float printDelayTime = 0.05f;
    public float twinklingDelayTime_On = 0.5f;
    public float twinklingDelayTime_Off = 0.2f;

    private Transform tutorial;
    public GameObject twinklingIcon;
    public Text script;
    private string originScript;

    

    private bool isClickSkip;
    
    // Coroutine.
    private IEnumerator Coroutine_Scripting;
    private bool isScripting;
    private IEnumerator Coroutine_Twinkling;
    private bool isTwinkling;
    private IEnumerator Coroutine_00;
    void Awake() {
        if (!instance) instance = this;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (isClickSkip) {
                if (isScripting) {
                    script.text = originScript;
                    StopCoroutine(Coroutine_Scripting);
                    isScripting = false;
                }
                if (isTwinkling) {
                    twinklingIcon.SetActive(false);
                    StopCoroutine(Coroutine_Twinkling);
                    isTwinkling = false;
                }
            }
        }
    }

    private void RememberPreviousScene() {
        if (MainScene.activeSelf) previousScene = MainScene;
        else if (TanningScene.activeSelf) previousScene = TanningScene;
        else if (RouletteScene.activeSelf) previousScene = RouletteScene;
    }

    public void PlayTutorial() {
        BackgroundManager.instance.SetAM();
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.Pause();
        }
        tutorialMainScene.SetActive(true);
        MainScene.SetActive(false);
        MainUI.SetActive(false);
        MainObjects.SetActive(false);

        StartCoroutine(Tutorial());
    }
    public void PlayTanningTutorial() {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.Pause();            
        }
        StartCoroutine(TanningTutorial());
    }

    public IEnumerator Tutorial()
    {
        GameManager.Instance.IsTutorial = true;
        Nyang nyang;
        {
            // 새 튜토리얼 설정.
            SetTutorial(0);

            isClickSkip = true;

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(1);
            twinklingIcon = tutorial.GetChild(7).gameObject;
            //shadow = tutorial.GetChild(8).gameObject;
            shadow.transform.parent = GameObject.Find("Canvas").transform;

            // 냥 생성.
            nyang = Instantiate(NyangManager.Instance.nyangPrefabDic[101], NyangManager.Instance.nyangPositionDic[NyangPosition.E], Quaternion.identity).GetComponent<Nyang>();
            UIManager.instance.ResizeAndRepositionObject(nyang.gameObject, false);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => nyang.State == NyangState.Order);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;
            shadow.SetActive(false);
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(2);
            AngryGuageOn();
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);

        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(3);
            RecipeOpen(401, 502, 602);
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(4);
            OpenMeatSelectPanel();
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => CookManager.instance.isMeatCooking);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;

        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(5);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            //yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            Coroutine_00 = HandDownDown(tutorial.GetChild(4).gameObject);
            StartCoroutine(Coroutine_00);
            yield return new WaitUntil(() => (CookManager.instance.cookFood.step) == CookStep.Turn07);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            StopCoroutine(Coroutine_00);
            isTwinkling = false;
            isClickSkip = true;
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(6);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(7);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => (CookManager.instance.cookFood.powder) != null);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(8);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => (CookManager.instance.cookFood.sauce) != null);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;
        }
        {
            // 새 튜토리얼 설정.
            SetTutorial(9);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => nyang.State != NyangState.Order);
            twinklingIcon.SetActive(false);
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;
        }
        {
            nyang.OutNyang();
            SetTutorial(10);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(11);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(12);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(13);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        tutorialMainScene.SetActive(false);
        MainScene.SetActive(true);
        MainUI.SetActive(true);
        MainObjects.SetActive(true);
        if (tutorial) tutorial.gameObject.SetActive(false);

        GameManager.Instance.IsTutorial = false;
        EventEndTutorial();
    }

    private IEnumerator TanningTutorial()
    {
        GameManager.Instance.IsTutorial = true;
        defaultTanningTipNyang.SetActive(false);
        {
            isClickSkip = true;
            SetTutorial(14);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(15);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(16);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            TanningManager.instance.switch01 = true;
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => (!isTwinkling && !TanningManager.instance.switch01));
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
        }
        {
            SetTutorial(17);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            TanningManager.instance.switch02 = true;
            yield return new WaitUntil(() => !isScripting);
            isClickSkip = false;
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => (!TanningManager.instance.switch02));
            StopCoroutine(Coroutine_Twinkling);
            isTwinkling = false;
            isClickSkip = true;
        }
        {
            SetTutorial(18);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(19);

            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(20);
            TanningManager.instance.switch03 = true;
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(21);
            TanningManager.instance.switch04 = true;
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
        }
        {
            SetTutorial(22);
            TanningManager.instance.switch05 = true;
            // Scripting, Twinkling.
            StartCoroutine(Coroutine_Scripting);
            yield return new WaitUntil(() => !isScripting);
            StartCoroutine(Coroutine_Twinkling);
            yield return new WaitUntil(() => !isTwinkling);
            TanningManager.instance.switch06 = true;
            tutorial.gameObject.SetActive(false);
            UIManager.instance.AngryGuage.SetActive(false);
        }
        TanningManager.instance.switch08 = true;
        defaultTanningTipNyang.SetActive(true);
        GameManager.Instance.IsTutorial = false;
    }

    // SetTutorial: 튜토리얼 재설정, 스크립트 및 아이콘 갱신.
    private void SetTutorial(int index) {
        // 기존 튜토리얼 장면 해제.
        if (tutorial) tutorial.gameObject.SetActive(false);
        // 새 튜토리얼 장면 설정.
        tutorial = this.transform.GetChild(index);
        tutorial.gameObject.SetActive(true);
        // 스크립트 갱신.
        script = tutorial.GetChild(2).GetComponent<Text>();
        // 아이콘 갱신.
        twinklingIcon = tutorial.GetChild(3).gameObject;
        // 코루틴 갱신.
        Coroutine_Scripting = Scripting();
        Coroutine_Twinkling = Twinkling();
    }

    // Twinkling: 노란 아이콘을 반짝인다. 클릭시 스킵.
    private IEnumerator Twinkling() {
        isTwinkling = true;
        while (isTwinkling) {
            twinklingIcon.SetActive(true);
            yield return new WaitForSeconds(twinklingDelayTime_On);
            twinklingIcon.SetActive(false);
            yield return new WaitForSeconds(twinklingDelayTime_Off);
        }
        isTwinkling = false;
    }

    // Scripting: 대사를 한 글자씩 출력, 클릭시 스킵(즉시 전체출력)
    private IEnumerator Scripting() {
        isScripting = true;
        script.text = StringDataObject.GetStringData(script.GetComponent<TextLocalizer>().StringIndex);
        yield return null;
        originScript = script.text;
        builder = new StringBuilder(script.text);
        builder.Remove(0, builder.Length);
        builder.Append(script.text);
        int endIndex = 0;
        script.text = "";
        while (isScripting && script.text != originScript) {
            script.text = builder.ToString(0, endIndex);
            yield return new WaitForSeconds(printDelayTime);
            if (endIndex >= builder.Length) {
                endIndex = builder.Length;
                break;
            }
            ++endIndex;
        }
        isScripting = false;
    }

    // HandDownDown
    private IEnumerator HandDownDown(GameObject obj) {
        // -350 -> -500
        float time = 0.5f;
        float t = 0;
        float posy = -350;
        while (true) {
            while (Mathf.Abs(posy + 500) >= 0.1f) {
                posy = Mathf.Lerp(-350, -500, t / time);
                t += Time.deltaTime;
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posy);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -350);
            t = 0;
            posy = -350;
        }

    }
    #region OneOneGoodText
    StringBuilder builder = null;
    bool bPlay = false;

    private void ScriptingText(string _script) {
        builder = new StringBuilder(_script);
        builder.Remove(0, builder.Length);
        builder.Append(_script);
        Coroutine_Scripting = OneOneTextGood(printDelayTime);
        OneOneTextGoodPlay();
    }
    private void OneOneTextGoodPlay() {
        bPlay = true;
        StartCoroutine(Coroutine_Scripting);
    }
    private void OneOneTextGoodStop(bool bForce = false) {
        bPlay = false;
        if (bForce)
            StopCoroutine(Coroutine_Scripting);
    }
    private IEnumerator OneOneTextGood(float delay) {
        int endIndex = 0;
        while (bPlay) {
            script.text = builder.ToString(0, endIndex);
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

    #region IngredientSelect
    // IngredientSelect Panel: 재료 선택 창.
    [Header("MeatSelectPanel")]
    [Space(20)]
    public GameObject meatSelectPanel;
    public GameObject meatSelect_nextButton;
    public GameObject meatSelect_prevButton;
    public GameObject meatSelect_Menu01;
    public GameObject meatSelect_Menu02;
    public GameObject meatSelect_Menu03;
    private List<Ingredient> meatSelect_meatList;
    private int meatSelect_listMaxPage;
    private int meatSelect_listCurrentPage;
    private Ingredient meatSelect_menu01;
    private Ingredient meatSelect_menu02;
    private Ingredient meatSelect_menu03;
    [Header("SauceSelectPanel")]
    public GameObject sauceSelectPanel;
    public GameObject sauceSelect_nextButton;
    public GameObject sauceSelect_prevButton;
    public GameObject sauceSelect_Menu01;
    public GameObject sauceSelect_Menu02;
    public GameObject sauceSelect_Menu03;
    public GameObject sauceSelect_Menu04;
    public GameObject sauceSelect_Menu05;
    public GameObject sauceSelect_Menu06;
    private List<Ingredient> sauceSelect_sauceList;
    private List<Ingredient> sauceSelect_powderList;
    private int sauceSelect_listMaxPage;
    private int sauceSelect_listCurrentPage;
    private Ingredient sauceSelect_menu01;
    private Ingredient sauceSelect_menu02;
    private Ingredient sauceSelect_menu03;
    private Ingredient sauceSelect_menu04;
    private Ingredient sauceSelect_menu05;
    private Ingredient sauceSelect_menu06;
    // OpenMeatSelectPanel: 고기 선택 창 띄우기.
    public void OpenMeatSelectPanel() {
        // 창 띄우기.
        meatSelectPanel.SetActive(true);
        // 보여줄 리스트 설정.
        meatSelect_meatList = GetAvailableIngredientList(IngredientType.Meat);
        // 페이지 설정.
        meatSelect_listCurrentPage = 0;
        meatSelect_listMaxPage = GetListMaxPage(meatSelect_meatList);
        // 페이지 보여주기.
        ShowMeatSelectList(meatSelect_listCurrentPage);
    }
    // CloseMeatSelectPanel: 고기 선택 창 닫기.
    public void CloseMeatSelectPanel() {
        // 창 닫기.
        meatSelectPanel.SetActive(false);
    }
    // SetMeatSelectListPage: page에 해당하는 메뉴 세개 선택.
    public void SetMeatSelectListPage(int page) {
        meatSelect_menu01 = meatSelect_meatList[3 * page];
        meatSelect_menu02 = meatSelect_meatList[3 * page + 1];
        meatSelect_menu03 = meatSelect_meatList[3 * page + 2];
    }
    // ShowMeatSelectList: 해당 페이지를 보여줌.
    public void ShowMeatSelectList(int page) {
        SetMeatSelectListPage(page);
        meatSelect_Menu01.transform.GetComponent<Image>().sprite = meatSelect_menu01.sprite_Icon;
        meatSelect_Menu01.transform.GetComponent<Image>().SetNativeSize();
        meatSelect_Menu02.transform.GetComponent<Image>().sprite = meatSelect_menu02.sprite_Icon;
        meatSelect_Menu02.transform.GetComponent<Image>().SetNativeSize();
        meatSelect_Menu03.transform.GetComponent<Image>().sprite = meatSelect_menu03.sprite_Icon;
        meatSelect_Menu03.transform.GetComponent<Image>().SetNativeSize();
    }
    // Next/PrevMeatSelectPanel: 다음/이전 페이지를 보여줌.
    public void NextMeatSelectPanel() {
        if (meatSelect_listCurrentPage < meatSelect_listMaxPage) {
            meatSelect_listCurrentPage++;
            ShowMeatSelectList(meatSelect_listCurrentPage);
        }
    }
    public void PrevMeatSelectPanel() {
        if (meatSelect_listCurrentPage > 0) {
            meatSelect_listCurrentPage--;
            ShowMeatSelectList(meatSelect_listCurrentPage);
        }
    }
    // SelectMeat: 고기 선택.
    public void SelectMeat(int index) {
        // 이미 조리하고 있는 고기가 있으면 선택할 수 없다.
        if (CookManager.instance.cookFood) return;
        // 고기 선택 창을 닫는다.
        CloseMeatSelectPanel();
        // 선택한 고기를 CookManager에 넘겨줌.
        if (index == 1) CookManager.instance.SelectMeat(meatSelect_menu01);
        else if (index == 2) CookManager.instance.SelectMeat(meatSelect_menu02);
        else CookManager.instance.SelectMeat(meatSelect_menu03);
    }


    // OpenSauceSelectPanel: 소스 선택 창 띄우기.
    public void OpenSauceSelectPanel() {
        // 창 띄우기.
        sauceSelectPanel.SetActive(true);
        // 보여줄 리스트 설정.
        sauceSelect_sauceList = GetAvailableIngredientList(IngredientType.Sauce);
        sauceSelect_powderList = GetAvailableIngredientList(IngredientType.Powder);
        // 페이지 설정.
        sauceSelect_listCurrentPage = 0;
        sauceSelect_listMaxPage = GetListMaxPage(sauceSelect_sauceList, sauceSelect_powderList);
        // 페이지 보여주기.
        ShowSauceSelectList(sauceSelect_listCurrentPage);
    }
    // CloseSauceSelectPanel: 소스 선택 창 닫기.
    public void CloseSauceSelectPanel() {
        // 창 닫기.
        sauceSelectPanel.SetActive(false);
    }
    // SetSauceSelectListPage: page에 해당하는 메뉴 세개/세개 선택.
    public void SetSauceSelectListPage(int page) {
        sauceSelect_menu01 = sauceSelect_powderList[3 * page];
        sauceSelect_menu02 = sauceSelect_powderList[3 * page + 1];
        sauceSelect_menu03 = sauceSelect_powderList[3 * page + 2];
        sauceSelect_menu04 = sauceSelect_sauceList[3 * page];
        sauceSelect_menu05 = sauceSelect_sauceList[3 * page + 1];
        sauceSelect_menu06 = sauceSelect_sauceList[3 * page + 2];
    }
    // ShowSauceSelectList: 해당 페이지를 보여줌.
    public void ShowSauceSelectList(int page) {
        SetSauceSelectListPage(page);
        sauceSelect_Menu01.transform.GetComponent<Image>().sprite = sauceSelect_menu01.sprite_Icon;
        sauceSelect_Menu02.transform.GetComponent<Image>().sprite = sauceSelect_menu02.sprite_Icon;
        sauceSelect_Menu03.transform.GetComponent<Image>().sprite = sauceSelect_menu03.sprite_Icon;
        sauceSelect_Menu04.transform.GetComponent<Image>().sprite = sauceSelect_menu04.sprite_Icon;
        sauceSelect_Menu05.transform.GetComponent<Image>().sprite = sauceSelect_menu05.sprite_Icon;
        sauceSelect_Menu06.transform.GetComponent<Image>().sprite = sauceSelect_menu06.sprite_Icon;
    }
    // Next/PrevSauceSelectPanel: 다음/이전 페이지를 보여줌.
    public void NextSauceSelectPanel() {
        if (sauceSelect_listCurrentPage < sauceSelect_listMaxPage) {
            sauceSelect_listCurrentPage++;
            ShowSauceSelectList(sauceSelect_listCurrentPage);
        }
    }
    public void PrevSauceSelectPanel() {
        if (sauceSelect_listCurrentPage > 0) {
            sauceSelect_listCurrentPage--;
            ShowSauceSelectList(sauceSelect_listCurrentPage);
        }
    }
    // SelectPowder/Sauce: 가루/소스 선택. (순서: 가루 > 소스)
    public void SelectPowder(int index) {
        // 이미 가루를 선택했다면 선택할 수 없다.
        if (CookManager.instance.cookFood.powder) return;
        // 선택한 가루를 cookFood에 넘겨줌.
        if (index == 1) CookManager.instance.cookFood.SetPowder(sauceSelect_menu01);
        else if (index == 2) CookManager.instance.cookFood.SetPowder(sauceSelect_menu02);
        else CookManager.instance.cookFood.SetPowder(sauceSelect_menu03);
    }
    public void SelectSauce(int index) {
        // 가루를 선택하지 않았다면 선택할 수 없다.
        if (!CookManager.instance.cookFood.powder) return;
        // 이미 소스를 선택했다면 선택할 수 없다.
        if (CookManager.instance.cookFood.sauce) return;
        // 선택한 소스를 cookFood에 넘겨줌.
        if (index == 1) CookManager.instance.cookFood.SetSauce(sauceSelect_menu04);
        else if (index == 2) CookManager.instance.cookFood.SetSauce(sauceSelect_menu05);
        else CookManager.instance.cookFood.SetSauce(sauceSelect_menu06);
    }
    // GetAvailableIngredientList: 사용 가능한 재료 리스트를 받는다.
    private List<Ingredient> GetAvailableIngredientList(IngredientType type) {
        List<Ingredient> list = new List<Ingredient>();

        Dictionary<int, Ingredient> dictionary = new Dictionary<int, Ingredient>();

        switch (type) {
            case IngredientType.Meat:
                dictionary = IngredientManager.instance.meatDic;
                break;
            case IngredientType.Powder:
                dictionary = IngredientManager.instance.powderDic;
                break;
            case IngredientType.Sauce:
                dictionary = IngredientManager.instance.sauceDic;
                break;
        }
        foreach (int i in dictionary.Keys)
            if (dictionary[i].IsAvailable)
                list.Add(dictionary[i]);
        return list;
    }
    // GetListMaxPage: 재료 리스트의 마지막 페이지를 받는다.
    public int GetListMaxPage(List<Ingredient> list) {
        return Mathf.CeilToInt(list.Count / 3.0f) - 1;
    }
    public int GetListMaxPage(List<Ingredient> list1, List<Ingredient> list2, bool isThreeMenu = false) {
        if (!isThreeMenu) {
            int count = (list1.Count >= list2.Count) ? list1.Count : list2.Count;
            return Mathf.CeilToInt(count / 3.0f) - 1;
        }
        else
            return Mathf.CeilToInt((list1.Count + list2.Count) / 3.0f) - 1;
    }
    #endregion

    #region Recipe
    [Header("Recipe")]
    [Space(20)]
    public GameObject recipePanel;
    public GameObject recipe_Meat;
    public GameObject recipe_Powder;
    public GameObject recipe_Sauce;
    // Recipe
    public void RecipeOpen(int meatIndex, int powderIndex, int sauceIndex) {
        // 레시피 창을 띄움.
        recipePanel.SetActive(true);
        // 레시피 이미지.
        recipe_Meat.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.meatDic[meatIndex].sprite_Icon;
        recipe_Powder.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.powderDic[powderIndex].sprite_Icon;
        recipe_Sauce.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.sauceDic[sauceIndex].sprite_Icon;
    }
    public void RecipeClose() {
        // 레시피 창을 닫음.
        recipePanel.SetActive(false);
    }

    #endregion

    #region AngryGuage

    public GameObject AngryGuage;

    public void AngryGuageOn() {
        AngryGuage.SetActive(true);
        AngryGuage.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(28, 34);
    }
    public void AngryGuageOff() {
        Debug.Log("AngryGuageOff");
        AngryGuage.SetActive(false);
    }
    public void SetAngryGuage(float f) {
        AngryGuage.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(28, 34 + 56 * (f / TimeManager.Instance.waitingTime));
    }

    #endregion
}