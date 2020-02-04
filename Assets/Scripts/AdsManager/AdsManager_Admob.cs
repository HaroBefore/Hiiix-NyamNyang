using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager_Admob : MonoBehaviour {

    public static AdsManager_Admob instance;

    public bool isBuffSwitch;

    private InterstitialAd ad_interstitial;
    private RewardBasedVideoAd ad_reward;
    //private InterstitialAd ad_reward;

    [SerializeField] private string appId;
    [SerializeField] private string unitId;
    [SerializeField] private string deviceId;
    [SerializeField] private bool isTest;

    // 광고 제거 여부.
    private bool isRemoveAds;
    public bool IsRemoveAds {
        get { return isRemoveAds; }
        set {
            isRemoveAds = value;
            PlayerPrefs.SetInt("IsRemoveAds", value ? 1 : 0);
        }
    }

    private TimeManager timeManager;
    private UIManager uiManager;

    void Awake() {
        if (!instance) instance = this;
        timeManager = FindObjectOfType<TimeManager>();
        uiManager = FindObjectOfType<UIManager>();
        // 초기값 세팅.
        //if (PlayerPrefs.GetInt("NyamNyangAds") != 1049) {

        //    IsRemoveAds = false;

        //    PlayerPrefs.SetInt("NyamNyangAds", 1049);
        //}
        //else {

        //    IsRemoveAds = PlayerPrefs.GetInt("IsRemoveAds") == 1;

        //}
    }

    void Start() {
        MobileAds.Initialize(appId);
        LoadAd();
        LoadAdReward();
    }

    void LoadAd(bool DEBUG = false) {
        // Request 객체 생성.
        AdRequest request = new AdRequest.Builder().Build();
        // UnitId / DeviceId 설정.
        if (!DEBUG) {
            if (isTest) {
                if (deviceId.Length > 0)
                    request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(deviceId).Build();
                else
                    unitId = "ca-app-pub-3940256099942544/1033173712"; //테스트 유닛 ID

            }
        }
        else {
#if UNITY_EDITOR
            unitId = "unused";
#elif UNITY_ANDROID
            unitId = "ca-app-pub-5724616456813938/7012056755";
#elif UNITY_IOS
            unitId = "ca-app-pub-5724616456813938/6789860169";
#endif
        }
        // 광고 객체 생성.
        ad_interstitial = new InterstitialAd(unitId);
        // 광고 로드.
        ad_interstitial.LoadAd(request);
        // 콜백 함수 등록.
        if (!DEBUG) {
            ad_interstitial.OnAdClosed += HandleOnAdClosed;
            ad_interstitial.OnAdOpening += HandleOnAdOpening;
        }
    }
    void LoadAdReward() {
        AdRequest request = new AdRequest.Builder().Build();
#if UNITY_EDITOR
        unitId = "unused";
#elif UNITY_ANDROID
        unitId = "ca-app-pub-5724616456813938/4734044679"; //"ca-app-pub-5724616456813938/4734044679";
#elif UNITY_IOS
        unitId = "ca-app-pub-5724616456813938/4446711000";
#endif
        ad_reward = RewardBasedVideoAd.Instance;
        ad_reward.LoadAd(request, unitId);
        ad_reward.OnAdOpening += HandleOnAdOpening;
        ad_reward.OnAdClosed += HandleOnAdRewardClosed;
        ad_reward.OnAdRewarded += HandleOnAdRewarded;
    }

    private void HandleOnAdOpening(object sender, EventArgs e) {
        timeManager.Pause();
    }

    private void HandleOnAdClosed(object sender, EventArgs e) {
        ActionAfterAds();
        // 광고 객체 갱신.
        ad_interstitial.Destroy();
        LoadAd();
    }

    private void HandleOnAdRewardClosed(object sender, EventArgs e) {
        timeManager.Resume();
        if (!isBuffSwitch) uiManager.BuffOff();
    }

    private void HandleOnAdRewarded(object sender, EventArgs e) {
        isBuffSwitch = true;
        ActionAfterAdsReward();
        LoadAdReward();//TODO::핸들러등록한번만
    }

    public void ShowAd() {
        Debug.Log("SHOWAD START");
#if UNITY_EDITOR
        ActionAfterAds();
#else
        if (IsRemoveAds) {
            ActionAfterAds();
            return;
        }
        if (!ad_interstitial.IsLoaded()) {
            LoadAd();
            return;
        }
        else ad_interstitial.Show();
#endif
        Debug.Log("SHOWAD END");
    }

    public void ShowAd_Buff() {
        AudioManager.instance?.Play(AudioManager.instance.button01);
        isBuffSwitch = false;
#if UNITY_EDITOR
        ActionAfterAdsReward();
#else
        if (IsRemoveAds) {
            //ActionAfterAds();
            ad_reward.Show();
        }
        else if (!ad_reward.IsLoaded()) {
            LoadAdReward();
        }
        else ad_reward.Show();
#endif
        timeManager.ResetBuffCooltime();
        uiManager.CloseBuffPopup();
        uiManager.buffButton_Pushed.SetActive(true);
        uiManager.buffButton.SetActive(false);
    }

    public void RemoveAds() {
        IsRemoveAds = true;
    }

    // 광고 후 처리. 광고 제거로 인해 광고를 재생하지 않으면 따로 얘를 실행시켜줌...
    private void ActionAfterAds() {
        Debug.Log("ActionAfterAds START");
        if (ScenarioManager.instance.lastScenarioType == ScenarioType.Boss) timeManager.BossOpen();
        else timeManager.Open(true);
        if (!IsRemoveAds) TipManager.instance.ShowTip(TipType.Option);
        timeManager.Resume();
        Debug.Log("ActionAfterAds END");
    }

    private void ActionAfterAdsReward() {
        GoldManager.instance.IsBuff = true;
    }
}