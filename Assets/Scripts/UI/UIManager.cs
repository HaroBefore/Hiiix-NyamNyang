using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    // MainGame.
    [Header("MainGame")]
    public GameObject Main_Objects;
    public GameObject Main_Scene;
    public GameObject Main_UI;
    public GameObject AngryGuage;

    // Date/Time: 날짜 및 시간.
    [Header("Date/Time")]
    public GameObject Calender;
    public GameObject Watch;
    public GameObject BuffCoolTimer;
    // Money: 현재 가지고 있는 돈.
    [Header("Money")]
    public GameObject Money;
    public GameObject NyangMoney;
    // Recipe Panel: 주문이 들어오면 창을 띄운다.
    [Header("Recipe")]
    [Space(20)]
    public GameObject recipePanel;
    public GameObject recipe_Meat;
    public GameObject recipe_Powder;
    public GameObject recipe_Sauce;
    [Header("IngredientSelect_Animation")]
    public GameObject ingredientSelect_Animation_Object;
    private Ingredient selectedIngredient;
    private IEnumerator Coroutine_IngredientAnimation;
    [Header("MeatSelectPanel")]
    [Space(20)]
    public GameObject meatSelectPanel;
    public GameObject meatSelect_nextButton;
    public GameObject meatSelect_prevButton;
    public GameObject meatSelect_Menu01;
    public GameObject meatSelect_Menu02;
    public GameObject meatSelect_Menu03;
    public Text meatSelect_Menu01_Count;
    public Text meatSelect_Menu02_Count;
    public Text meatSelect_Menu03_Count;
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
    public Text sauceSelect_Menu01_Count;
    public Text sauceSelect_Menu02_Count;
    public Text sauceSelect_Menu03_Count;
    public Text sauceSelect_Menu04_Count;
    public Text sauceSelect_Menu05_Count;
    public Text sauceSelect_Menu06_Count;
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
    // IngredientStore
    [Header("IngredientStore")]
    [Space(20)]
    public GameObject ingredientShopPanel;
    public GameObject ingredientShopButton;
    public GameObject ingredientShop_meat_Menu01;
    public GameObject ingredientShop_meat_Menu02;
    public GameObject ingredientShop_meat_Menu03;
    public GameObject ingredientShop_sauce_Menu01;
    public GameObject ingredientShop_sauce_Menu02;
    public GameObject ingredientShop_sauce_Menu03;
    private List<Ingredient> ingredientShop_meatList;
    private List<Ingredient> ingredientShop_powderList;
    private List<Ingredient> ingredientShop_sauceList;
    private int ingredientShop_meatListMaxPage;
    private int ingredientShop_sauceListMaxPage;
    private int ingredientShop_meatListCurrentPage;
    private int ingredientShop_sauceListCurrentPage;
    private Ingredient ingredientShop_meat_menu01;
    private Ingredient ingredientShop_meat_menu02;
    private Ingredient ingredientShop_meat_menu03;
    private Ingredient ingredientShop_sauce_menu01;
    private Ingredient ingredientShop_sauce_menu02;
    private Ingredient ingredientShop_sauce_menu03;
    // IngredientPurchasePopup
    [Header("IngredientPurchasePopup")]
    [Space(20)]
    public GameObject IngredientPurchasePopup;
    public GameObject IngredientPurchase_Info_Icon;
    public GameObject IngredientPurchase_WarnningSign;
    public GameObject IngredientPurchase_Price;
    public GameObject IngredientPurchase_Amount;
    public GameObject IngredientPurchase_PurchaseButton;
    public GameObject IngredientPurchase_noPurchaseButton;
    private Ingredient IngredientPurchase_Item;
    private int IngredientPurchase_amount;
    // DecoShop
    [Header("DecoShop")]
    [Space(20)]
    public GameObject DecoShop_Panel;
    public GameObject DecoShop_Button;
    public GameObject DecoShop_Category;
    public GameObject DecoShop_Menu01;
    public GameObject DecoShop_Menu02;
    public GameObject DecoShop_Menu03;
    private DecoType DecoShop_currentCategory;
    private int DecoShop_ListCurrentPage;
    private int DecoShop_ListMaxPage;
    private Deco DecoShop_menu01;
    private Deco DecoShop_menu02;
    private Deco DecoShop_menu03;
    private Deco[] originDeco;
    private Deco[] prevDeco;
    private bool[] needUndo;
    // DecoPurchasePopup
    [Header("DecoPurchasePopup")]
    [Space(20)]
    public GameObject DecoPurchasePopup;
    public GameObject DecoPurchasePopup_Info_Icon;
    public GameObject DecoPurchasePopup_WarnningSign;
    public GameObject DecoPurchasePopup_Price;
    public GameObject DecoPurchasePopup_PurchaseButton;
    public GameObject DecoPurchasePopup_noPurchaseButton;
    private Deco DecoPurchase_Item;
    // MiniGame
    [Header("MiniGame")]
    [Space(20)]
    // MiniGame_Tanning.
    public GameObject MiniGame_Tanning_Popup;
    public GameObject MiniGame_Tanning_UI;
    // NyangList
    [Header("NyangList")]
    public GameObject NyangListPanel;
    public GameObject NyangList01;
    public GameObject NyangList02;
    public GameObject NyangList03;
    public GameObject NyangList04;
    public GameObject NyangList05;
    public GameObject NyangList06;
    public Sprite nyangList_hideSprite;
    private List<Nyang> nyangList;
    private int nyangList_currentPage;
    private int nyangList_maxPage;
    private Nyang nyangList01;
    private Nyang nyangList02;
    private Nyang nyangList03;
    private Nyang nyangList04;
    private Nyang nyangList05;
    private Nyang nyangList06;
    public GameObject NyangStoryPopup;

    [Header("Option")]
    public GameObject OptionPanel;

    [Header("BuffAds")]
    public GameObject buffButton;
    public GameObject buffButton_Pushed;
    public GameObject BuffPopup;
    public GameObject BuffCoolPopup;
    public GameObject BuffNyang;

    [Header("OpenBetaPopup")]
    public GameObject OpenBetaPopup;

    private TimeManager timeManager;
    private AudioManager audioManager;
    private InputManager inputManager;

    public float size { get; protected set; }
    public void ResizeAndRepositionObject(GameObject obj, bool isReposition = true) {
        obj.transform.localScale *= size;
        if (isReposition) obj.transform.position *= size;
    }
    void Awake() {
        if (!instance) instance = this;
        timeManager = FindObjectOfType<TimeManager>();
        audioManager = FindObjectOfType<AudioManager>();
        inputManager = FindObjectOfType<InputManager>();
        float a = Screen.width;
        float b = Screen.height;
        size = 16 * a / 9 / b;
        if (((float)Screen.width / (float)Screen.height) > 0.5625) size = 1;
        BorderManager bdm = FindObjectOfType<BorderManager>();

        bdm.nyangPositionA *= size;
        bdm.nyangPositionB *= size;
        bdm.nyangPositionC *= size;
        bdm.nyangPositionD *= size;
        bdm.nyangPositionE *= size;
        bdm.nyangPositionF *= size;

        bdm.customerSeatPosition *= size;
        bdm.stovePosition *= size;
    }

    void Update() {

        
        DEBUG_UPDATE();
    }

    // DEBUG
    private void DEBUG_UPDATE() {
        if (Input.GetKeyDown(KeyCode.P)) {
            timeManager.SetTime(890);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            timeManager.SetTime(1190);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            CookManager.instance.cookFood.Cook();
        }

        if (Input.GetKeyDown(KeyCode.I)) { timeManager.Day = 15; }

        if (Input.GetKeyDown(KeyCode.Z)) { NyangManager.instance.orderNyang.waitingTime += 99999; }
        if (Input.GetKeyDown(KeyCode.G)) GoldManager.instance.CurrentGold += 9999999;
        if (Input.GetKeyDown(KeyCode.H)) GoldManager.instance.CurrentGold -= 10000;
        if (Input.GetKeyDown(KeyCode.Keypad1)) timeManager.Day = 14;
        if (Input.GetKeyDown(KeyCode.Keypad2)) timeManager.Day = 15;
        if (Input.GetKeyDown(KeyCode.Keypad3)) timeManager.Day = 3;
    }
    public void DEBUG_BUTTON_CAT_GOAWAY() {
        List<Nyang> nyangList = NyangManager.instance.nyangList;
        for (int i = 0; i < nyangList.Count; i++) {
            Destroy(nyangList[i].gameObject);
            NyangManager.instance.nyangList.RemoveAt(i);
        }
    }
    #region Buff
    public void BuffOn() {
        BackgroundManager.instance.SetBuff();
        BuffNyang.SetActive(true);
    }
    public void BuffOff() {
        if (timeManager.timeType == TimeType.PMOpenTime ||
            timeManager.timeType == TimeType.PM ||
            timeManager.timeType == TimeType.Closetime)
            BackgroundManager.instance.SetPM();
        else BackgroundManager.instance.SetAM();
        buffButton_Pushed.SetActive(false);
        buffButton.SetActive(true);
        BuffNyang.SetActive(false);
    }

    #endregion

    #region BuffAds
    public void OpenBuffPopup() {
        if (ingredientShopPanel.activeSelf || DecoShop_Panel.activeSelf || BuffPopup.activeSelf) return;

        if (BossManager.instance.isBossStage) return;

        AudioManager.instance?.Play(AudioManager.instance.button01);
        buffButton.SetActive(false);
        buffButton_Pushed.SetActive(true);
        TipManager.instance.CloseTip(TipType.Buff);
        if (timeManager.IsBuffAvailable()) {
            OpenBuffAvailablePopup();
        }
        else {
            OpenBuffCoolPopup();
        }
    }
    private void OpenBuffAvailablePopup() {
        if (DecoShop_Panel.activeSelf) return;
        if (meatSelectPanel.activeSelf) meatSelectPanel.SetActive(false);
        if (sauceSelectPanel.activeSelf) sauceSelectPanel.SetActive(false);

        BuffPopup.SetActive(true);
    }
    private void OpenBuffCoolPopup() {
        if (DecoShop_Panel.activeSelf) return;
        if (meatSelectPanel.activeSelf) meatSelectPanel.SetActive(false);
        if (sauceSelectPanel.activeSelf) sauceSelectPanel.SetActive(false);

        BuffCoolPopup.SetActive(true);
    }

    public void ToggleBuffPopup() {
        if (BuffCoolPopup.activeSelf) {
            BuffPopup.SetActive(true);
            BuffCoolPopup.SetActive(false);
        }
    }
    public void CloseBuffPopup() {
        AudioManager.instance?.Play(AudioManager.instance.button01);
        if (BuffPopup.activeSelf) {
            BuffPopup.SetActive(false);
            buffButton_Pushed.SetActive(false);
            buffButton.SetActive(true);
        }
        if (BuffCoolPopup.activeSelf) {
            BuffCoolPopup.SetActive(false);
            buffButton_Pushed.SetActive(false);
            buffButton.SetActive(true);
        }
    }
    #endregion

    #region Recipe

    // Recipe
    public void RecipeOpen(int meatIndex, int powderIndex, int sauceIndex) {
        if (BossManager.instance.isBossStage) {
            BossManager.instance.bossRecipeBox.SetActive(true);
        }
        else {
            // 레시피 창을 띄움.
            recipePanel.SetActive(true);
            // 레시피 이미지.
            recipe_Meat.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.meatDic[meatIndex].sprite_Icon;
            recipe_Powder.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.powderDic[powderIndex].sprite_Icon;
            recipe_Sauce.transform.GetComponent<SpriteRenderer>().sprite = IngredientManager.instance.sauceDic[sauceIndex].sprite_Icon;
        }
    }
    public void RecipeClose() {
        if (BossManager.instance.isBossStage) {
            BossManager.instance.bossRecipeBox.SetActive(false);
        }
        else {
            // 레시피 창을 닫음.
            recipePanel.SetActive(false);
        }
    }

    #endregion

    #region NyangMoney

    public void ShowNyangMoneyForSeconds(int price, float seconds) {
        StartCoroutine(ShowNyangMoney(price, seconds));
    }
    private IEnumerator ShowNyangMoney(int price, float seconds) {
        NyangMoney.SetActive(true);
        NyangMoney.transform.GetChild(1).GetComponent<Text>().text = price.ToString();
        yield return new WaitForSeconds(seconds);
        NyangMoney.SetActive(false);


    }

    #endregion

    #region IngredientSelect

    // OpenMeatSelectPanel: 고기 선택 창 띄우기.
    public void OpenMeatSelectPanel() {
        if (DecoShop_Panel.activeSelf || BuffPopup.activeSelf) return;
        audioManager?.Play(audioManager.box_open, 1f);
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
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
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
        Image image = meatSelect_Menu01.transform.GetChild(0).GetComponent<Image>();
        image.sprite = meatSelect_menu01.sprite_Icon;
        image.SetNativeSize();
        if (meatSelect_menu01.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = meatSelect_Menu02.transform.GetChild(0).GetComponent<Image>();
        image.sprite = meatSelect_menu02.sprite_Icon;
        image.SetNativeSize();
        if (meatSelect_menu02.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = meatSelect_Menu03.transform.GetChild(0).GetComponent<Image>();
        image.sprite = meatSelect_menu03.sprite_Icon;
        image.SetNativeSize();
        if (meatSelect_menu03.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        SetMeatCount();
    }
    private void SetMeatCount() {
        // Count.
        meatSelect_Menu01_Count.text = meatSelect_menu01.Count.ToString();
        meatSelect_Menu02_Count.text = meatSelect_menu02.Count.ToString();
        meatSelect_Menu03_Count.text = meatSelect_menu03.Count.ToString();
    }
    // Next/PrevMeatSelectPanel: 다음/이전 페이지를 보여줌.
    public void NextMeatSelectPanel() {
        if (meatSelect_listCurrentPage < meatSelect_listMaxPage)
            meatSelect_listCurrentPage++;
        else meatSelect_listCurrentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowMeatSelectList(meatSelect_listCurrentPage);
    }
    public void PrevMeatSelectPanel() {
        if (meatSelect_listCurrentPage > 0)
            meatSelect_listCurrentPage--;
        else meatSelect_listCurrentPage = meatSelect_listMaxPage;
        audioManager?.Play(audioManager.button01);
        ShowMeatSelectList(meatSelect_listCurrentPage);
    }
    // SelectMeat: 고기 선택.
    public void SelectMeat(int index) {
        // 이미 조리하고 있는 고기가 있으면 선택할 수 없다.
        if (CookManager.instance.cookFood) return;
        // 주문하고 있는 냥이가 없으면 선택 불가.
        if (!NyangManager.instance.orderNyang) return;
        // 재료 상점이 열려있는 경우 선택할 수 없다.
        if (ingredientShopPanel.activeSelf) return;
        // 고기 설정.
        Ingredient meat = null;
        if (index == 1) meat = meatSelect_menu01;
        else if (index == 2) meat = meatSelect_menu02;
        else meat = meatSelect_menu03;
        selectedIngredient = meat;
        // 재고 설정.
        if (meat.Count <= 0) {
            OpenIngredientShopSelectPanel();
            return;
        }
        meat.Count--;
        // 고기 선택 창을 닫는다.
        CloseMeatSelectPanel();
        // 선택한 고기를 CookManager에 넘겨줌.
        CookManager.instance.SelectMeat(meat);
    }


    // OpenSauceSelectPanel: 소스 선택 창 띄우기.
    public void OpenSauceSelectPanel() {
        if (DecoShop_Panel.activeSelf || BuffPopup.activeSelf) return;
        audioManager?.Play(audioManager.box_open, 1f);
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
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
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

        Image image = sauceSelect_Menu01.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu01.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu01.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = sauceSelect_Menu02.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu02.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu02.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = sauceSelect_Menu03.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu03.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu03.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = sauceSelect_Menu04.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu04.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu04.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = sauceSelect_Menu05.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu05.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu05.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        image = sauceSelect_Menu06.transform.GetChild(0).GetComponent<Image>();
        image.sprite = sauceSelect_menu06.sprite_Icon;
        image.SetNativeSize();
        if (sauceSelect_menu06.Count <= 0) image.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
        else image.color = new Color(1, 1, 1, 1);
        SetSauceCount();
    }
    private void SetSauceCount() {
        // Count.
        sauceSelect_Menu01_Count.text = sauceSelect_menu01.Count.ToString();
        sauceSelect_Menu02_Count.text = sauceSelect_menu02.Count.ToString();
        sauceSelect_Menu03_Count.text = sauceSelect_menu03.Count.ToString();
        sauceSelect_Menu04_Count.text = sauceSelect_menu04.Count.ToString();
        sauceSelect_Menu05_Count.text = sauceSelect_menu05.Count.ToString();
        sauceSelect_Menu06_Count.text = sauceSelect_menu06.Count.ToString();
    }
    // Next/PrevSauceSelectPanel: 다음/이전 페이지를 보여줌.
    public void NextSauceSelectPanel() {
        if (sauceSelect_listCurrentPage < sauceSelect_listMaxPage)
            sauceSelect_listCurrentPage++;
        else sauceSelect_listCurrentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowSauceSelectList(sauceSelect_listCurrentPage);
    }
    public void PrevSauceSelectPanel() {
        if (sauceSelect_listCurrentPage > 0)
            sauceSelect_listCurrentPage--;
        else sauceSelect_listCurrentPage = sauceSelect_listMaxPage;
        audioManager?.Play(audioManager.button01);
        ShowSauceSelectList(sauceSelect_listCurrentPage);
    }
    // SelectPowder/Sauce: 가루/소스 선택. (순서: 가루 > 소스)
    public void SelectPowder(int index) {
        // 재료 상점이 열려있는 경우 선택할 수 없다.
        if (ingredientShopPanel.activeSelf) return;
        // 주문하는 냥이가 없다면 선택할 수 없다.
        if (!NyangManager.instance.orderNyang) return;
        // 요리가 올려져있지 않다면 선택할 수 없다.
        if (!CookManager.instance.cookFood) return;
        // 이미 가루를 선택했다면 선택할 수 없다.
        if (CookManager.instance.cookFood.powder) return;
        // 다 안구워지면 선택불가.
        if (CookManager.instance.cookFood.step != CookStep.Turn07) return;
        // 가루 설정.
        Ingredient powder = null;
        if (index == 1) powder = sauceSelect_menu01;
        else if (index == 2) powder = sauceSelect_menu02;
        else powder = sauceSelect_menu03;
        selectedIngredient = powder;
        // 재고 설정.
        if (powder.Count <= 0) {
            OpenIngredientShopSelectPanel();
            return;
        }
        powder.Count--;
        // 선택한 가루를 cookFood에 넘겨줌.
        CookManager.instance.cookFood.SetPowder(powder);
        // 애니메이션 재생.
        StopIngredientSelectAnimation();
        StartIngredientSelectAnimation();
    }
    public void SelectSauce(int index) {
        // 재료 상점이 열려있는 경우 선택할 수 없다.
        if (ingredientShopPanel.activeSelf) return;
        // 주문하는 냥이가 없다면 선택할 수 없다.
        if (!NyangManager.instance.orderNyang) return;
        // 요리가 올려져있지 않다면 선택할 수 없다.
        if (!CookManager.instance.cookFood) return;
        // 이미 소스를 선택했다면 선택할 수 없다.
        if (CookManager.instance.cookFood.sauce) return;
        // 다 안구워지면 선택불가.
        if (CookManager.instance.cookFood.step != CookStep.Turn07) return;
        // 소스 설정.
        Ingredient sauce = null;
        if (index == 1) sauce = sauceSelect_menu04;
        else if (index == 2) sauce = sauceSelect_menu05;
        else sauce = sauceSelect_menu06;
        selectedIngredient = sauce;
        // 재고 설정.
        if (sauce.Count <= 0) {
            OpenIngredientShopSelectPanel();
            return;
        }
        sauce.Count--;
        // 선택한 소스를 cookFood에 넘겨줌.
        CookManager.instance.cookFood.SetSauce(sauce);
        // 애니메이션 재생.
        StopIngredientSelectAnimation();
        StartIngredientSelectAnimation();
    }

    private void StartIngredientSelectAnimation() {
        Coroutine_IngredientAnimation = IngredientSelectAnimation();
        StartCoroutine(Coroutine_IngredientAnimation);
    }
    public void StopIngredientSelectAnimation() {
        if (Coroutine_IngredientAnimation != null) StopCoroutine(Coroutine_IngredientAnimation);
        ingredientSelect_Animation_Object.SetActive(false);
    }
    private IEnumerator IngredientSelectAnimation() {
        Image image = ingredientSelect_Animation_Object.GetComponent<Image>();
        image.sprite = selectedIngredient.sprite_Icon;
        ingredientSelect_Animation_Object.SetActive(true);
        image.color = new Color(1, 1, 1, 1);
        float animationTime = 0.2f;
        yield return new WaitForSeconds(animationTime);

        float per = 0;
        float perTime = 0;
        while (per < 1) {
            perTime += Time.deltaTime;
            per = perTime / animationTime;
            image.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, per));
            yield return null;
        }
        ingredientSelect_Animation_Object.SetActive(false);
    }
    #endregion

    #region IngredientShop

    // OpenIngredientShopSelectPanel: 재료 상점 창 띄우기.
    public void OpenIngredientShopSelectPanel() {
        //if (meatSelectPanel.activeSelf || sauceSelectPanel.activeSelf) return;
        if (BuffPopup.activeSelf) return;
        audioManager?.Play(audioManager.box_open, 1f);
        audioManager?.PauseCookMeat();
        if (DecoShop_Panel.activeSelf) CloseDecoShopSelectPanel();
        // 창 띄우기.
        Main_Objects.SetActive(false);
        Main_Scene.SetActive(false);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(false);
        ingredientShopPanel.SetActive(true);
        Calender.SetActive(false);
        TipManager.instance.HideTip();
        // 보여줄 리스트 설정.
        ingredientShop_meatList = GetAvailableIngredientList(IngredientType.Meat);
        ingredientShop_sauceList = GetAvailableIngredientList(IngredientType.Sauce);
        ingredientShop_powderList = GetAvailableIngredientList(IngredientType.Powder);
        // 페이지 설정.
        ingredientShop_meatListCurrentPage = 0;
        ingredientShop_sauceListCurrentPage = 0;
        ingredientShop_meatListMaxPage = GetListMaxPage(ingredientShop_meatList);
        ingredientShop_sauceListMaxPage = GetListMaxPage(ingredientShop_sauceList, ingredientShop_powderList, true);
        // 페이지 보여주기.
        ShowIngredientShopMeatList(ingredientShop_meatListCurrentPage);
        ShowIngredientShopSauceList(ingredientShop_sauceListCurrentPage);
        // 게임 시간 멈추기.
        timeManager.SetTime_Stop();
        timeManager.SetGameTime_Stop();
        // 버튼 색깔 회색으로.
        ingredientShopButton.transform.GetChild(0).GetComponent<Image>().color = new Color(197f / 255f, 198f / 255f, 200f / 255f, 1f);
    }
    // CloseIngredientShopPanel: 재료 상점 창 닫기.
    public void CloseIngredientShopPanel() {
        // 창 닫기.
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
        audioManager?.ResumeCookMeat();
        ingredientShopPanel.SetActive(false);
        Main_Objects.SetActive(true);
        Main_Scene.SetActive(true);
        Calender.SetActive(true);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(true);
        TipManager.instance.UnhideTip();
        timeManager.SetTime_Go();
        timeManager.SetGameTime_Go();
        ingredientShopButton.transform.GetChild(0).GetComponent<Image>().color = new Color(255f / 255f, 221f / 255f, 0f / 255f, 1f);
        OpenBoxOnCloseUI();
    }
    // SetIngredientShopMeat/SauceListPage: page에 해당하는 메뉴 세개 선택.
    public void SetIngredientShopMeatListPage(int page) {
        ingredientShop_meat_menu01 = ingredientShop_meatList[3 * page];
        ingredientShop_meat_menu02 = ingredientShop_meatList[3 * page + 1];
        ingredientShop_meat_menu03 = ingredientShop_meatList[3 * page + 2];
    }
    public void SetIngredientShopSauceListPage(int page) {
        if (page % 2 == 0) {
            int listPage = (int)(page * 1.5f);
            ingredientShop_sauce_menu01 = ingredientShop_powderList[listPage];
            ingredientShop_sauce_menu02 = ingredientShop_powderList[listPage + 1];
            ingredientShop_sauce_menu03 = ingredientShop_powderList[listPage + 2];
        }
        else {
            int listPage = (int)((page - 1) * 1.5f);
            ingredientShop_sauce_menu01 = ingredientShop_sauceList[listPage];
            ingredientShop_sauce_menu02 = ingredientShop_sauceList[listPage + 1];
            ingredientShop_sauce_menu03 = ingredientShop_sauceList[listPage + 2];
        }
    }
    // ShowIngredientShopMeat/SauceList: 해당 페이지를 보여줌.
    public void ShowIngredientShopMeatList(int page) {
        SetIngredientShopMeatListPage(page);
        ingredientShop_meat_Menu01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ingredientShop_meat_menu01.sprite_Market;
        ingredientShop_meat_Menu01.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_meat_menu01.price.ToString();
        ingredientShop_meat_Menu02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ingredientShop_meat_menu02.sprite_Market;
        ingredientShop_meat_Menu02.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_meat_menu02.price.ToString();
        ingredientShop_meat_Menu03.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ingredientShop_meat_menu03.sprite_Market;
        ingredientShop_meat_Menu03.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_meat_menu03.price.ToString();
    }
    public void ShowIngredientShopSauceList(int page) {
        SetIngredientShopSauceListPage(page);
        ingredientShop_sauce_Menu01.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = ingredientShop_sauce_menu01.sprite_Icon;
        ingredientShop_sauce_Menu01.transform.GetChild(0).GetChild(1).GetComponent<Image>().SetNativeSize();
        ingredientShop_sauce_Menu01.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_sauce_menu01.price.ToString();
        ingredientShop_sauce_Menu02.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = ingredientShop_sauce_menu02.sprite_Icon;
        ingredientShop_sauce_Menu02.transform.GetChild(0).GetChild(1).GetComponent<Image>().SetNativeSize();
        ingredientShop_sauce_Menu02.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_sauce_menu02.price.ToString();
        ingredientShop_sauce_Menu03.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = ingredientShop_sauce_menu03.sprite_Icon;
        ingredientShop_sauce_Menu03.transform.GetChild(0).GetChild(1).GetComponent<Image>().SetNativeSize();
        ingredientShop_sauce_Menu03.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ingredientShop_sauce_menu03.price.ToString();
    }
    // Next/PrevIngredientShopMeat/SaucePanel: 다음/이전 페이지를 보여줌.
    public void NextIngredientShopMeatPanel() {
        if (ingredientShop_meatListCurrentPage < ingredientShop_meatListMaxPage)
            ingredientShop_meatListCurrentPage++;
        else ingredientShop_meatListCurrentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowIngredientShopMeatList(ingredientShop_meatListCurrentPage);
    }
    public void PrevIngredientShopMeatPanel() {
        if (ingredientShop_meatListCurrentPage > 0)
            ingredientShop_meatListCurrentPage--;
        else ingredientShop_meatListCurrentPage = ingredientShop_meatListMaxPage;
        audioManager?.Play(audioManager.button01);
        ShowIngredientShopMeatList(ingredientShop_meatListCurrentPage);
    }
    public void NextIngredientShopSaucePanel() {
        if (ingredientShop_sauceListCurrentPage < ingredientShop_sauceListMaxPage)
            ingredientShop_sauceListCurrentPage++;
        else ingredientShop_sauceListCurrentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowIngredientShopSauceList(ingredientShop_sauceListCurrentPage);
    }
    public void PrevIngredientShopSaucePanel() {
        if (ingredientShop_sauceListCurrentPage > 0)
            ingredientShop_sauceListCurrentPage--;
        else ingredientShop_sauceListCurrentPage = ingredientShop_sauceListMaxPage;
        audioManager?.Play(audioManager.button01);
        ShowIngredientShopSauceList(ingredientShop_sauceListCurrentPage);
    }
    // SelectIngredient: 재료 선택.
    public void SelectIngredient(int index) {
        switch (index) {
            case 1:
                ShowIngredientPurchasePanel(ingredientShop_meat_menu01);
                break;
            case 2:
                ShowIngredientPurchasePanel(ingredientShop_meat_menu02);
                break;
            case 3:
                ShowIngredientPurchasePanel(ingredientShop_meat_menu03);
                break;
            case 4:
                ShowIngredientPurchasePanel(ingredientShop_sauce_menu01);
                break;
            case 5:
                ShowIngredientPurchasePanel(ingredientShop_sauce_menu02);
                break;
            case 6:
                ShowIngredientPurchasePanel(ingredientShop_sauce_menu03);
                break;
        }
    }

    #endregion

    #region IngredientPurchase

    // ShowIngredientPurchasePanel: 구매창을 띄운다.
    private void ShowIngredientPurchasePanel(Ingredient ingredient) {
        if (ingredient.Count >= 99) return;
        // 팝업창 띄우기.
        audioManager?.Play(audioManager.box_open, 1f);
        IngredientPurchasePopup.SetActive(true);

        IngredientPurchase_Item = ingredient;
        IngredientPurchase_amount = 1;

        IngredientPurchase_Info_Icon.transform.GetChild(1).GetComponent<Image>().sprite = IngredientPurchase_Item.sprite_Market_Popup;

        WarnningPurchasePrice();

        IngredientPurchase_Amount.transform.GetChild(1).GetComponent<Text>().text = IngredientPurchase_amount.ToString();

        IngredientPurchase_Price.transform.GetChild(1).GetComponent<Text>().text = IngredientPurchase_Item.price.ToString();
    }
    // CloseIngredientPurchasePanel: 구매창을 닫는다.
    public void CloseIngredientPurchasePanel() {
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
        IngredientPurchase_Item = null;
        IngredientPurchasePopup.SetActive(false);
    }
    // IngredientPurchase_AmountUpDown: 구매 수량 조절.
    public void IngredientPurchase_AmountUpDown(bool isUp, int amount = 1) {
        // 수량 조절.
        if (isUp) {
            if (IngredientPurchase_Item.Count + IngredientPurchase_amount < 99)
                IngredientPurchase_amount += amount;
        }
        else {
            if (IngredientPurchase_amount == 1) return;
            IngredientPurchase_amount -= amount;
        }
        // 수량 및 가격 업데이트.
        SetIngredientPurchaseAmountPrice();
        // 가격 비교 및 구매 버튼 활성화/비활성화.
        WarnningPurchasePrice();
    }
    // WarnningPurchasePrice: 가격 비교 및 구매 버튼 활성화/비활성화.
    private void WarnningPurchasePrice() {
        if (IngredientPurchase_Item.price * IngredientPurchase_amount <= GoldManager.instance.CurrentGold) {
            IngredientPurchase_PurchaseButton.SetActive(true);
            IngredientPurchase_noPurchaseButton.SetActive(false);
            IngredientPurchase_WarnningSign.SetActive(false);
        }
        else {
            IngredientPurchase_PurchaseButton.SetActive(false);
            IngredientPurchase_noPurchaseButton.SetActive(true);
            IngredientPurchase_WarnningSign.SetActive(true);
        }
    }
    // SetIngredientPurchaseAmountPrice: 구매량 및 가격 설정, 텍스트 갱신.
    private void SetIngredientPurchaseAmountPrice(int amount = 0) {
        if (amount != 0) IngredientPurchase_amount = amount;
        IngredientPurchase_Amount.transform.GetChild(1).GetComponent<Text>().text = IngredientPurchase_amount.ToString();
        IngredientPurchase_Price.transform.GetChild(1).GetComponent<Text>().text = (IngredientPurchase_Item.price * IngredientPurchase_amount).ToString();
    }
    // PurchaseIngredient: 재료 구입.
    public void PurchaseIngredient() {
        audioManager?.Play(audioManager.purchase, 2.5f);
        // 돈 차감.
        GoldManager.instance.CurrentGold -= IngredientPurchase_Item.price * IngredientPurchase_amount;
        // 재고 수량 증가.
        IngredientPurchase_Item.Count += IngredientPurchase_amount;
        // 팝업창 닫기.
        CloseIngredientPurchasePanel();
        // 재료 재고 새로고침.
        SetMeatCount();
        SetSauceCount();
        // 재료 상자가 열려있으면 해당 재료 상자 페이지 새로고침.
        if (meatSelectPanel.activeSelf) ShowMeatSelectList(meatSelect_listCurrentPage);
        else if (sauceSelectPanel.activeSelf) ShowSauceSelectList(sauceSelect_listCurrentPage);

        if (IngredientPurchase_amount >= 99) FindObjectOfType<AchievementManager>().Achievement_SoMuchPurchase();
    }

    #endregion

    #region DecoShop
    // OpenDecoShopSelectPanel: 데코 상점 창 띄우기.
    public void OpenDecoShopSelectPanel() {
        if (meatSelectPanel.activeSelf || sauceSelectPanel.activeSelf) return;
        if (BuffPopup.activeSelf) return;
        if (ingredientShopPanel.activeSelf) CloseIngredientShopPanel();
        audioManager?.Play(audioManager.box_open, 1f);
        audioManager?.PauseCookMeat();
        // 창 띄우기.
        Main_Objects.SetActive(false);
        meatSelectPanel.SetActive(false);
        sauceSelectPanel.SetActive(false);
        AngryGuage.SetActive(false);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(false);
        DecoShop_Panel.SetActive(true);
        BackgroundManager.instance.SetDeco();
        TipManager.instance.HideTip();

        originDeco = new Deco[5];
        for(int i = 0; i < 5; i++)
            originDeco[i] = DecoManager.instance.appliedDeco[i];
        prevDeco = new Deco[5];
        needUndo = new bool[5];

        ShowDecoCategory(DecoType.Roof, 0);

        // 게임 시간 멈추기.
        timeManager.SetTime_Stop();
        timeManager.SetGameTime_Stop();
        // 버튼 색깔 회색으로.
        DecoShop_Button.transform.GetChild(0).GetComponent<Image>().color = new Color(197f / 255f, 198f / 255f, 200f / 255f, 1);
    }
    // CloseDecoShopSelectPanel: 데코 상점 창 닫기.
    public void CloseDecoShopSelectPanel() {
        // 창 닫기.
        Main_Objects.SetActive(true);
        if (NyangManager.instance.orderNyang && !BossManager.instance.isBossStage) AngryGuage.SetActive(true);
        if (BossManager.instance.isBossStage) BackgroundManager.instance.SetBoss();
        else if (timeManager.CurrentTime >= timeManager.runtime_PM_Open) BackgroundManager.instance.SetPM();
        else BackgroundManager.instance.SetAM();
        if (GoldManager.instance.IsBuff) BackgroundManager.instance.SetBuff();
        DecoShop_Panel.SetActive(false);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(true);
        TipManager.instance.UnhideTip();
        audioManager?.Play(audioManager.box_close, 1f);
        audioManager?.ResumeCookMeat();
        inputManager.AsdadSwitch();
        // 데코 미리보기 초기화.
        for (int i = 0; i < 5; i++) {
            if (needUndo[i]) {
                if (prevDeco[i]) {
                    DecoManager.instance.ApplyDeco(prevDeco[i]);
                }
                else {
                    DecoManager.instance.DeapplyDeco((DecoType)i);
                }
            }
        }
        // 게임 시간 멈추기.
        timeManager.SetTime_Go();
        timeManager.SetGameTime_Go();
        DecoShop_Button.transform.GetChild(0).GetComponent<Image>().color = new Color(255f / 255f, 221f / 255f, 0f / 255f, 1f);
        OpenBoxOnCloseUI();
    }
    // ShowDecoCategory: 해당 데코 카테고리 페이지 띄우기.
    public void ShowDecoCategory(string category) {
        audioManager?.Play(audioManager.button01);
        switch (category) {
            case "Roof": ShowDecoCategory(DecoType.Roof); break;
            case "Stove": ShowDecoCategory(DecoType.Stove); break;
            case "Bulb": ShowDecoCategory(DecoType.Bulb); break;
            case "Sign": ShowDecoCategory(DecoType.Sign); break;
            case "Accessory": ShowDecoCategory(DecoType.Accessory); break;
        }
    }
    public void ShowDecoCategory(DecoType category, int page = 0) {
        DecoShop_currentCategory = category;
        DecoShop_ListCurrentPage = page;

        GameObject currentCategory = null;
        List<Deco> currentList = null;

        for (int i = 1; i < 6; i++) DecoShop_Category.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;

        switch (category) {
            case DecoType.Roof:
                currentCategory = DecoShop_Category.transform.GetChild(1).gameObject;
                break;
            case DecoType.Stove:
                currentCategory = DecoShop_Category.transform.GetChild(2).gameObject;
                break;
            case DecoType.Bulb:
                currentCategory = DecoShop_Category.transform.GetChild(3).gameObject;
                break;
            case DecoType.Sign:
                currentCategory = DecoShop_Category.transform.GetChild(4).gameObject;
                break;
            case DecoType.Accessory:
                currentCategory = DecoShop_Category.transform.GetChild(5).gameObject;
                break;
        }
        currentCategory.GetComponent<Image>().color = new Color(1, 243f / 255f, 114f / 255f, 1);
        currentList = GetAvailableDecoList(category);
        DecoShop_ListCurrentPage = page;
        DecoShop_ListMaxPage = GetListMaxPage(currentList);

        DecoShop_menu01 = currentList[3 * page];
        if (3 * page + 1 < currentList.Count) DecoShop_menu02 = currentList[3 * page + 1]; else DecoShop_menu02 = null;
        if (3 * page + 2 < currentList.Count) DecoShop_menu03 = currentList[3 * page + 2]; else DecoShop_menu03 = null;
        
        DecoShop_Menu01.transform.GetChild(0).GetComponent<Image>().sprite = DecoShop_menu01.sprite_Icon;
        DecoShop_Menu01.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        DecoShop_Menu01.transform.GetChild(1).GetChild(1).gameObject.SetActive(DecoShop_menu01.IsApply);
        if (!DecoShop_menu01.IsGet) {
            DecoShop_Menu01.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            DecoShop_Menu01.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            DecoShop_Menu01.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = DecoShop_menu01.price.ToString();
            DecoShop_Menu01.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            DecoShop_Menu01.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
        }
        else {
            DecoShop_Menu01.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            DecoShop_Menu01.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            DecoShop_Menu01.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            DecoShop_Menu01.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
        }
        if (DecoShop_menu02) {
            DecoShop_Menu02.SetActive(true);
            DecoShop_Menu02.transform.GetChild(0).GetComponent<Image>().sprite = DecoShop_menu02.sprite_Icon;
            DecoShop_Menu02.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            DecoShop_Menu02.transform.GetChild(1).GetChild(1).gameObject.SetActive(DecoShop_menu02.IsApply);
            if (!DecoShop_menu02.IsGet) {
                DecoShop_Menu02.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                DecoShop_Menu02.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                DecoShop_Menu02.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = DecoShop_menu02.price.ToString();
                DecoShop_Menu02.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                DecoShop_Menu02.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
            }
            else {
                DecoShop_Menu02.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                DecoShop_Menu02.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                DecoShop_Menu02.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                DecoShop_Menu02.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            }
        }
        else
            DecoShop_Menu02.SetActive(false);
        if (DecoShop_menu03) {
            DecoShop_Menu03.SetActive(true);
            DecoShop_Menu03.transform.GetChild(0).GetComponent<Image>().sprite = DecoShop_menu03.sprite_Icon;
            DecoShop_Menu03.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            DecoShop_Menu03.transform.GetChild(1).GetChild(1).gameObject.SetActive(DecoShop_menu03.IsApply);
            if (!DecoShop_menu03.IsGet) {
                DecoShop_Menu03.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                DecoShop_Menu03.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                DecoShop_Menu03.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = DecoShop_menu03.price.ToString();
                DecoShop_Menu03.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                DecoShop_Menu03.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
            }
            else {
                DecoShop_Menu03.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                DecoShop_Menu03.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                DecoShop_Menu03.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                DecoShop_Menu03.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            }
        }
        else
            DecoShop_Menu03.SetActive(false);
    }
    // Next/PrevDecoShopPage: 다음/이전 페이지를 보여줌.
    public void NextDecoShopPage() {
        if (DecoShop_ListCurrentPage < DecoShop_ListMaxPage)
            DecoShop_ListCurrentPage++;
        else DecoShop_ListCurrentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowDecoCategory(DecoShop_currentCategory, DecoShop_ListCurrentPage);
    }
    public void PrevDecoShopPage() {
        if (DecoShop_ListCurrentPage > 0)
            DecoShop_ListCurrentPage--;
        else DecoShop_ListCurrentPage = DecoShop_ListMaxPage;
        audioManager?.Play(audioManager.button01);
        ShowDecoCategory(DecoShop_currentCategory, DecoShop_ListCurrentPage);
    }
    // GetAvailableDecoList: 사용 가능한 데코 리스트를 받는다.
    private List<Deco> GetAvailableDecoList(DecoType type) {
        List<Deco> list = new List<Deco>();

        Dictionary<int, Deco> dictionary = new Dictionary<int, Deco>();

        switch (type) {
            case DecoType.Roof:
                dictionary = DecoManager.instance.roofDic;
                break;
            case DecoType.Stove:
                dictionary = DecoManager.instance.stoveDic;
                break;
            case DecoType.Bulb:
                dictionary = DecoManager.instance.bulbDic;
                break;
            case DecoType.Sign:
                dictionary = DecoManager.instance.signDic;
                break;
            case DecoType.Accessory:
                dictionary = DecoManager.instance.accessoryDic;
                break;
        }
        foreach (int i in dictionary.Keys)
            if (dictionary[i].IsAvailable)
                list.Add(dictionary[i]);
        return SortDecoList(list);
    }
    // SortDecoList: 리스트 재정렬.
    private List<Deco> SortDecoList(List<Deco> list) {
        List<Deco> newList = new List<Deco>();

        int index = -1;
        int min = 99999999;

        while (list.Count > 0) {
            for (int i = 0; i < list.Count; i++) {
                if (min > list[i].price) {
                    index = i;
                    min = list[i].price;
                }
            }
            newList.Add(list[index]);
            list.RemoveAt(index);
            index = -1;
            min = 99999999;
        }

        return newList;
    }
    // GetListMaxPage: 데코 리스트의 마지막 페이지를 받는다.
    public int GetListMaxPage(List<Deco> list) {
        return Mathf.CeilToInt(list.Count / 3.0f) - 1;
    }
    // SelectDeco: 데코 선택.
    public void SelectDeco(int index) {
        switch (index) {
            case 1:
                if (!DecoShop_menu01.IsGet)
                    ShowDecoPurchasePanel(DecoShop_menu01);
                break;
            case 2:
                if (!DecoShop_menu02.IsGet)
                    ShowDecoPurchasePanel(DecoShop_menu02);
                break;
            case 3:
                if (!DecoShop_menu03.IsGet)
                    ShowDecoPurchasePanel(DecoShop_menu03);
                break;
        }
    }
    // SelectDecoPreview: 데코 미리보기 선택.
    public void SelectDecoPreview(int index) {
        switch (index) {
            case 1: PreviewDeco(DecoShop_menu01); break;
            case 2: PreviewDeco(DecoShop_menu02); break;
            case 3: PreviewDeco(DecoShop_menu03); break;
        }
    }
    // PreviewDeco: 데코 미리보기.
    public void PreviewDeco(Deco deco) {
        int typeIndex = (int)deco.type;
        if (deco.IsGet) {
            // 적용.
            audioManager?.Play(audioManager.apply_item, 1f);
            DecoManager.instance.ApplyDeco(deco);
            if (prevDeco[typeIndex]) prevDeco[(int)deco.type] = null;
            needUndo[typeIndex] = false;
            originDeco[typeIndex] = deco;
        }
        else {
            //prevDeco[(int)deco.type] = DecoManager.instance.appliedDeco[(int)deco.type];
            audioManager?.Play(audioManager.apply_item, 1f);
            DecoManager.instance.ApplyDeco(deco);
            prevDeco[typeIndex] = originDeco[typeIndex];
            needUndo[typeIndex] = true;
        }
        ShowDecoCategory(DecoShop_currentCategory, DecoShop_ListCurrentPage);
    }
    #endregion

    #region DecoPurchase
    
    // ShowDecoPurchasePanel: 구매창을 띄운다.
    private void ShowDecoPurchasePanel(Deco deco) {
        audioManager?.Play(audioManager.box_open, 1f);
        // 팝업창 띄우기.
        DecoPurchasePopup.SetActive(true);

        DecoPurchase_Item = deco;

        DecoPurchasePopup_Info_Icon.transform.GetChild(1).GetComponent<Image>().sprite = DecoPurchase_Item.sprite_Icon;
        DecoPurchasePopup_Info_Icon.transform.GetChild(1).GetComponent<Image>().SetNativeSize();

        DecoWarnningPurchasePrice();

        DecoPurchasePopup_Price.transform.GetChild(1).GetComponent<Text>().text = DecoPurchase_Item.price.ToString();
        

    }
    // CloseDecoPurchasePanel: 구매창을 닫는다.
    public void CloseDecoPurchaseDeco() {
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
        DecoPurchase_Item = null;
        DecoPurchasePopup.SetActive(false);
    }
    // DecoWarnningPurchasePrice: 가격 비교 및 구매 버튼 활성화/비활성화.
    public void DecoWarnningPurchasePrice() {
        if(DecoPurchase_Item.price <= GoldManager.instance.CurrentGold) {
            DecoPurchasePopup_PurchaseButton.SetActive(true);
            DecoPurchasePopup_noPurchaseButton.SetActive(false);
            DecoPurchasePopup_WarnningSign.SetActive(false);
        }
        else {
            DecoPurchasePopup_PurchaseButton.SetActive(false);
            DecoPurchasePopup_noPurchaseButton.SetActive(true);
            DecoPurchasePopup_WarnningSign.SetActive(true);
        }
    }
    // PurchaseDeco: 재료 구입.
    public void PurchaseDeco() {
        audioManager?.Play(audioManager.purchase, 2.5f);
        // 돈 차감.
        GoldManager.instance.CurrentGold -= DecoPurchase_Item.price;
        // 데코 아이템 획득.
        DecoPurchase_Item.IsGet = true;
        PreviewDeco(DecoPurchase_Item);
        // 팝업창 닫기.
        CloseDecoPurchaseDeco();
        // 리스트 새로고침.
        ShowDecoCategory(DecoShop_currentCategory, DecoShop_ListCurrentPage);
    }

    #endregion

    #region MiniGame_Tanning

    // Open/CloseMiniGameTanningPopup: 썬탠 게임 하러 가는 팝업 화면 열고 닫기.
    public void OpenMiniGameTanningPopup() {
        CloseAllUI();
        MiniGame_Tanning_Popup.SetActive(true);
        TipManager.instance.HideTip();
    }
    public void CloseMiniGameTanningPopup() {
        inputManager.AsdadSwitch();
        audioManager?.Play(audioManager.button01);
        MiniGame_Tanning_Popup.SetActive(false);
    }

    // ChangeSceneMainToTanning: 메인 게임 -> 썬탠 게임 화면 전환 및 썬탠 게임 시작.
    public void ChangeSceneMainToTanning() {
        // 메인 게임 화면 비활성화.
        Main_Objects.SetActive(false);
        Main_UI.SetActive(false);
        Main_Scene.SetActive(false);
        Calender.SetActive(false);
        // 썬탠 게임 활성화.
        MiniGame_Tanning_UI.SetActive(true);
        // 썬탠 게임 시작.
        TanningManager.instance.GameStart();
    }
    // ChangeSceneTanningToMain: 썬탠 게임 -> 메인 게임 화면 전환 및 메인 게임 재개.
    public void ChangeSceneTanningToMain() {
        inputManager.AsdadSwitch();
        audioManager?.Play(audioManager.button01);
        TipManager.instance.UnhideTip();
        // 썬탠 게임 비활성화.
        MiniGame_Tanning_UI.SetActive(false);
        // 메인 게임 화면 활성화.
        Main_Scene.SetActive(true);
        Main_UI.SetActive(true);
        Main_Objects.SetActive(true);
        Calender.SetActive(true);
        // 메인 게임 시작: 오후 장사로.
        timeManager.SetTime_PMOpen();
    }
    #endregion
    
    #region NyangList

    public void OpenNyangListPanel() {
        if (ingredientShopPanel.activeSelf || DecoShop_Panel.activeSelf || BuffPopup.activeSelf) return;
        audioManager?.Play(audioManager.box_open, 1f);
        audioManager?.PauseCookMeat();

        Main_Objects.SetActive(false);
        Main_Scene.SetActive(false);
        Main_UI.SetActive(false);
        Calender.SetActive(false);
        TipManager.instance.HideTip();
        if (BossManager.instance.isBossStage) {
            BackgroundManager.instance.SetSize_Default();
            BossManager.instance.bossScene.SetActive(false);
        }
        NyangListPanel.SetActive(true);

        LoadAndSortNyangList();
        nyangList_currentPage = 0;
        nyangList_maxPage = GetNyangListMaxPage(nyangList);
        ShowNyangList(nyangList_currentPage);

        // 게임 시간 멈추기.
        timeManager.SetTime_Stop();
        timeManager.SetGameTime_Stop();

        // 팁 닫기.
        TipManager.instance.CloseTip(TipType.CatList);
    }
    public void CloseNyangListPanel() {
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
        audioManager?.ResumeCookMeat();

        Main_Objects.SetActive(true);
        Main_Scene.SetActive(true);
        Main_UI.SetActive(true);
        Calender.SetActive(true);
        TipManager.instance.UnhideTip();
        if (BossManager.instance.isBossStage) {
            BackgroundManager.instance.SetBoss();
            BossManager.instance.bossScene.SetActive(true);
        }
        NyangListPanel.SetActive(false);
        timeManager.SetTime_Go();
        timeManager.SetGameTime_Go();
        OpenBoxOnCloseUI();
    }
    // LoadAndSortNyangList: 냥이 리스트 재정렬.
    private void LoadAndSortNyangList() {
        // 냥이 리스트 불러오기.
        Dictionary<int, GameObject> nyangDic = NyangManager.instance.nyangPrefabDic;

        // 랭크별로 냥이 분류하기.
        Dictionary<int, Nyang> normalNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> rareNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> hiddenNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> bossNyang = new Dictionary<int, Nyang>();
        foreach (int key in nyangDic.Keys) {
            Nyang nyang = nyangDic[key].GetComponent<Nyang>();
            switch (nyang.rank) {
                case NyangRank.Normal:
                    normalNyang.Add(nyang.index, nyang);
                    break;
                case NyangRank.Rare:
                    rareNyang.Add(nyang.index, nyang);
                    break;
                case NyangRank.Hidden:
                    hiddenNyang.Add(nyang.index, nyang);
                    break;
                case NyangRank.Boss:
                    bossNyang.Add(nyang.index, nyang);
                    break;
            }
        }

        // 최종 리스트
        nyangList = new List<Nyang>();
        foreach (int index in normalNyang.Keys) {
            nyangList.Add(normalNyang[index]);
        }
        foreach (int index in rareNyang.Keys) {
            nyangList.Add(rareNyang[index]);
        }
        foreach (int index in hiddenNyang.Keys) {
            nyangList.Add(hiddenNyang[index]);
        }
        foreach(int index in bossNyang.Keys) {
            nyangList.Add(bossNyang[index]);
        }
    }
    // GetNyangListMaxPage: 냥이 리스트의 마지막 페이지를 받는다.
    private int GetNyangListMaxPage(List<Nyang> list) {
        return Mathf.CeilToInt(list.Count / 6.0f) - 1;
    }
    // GetNyangListofPage: 해당 페이지의 냥이 리스트를 받는다.
    private void GetNyangListofPage(int page) {
        nyangList01 = nyangList[6 * page];
        if (6 * page + 1 < nyangList.Count) nyangList02 = nyangList[6 * page + 1]; else nyangList02 = null;
        if (6 * page + 2 < nyangList.Count) nyangList03 = nyangList[6 * page + 2]; else nyangList03 = null;
        if (6 * page + 3 < nyangList.Count) nyangList04 = nyangList[6 * page + 3]; else nyangList04 = null;
        if (6 * page + 4 < nyangList.Count) nyangList05 = nyangList[6 * page + 4]; else nyangList05 = null;
        if (6 * page + 5 < nyangList.Count) nyangList06 = nyangList[6 * page + 5]; else nyangList06 = null;
    }
    // ShowNyangList: 해당 페이지의 냥이 리스트를 보여준다.
    private void ShowNyangList(int page) {
        GetNyangListofPage(page);

        ShowNyang(NyangList01, nyangList01);
        ShowNyang(NyangList02, nyangList02);
        ShowNyang(NyangList03, nyangList03);
        ShowNyang(NyangList04, nyangList04);
        ShowNyang(NyangList05, nyangList05);
        ShowNyang(NyangList06, nyangList06);

    }
    // ShowNyang: 냥이 보여주기.
    private void ShowNyang(GameObject listObj, Nyang nyang) {
        // 냥이가 null값이 아니면 리스트 오브젝트 활성화.
        listObj.SetActive(nyang);
        if (nyang) {// 냥이가 한 번이라도 방문한 경우에만 냥이 모습을 보여준다.
            Image nyangImage = listObj.transform.GetChild(0).GetComponent<Image>();
            if (nyang.VisitCount > 0) {
                nyangImage.sprite = nyang.visitSprite;
            }
            else
                nyangImage.sprite = nyangList_hideSprite;
            nyangImage.SetNativeSize();
            // 획득하지 못한 Hidden인 경우에만 이름을 숨긴다.
            if (nyang.rank == NyangRank.Hidden && !(nyang.IsCollected))
                listObj.transform.GetChild(2).GetComponent<Text>().text = "???";
            else
                listObj.transform.GetChild(2).GetComponent<Text>().text = nyang.NyangName;
            // 냥이 방문 조건 달성한 경우 특징을 보여주고,
            if (nyang.IsCollected)
                listObj.transform.GetChild(3).GetComponent<Text>().text = nyang.personality;
            // 냥이 방문 조건을 달성하지 못했을 경우,
            else {
                // Normal: 그냥 / 언젠가 옴.      Rare: 등장조건 보여줌.     Hidden: 걍 숨김.
                if (nyang.rank == NyangRank.Normal) listObj.transform.GetChild(3).GetComponent<Text>().text = "그냥\n언젠가 옴.";
                else if (nyang.rank == NyangRank.Rare) listObj.transform.GetChild(3).GetComponent<Text>().text = nyang.condition;
                else if (nyang.rank == NyangRank.Hidden) listObj.transform.GetChild(3).GetComponent<Text>().text = "???\n??";
            }
        }
    }

    public void NextNyangList() {
        if (NyangStoryPopup.activeSelf) return;
        if (nyangList_currentPage < nyangList_maxPage)
            nyangList_currentPage++;
        else nyangList_currentPage = 0;
        audioManager?.Play(audioManager.button01);
        ShowNyangList(nyangList_currentPage);
    }
    public void PrevNyangList() {
        if (NyangStoryPopup.activeSelf) return;
        if (nyangList_currentPage > 0)
            nyangList_currentPage--;
        else nyangList_currentPage = nyangList_maxPage;
        audioManager?.Play(audioManager.button01);
        ShowNyangList(nyangList_currentPage);
    }

    #endregion

    #region NyangList_Story
    public void OpenNyangStoryPopup() {
        // 팝업창 띄우기.
        NyangStoryPopup.SetActive(true);
        audioManager?.Play(audioManager.box_open, 1f);
    }
    public void CloseNyangStoryPopup() {
        // 팝업창 닫기.
        NyangStoryPopup.SetActive(false);
        inputManager.AsdadSwitch();
        audioManager?.Play(audioManager.box_close, 1f);
    }
    #endregion

    #region Option

    public void OpenOption() {
        if (ingredientShopPanel.activeSelf || DecoShop_Panel.activeSelf || BuffPopup.activeSelf) return;

        audioManager?.Play(audioManager.box_open, 1f);
        audioManager?.PauseCookMeat();

        Main_Objects.SetActive(false);
        Main_Scene.SetActive(false);
        Main_UI.SetActive(false);
        meatSelectPanel.SetActive(false);
        sauceSelectPanel.SetActive(false);
        Calender.SetActive(false);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(false);
        OptionPanel.SetActive(true);
        TipManager.instance.HideTip();
        timeManager.SetTime_Stop();
        timeManager.SetGameTime_Stop();
        TipManager.instance.CloseTip(TipType.Option);
        OptionManager.instance.OptionReset();
    }
    public void CloseOption() {
        audioManager?.Play(audioManager.box_close, 1f);
        inputManager.AsdadSwitch();
        audioManager?.ResumeCookMeat();

        timeManager.SetTime_Go();
        timeManager.SetGameTime_Go();
        TipManager.instance.UnhideTip();
        Main_Objects.SetActive(true);
        Main_Scene.SetActive(true);
        Main_UI.SetActive(true);
        Calender.SetActive(true);
        if (BossManager.instance.isBossStage) BossManager.instance.bossScene.SetActive(true);
        OptionPanel.SetActive(false);
        OpenBoxOnCloseUI();
    }
    #endregion

    #region OpenBetaPopup
    

    public void OpenOpenBetaPopup() {
        OpenBetaPopup.SetActive(true);
    }
    public void CloseOpenBetaPopup() {
        OpenBetaPopup.SetActive(false);

        // *********** OpenBeta Popup 삭제시 TimeManager, TutorialManager에 주석처리해놓은
        // *********** 각 함수들 주석처리를 풀어 시작하자마자 게임이 진행되도록 한다.
        if (timeManager.IsTutorial) TutorialManager.instance.StartCoroutine(TutorialManager.instance.Tutorial());
        else {
            timeManager.GameStartOrContinue();
        }
    }
    public void OpenBeta_Feedback() {
        string mailto = "peanutjelly.dev@gmail.com";
        string subject = EscapeURL("버그 리포트 / 기타 문의사항");
        string body = EscapeURL
            (
             "이 곳에 내용을 작성해주세요.\n\n\n\n" +
             "________" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
             "________"
            );

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }
    private string EscapeURL(string url) {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }


    public void OpenBeta_Funding() {
        Application.OpenURL("https://www.ycrowdy.com/open/207");
    }
    #endregion

    // CloseAllUI: 모든 UI 닫기.
    private void CloseAllUI() {
        if (meatSelectPanel.activeSelf) CloseMeatSelectPanel();
        if (sauceSelectPanel.activeSelf) CloseSauceSelectPanel();
        if (ingredientShopPanel.activeSelf) CloseIngredientShopPanel();
        if (IngredientPurchasePopup.activeSelf) CloseIngredientPurchasePanel();
        if (DecoShop_Panel.activeSelf) CloseDecoShopSelectPanel();
        if (DecoPurchasePopup.activeSelf) CloseDecoPurchaseDeco();
        if (NyangListPanel.activeSelf) CloseNyangListPanel();
        if (NyangStoryPopup.activeSelf) CloseNyangStoryPopup();
        if (OptionPanel.activeSelf) CloseOption();
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
    
    // UI를 닫을 때, 경우에 따라 상자 오픈.
    public void OpenBoxOnCloseUI() {
        // 냥이가 없으면 패스.
        if (!NyangManager.instance.orderNyang) return;
        // 고기가 올려져있지 않으면 고기상자를 연다.
        if (!CookManager.instance.cookFood) OpenMeatSelectPanel();
        // 고기가 다 구워져있으면 소스상자를 연다.
        else if (CookManager.instance.cookFood.step == CookStep.Turn07) OpenSauceSelectPanel();
    }
}