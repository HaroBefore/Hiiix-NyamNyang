using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

    public static OptionManager instance;

    [Header("Button")]
    public GameObject b_sound_on;
    public GameObject b_sound_off;
    public GameObject b_push_on;
    public GameObject b_push_off;
    public GameObject b_removeAds_purchase;
    public GameObject b_removeAds_actived;
    [Header("Tutorial")]
    public GameObject tutorial;
    public GameObject tutorialStartMask;
    [Header("TutorialScrenshots")]
    public Sprite[] mainTutorials;
    public Sprite[] tanningTutorials;
    private int tutorialScene;
    private bool isTutorial;
    private bool isMainTutorial;

    [Header("ResetPopup")]
    public GameObject ResetPopup;
    [Header("RemoveAdsPopup")]
    public GameObject RemoveAdsPopup;

    [Header("SpecialThanksPanel")]
    public GameObject specialThanksPanel;

    [Header("Company")]
    public int company_nyangCount;
    public GameObject company_list01;
    public GameObject company_list02;
    public GameObject company_list03;
    public Sprite[] company_sprites;
    [TextArea] public string[] company_texts;
    private int company_maxPage;
    private int company_currentPage;

    private bool isSoundOn;
    private bool isPushOn;

    public GameObject optionPanel;
    
    void Awake() {
        if (!instance) instance = this;



        // 초기값 세팅.
        if (PlayerPrefs.GetInt("NyamNyangOption") != 1049) {

            isSoundOn = true;
            PlayerPrefs.SetInt("Option_Sound", 1);
            isPushOn = true;
            PlayerPrefs.SetInt("Option_Push", 1);

            PlayerPrefs.SetInt("NyamNyangOption", 1049);
        }
        else {
            isSoundOn = PlayerPrefs.GetInt("Option_Sound") == 1;
            isPushOn = PlayerPrefs.GetInt("Option_Push") == 1;
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (isTutorial) {
                if (Input.mousePosition.x < Screen.width / 2)
                    PrevTutorial();
                else
                    NextTutorial();
            }
        }
    }

    public void ShowOption()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager?.Play(audioManager.box_open, 1f);
        
        optionPanel.SetActive(true);
        OptionReset();
    }

    public void HideOption()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager?.Play(audioManager.box_close, 1f);
        optionPanel.SetActive(false);
    }

    public void OptionReset() {
        SetButtons();
        Company_Set();
    }

    public void SetButtons() {
        SetButton(b_sound_on, isSoundOn);
        SetButton(b_sound_off, !isSoundOn);
        SetButton(b_push_on, isPushOn);
        SetButton(b_push_off, !isPushOn);


        bool isRemoveAds = AdsManager_Admob.instance.IsRemoveAds;
        b_removeAds_purchase.SetActive(!isRemoveAds);
        b_removeAds_actived.SetActive(isRemoveAds);
    }
    #region Sound
    public void Sound_On() {
        AudioManager.Instance?.SetMute(false);
        PlayerPrefs.SetInt("Option_Sound", 1);
        isSoundOn = true;
        SetButtons();
    }
    public void Sound_Off() {
        AudioManager.Instance?.SetMute(true);
        PlayerPrefs.SetInt("Option_Sound", 0);
        isSoundOn = false;
        SetButtons();
    }
    #endregion

    #region Push
    public void Push_On() {
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/topics/alarm");
        PlayerPrefs.SetInt("Option_Push", 1);
        isPushOn = true;
        SetButtons();
    }
    public void Push_Off() {
        Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync("/topics/alarm");
        PlayerPrefs.SetInt("Option_Push", 0);
        isPushOn = false;
        SetButtons();
    }
    #endregion

    #region Tutorial
    public void Tutorial_Main() {
        UIManager.instance.CloseOption();
        PlayTutorial(true);
    }
    public void Tutorial_Tanning() {
        UIManager.instance.CloseOption();
        PlayTutorial(false);
    }
    private void PlayTutorial(bool isMain) {
        TimeManager.Instance.Pause();
        tutorialScene = -1;
        isTutorial = true;
        isMainTutorial = isMain;
        tutorial.GetComponent<Image>().sprite = (isMain) ? mainTutorials[0] : tanningTutorials[0];
        tutorial.SetActive(true);
        tutorialStartMask.SetActive(true);
    }
    private void NextTutorial() {
        Sprite[] sprites = (isMainTutorial) ? mainTutorials : tanningTutorials;
        if (tutorialScene < 0) {
            tutorialScene++;
            tutorialStartMask.SetActive(false);
        }
        else if (tutorialScene < sprites.Length - 1) {
            tutorialScene++;
            tutorial.GetComponent<Image>().sprite = sprites[tutorialScene];
        }
        else
            EndTutorial();
    }
    private void PrevTutorial() {
        Sprite[] sprites = (isMainTutorial) ? mainTutorials : tanningTutorials;
        if (tutorialScene > 0) {
            tutorialScene--;
            tutorial.GetComponent<Image>().sprite = sprites[tutorialScene];
        }
    }
    private void EndTutorial() {
        tutorial.SetActive(false);
        isTutorial = false;
        TimeManager.Instance.Resume();
    }
    #endregion

    #region Language
    public void Language_Next()
    {
        StringDataObject.NextLanguage();
        FindObjectOfType<TextLocalizer>().ReloadText();
    }
    #endregion

    #region Application(System)
    public void ApplicationReset() {
        OpenResetPopup();
    }
    private void OpenResetPopup() {
        ResetPopup.SetActive(true);
    }
    public void CloseResetPopup() {
        ResetPopup.SetActive(false);
    }
    public void ResetGame() {
        PlayerPrefs.DeleteAll();
        //SceneManager.LoadScene("Title");
        Application.Quit();
    }
    public void ApplicationRestore() {
        IAPManager.instance.RestoreTransactions();
    }
    public void ApplicationRemoveAds() {
        OpenRemoveAdsPopup();
    }
    private void OpenRemoveAdsPopup() {
        RemoveAdsPopup.SetActive(true);
    }
    public void CloseRemoveAdsPopup() {
        RemoveAdsPopup.SetActive(false);
    }
    public void PurchaseRemoveAds() {
        if (!AdsManager_Admob.instance.IsRemoveAds) {
            CloseRemoveAdsPopup();
            IAPManager.instance.PurchaseItem_RemoveAds();
        }
    }
    public void ApplicationQuit() {
        Application.Quit();
    }
    #endregion

    #region Company
    public void FundingNyang() {
        Application.OpenURL("https://www.ycrowdy.com/open/207");
    }
    public void SpecialThanks() {
        OpenSpecialThanksPanel();
    }
    private void OpenSpecialThanksPanel() {
        specialThanksPanel.SetActive(true);
    }
    public void CloseSpecialThanksPanel() {
        specialThanksPanel.SetActive(false);
    }
    public void Company_Set() {
        company_currentPage = 0;
        company_maxPage = company_nyangCount / 3;
        Company_ShowPage(company_currentPage);
    }
    public void Company_NextPage() {
        if (company_currentPage < company_maxPage) company_currentPage++;
        else company_currentPage = 0;
        Company_ShowPage(company_currentPage);
    }
    public void Company_PrevPage() {
        if (company_currentPage > 0) company_currentPage--;
        else company_currentPage = company_maxPage;
        Company_ShowPage(company_currentPage);
    }
    private void Company_ShowPage(int page) {
        company_list01.GetComponent<Image>().sprite = company_sprites[page * 3];
        company_list01.transform.GetChild(0).GetComponent<Text>().text = company_texts[page * 3];
        if (company_sprites.Length > page * 3 + 1) {
            company_list02.GetComponent<Image>().sprite = company_sprites[page * 3 + 1];
            company_list02.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            company_list02.transform.GetChild(0).GetComponent<Text>().text = company_texts[page * 3 + 1];
        }
        else {
            company_list02.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            company_list02.transform.GetChild(0).GetComponent<Text>().text = "";
        }
        if (company_sprites.Length > page * 3 + 2) {
            company_list03.GetComponent<Image>().sprite = company_sprites[page * 3 + 2];
            company_list03.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            company_list03.transform.GetChild(0).GetComponent<Text>().text = company_texts[page * 3 + 2];
        }
        else {
            company_list03.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            company_list03.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
    #endregion



    private void SetButton(GameObject b, bool isOn) {
        b.GetComponent<Image>().color = isOn ? new Color(243f / 255f, 232f / 255f, 92f / 255f, 1) : new Color(187f / 255f, 187f / 255f, 187f / 255f, 1);
    }
}