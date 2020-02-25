using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class AchievementManager : MonoBehaviour {

    bool newNyang1, newNyang10, newNyang20;
    bool moneySwag1, moneySwag10, moneySwag50;
    bool dayCount15, dayCount30;
    bool rouletteNo, roulette20;
    bool tanning10;
    bool purchase99;

    
    void Awake() {
        DontDestroyOnLoad(this);

        Initialize();

        nyangCount = PlayerPrefs.GetInt("NyangCount");

        Login();
    }
    public void Initialize() {
#if UNITY_ANDROID
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#elif UNITY_IOS
                GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
    }

    private void OnDayChanged(object sender, TimeManager.EventDayChangeArgs args)
    {
        Achievement_DayCount(args.Day);
    }

    public void Login() {
        // 이미 인증된 사용자는 바로 로그인 성공.
        if (Social.localUser.authenticated)
            Debug.Log("Login: name: " + Social.localUser.userName);
        else
            Social.localUser.Authenticate((bool success) => {
                if (success) Debug.Log("Login: name: " + Social.localUser.userName);
                else Debug.Log("Login Fail");
            });
    }

    public void LogOut() {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }

    public void OpenAchievementUI() {
        Social.ShowAchievementsUI();
    }

#region Achievements
    public void Achievement_NewNyang(int count) {
        string ID = "";
        switch (count) {
            case 1:
#if UNITY_ANDROID
                ID = GPGSIds.achievement;
#elif UNITY_IOS
                ID = "NewNyang";
#endif
                break;
            case 10:
#if UNITY_ANDROID
                ID = GPGSIds.achievement___10;
#elif UNITY_IOS
                ID = "NewNyang10";
#endif
                break;
            case 20:
#if UNITY_ANDROID
                ID = GPGSIds.achievement___20;
#elif UNITY_IOS
                ID = "NewNyang20";
#endif
                break;
            default:
                break;
        }
        Social.ReportProgress(ID, 100.0f, null);
    }
    public void Achievement_MoneySwag(int count) {
        string ID = "";
        switch (count) {
            case 10000:
#if UNITY_ANDROID
                ID = GPGSIds.achievement_2;
#elif UNITY_IOS
                ID = "MoneySwag";
#endif
                break;
            case 100000:
#if UNITY_ANDROID
                ID = GPGSIds.achievement_4;
#elif UNITY_IOS
                ID = "MoneySwag10";
#endif
                break;
            case 500000:
#if UNITY_ANDROID
                ID = GPGSIds.achievement_5;
#elif UNITY_IOS
                ID = "MoneySwag50";
#endif
                break;
            default:
                break;
        }
        Social.ReportProgress(ID, 100.0f, null);
    }
    public void Achievement_DayCount(int count) {
        string ID = "";
        switch (count) {
            case 15:
#if UNITY_ANDROID
                ID = GPGSIds.achievement_3;
#elif UNITY_IOS
                ID = "Day15";
#endif
                break;
            case 30:
#if UNITY_ANDROID
                ID = GPGSIds.achievement_6;
#elif UNITY_IOS
                ID = "Day30";
#endif
                break;
            default:
                break;
        }
        Social.ReportProgress(ID, 100.0f, null);
    }
    public void Achievement_RouletteNO() {
#if UNITY_ANDROID
        Social.ReportProgress(GPGSIds.achievement_7, 100.0f, null);
#elif UNITY_IOS
        Social.ReportProgress("Roulette_Fail10", 100.0f, null);
#endif
    }
    public void Achievement_Roulette20() {
#if UNITY_ANDROID
        Social.ReportProgress(GPGSIds.achievement_8, 100.0f, null);
#elif UNITY_IOS
        Social.ReportProgress("Roulette20", 100.0f, null);
#endif
    }
    public void Achievement_Tanning20() {
#if UNITY_ANDROID
        Social.ReportProgress(GPGSIds.achievement_9, 100.0f, null);
#elif UNITY_IOS
        Social.ReportProgress("Tanning20", 100.0f, null);
#endif
    }
    public void Achievement_SoMuchPurchase() {
#if UNITY_ANDROID
        Social.ReportProgress(GPGSIds.achievement_10, 100.0f, null);
#elif UNITY_IOS
        Social.ReportProgress("Purchase99", 100.0f, null);
#endif
    }
#endregion

    int nyangCount;
    public void newNyangCount() {
        nyangCount++;
        PlayerPrefs.SetInt("NyangCount", nyangCount);
        if (nyangCount >= 1) Achievement_NewNyang(1);
        else if (nyangCount >= 10) Achievement_NewNyang(10);
        else if (nyangCount >= 20) Achievement_NewNyang(20);
    }

    int roulette0Count;
    int roulettePlayCount;
    public void Roulette0Count() {
        roulette0Count++;
        PlayerPrefs.SetInt("Roulette0Count", roulette0Count);
        if (roulette0Count >= 10) Achievement_RouletteNO();
    }
    public void RoulettePlayCount() {
        roulettePlayCount++;
        PlayerPrefs.SetInt("RoulettePlayCount", roulettePlayCount);
        if (roulettePlayCount >= 20) Achievement_Roulette20();
    }

    int tanningSuccessCount;
    public void TanningSuccessCount() {
        tanningSuccessCount++;
        PlayerPrefs.SetInt("TanningSuccessCount", tanningSuccessCount);
        if (tanningSuccessCount >= 20) Achievement_Tanning20();
    }


    private void GetAchievement(string ID) {
        Social.ReportProgress(ID, 100.0f, (bool success) => {
            if (success) {
                Debug.Log(ID + " is Get");
            }
            else {
                Debug.Log(ID + " is Not Get");
            }
        });
    }

    public void OnDEBUGHIDDENACHIEVEMENTBUTTON() {
        Achievement_MoneySwag(500000);
    }
}

//    private void GetAchievement(string achievementName, bool _switch) {
//        if (!_switch) {
//            Social.ReportProgress(achievementName, 100.0f, (bool success) => {
//            if (success) {
//                _switch = true;
//                    PlayerPrefs("ACHIEVEMENT_" + achievementName, true);
//                }
//            }
//            );
//        }
//    }
//}