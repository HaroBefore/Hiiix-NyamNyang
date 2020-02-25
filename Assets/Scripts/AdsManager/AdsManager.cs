using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour {

    public static AdsManager instance;

    public int AdsCycle;
    private int adsCycle;



    void Awake() {
        if (!instance) instance = this;


    }

    void Start() {
        // 광고주기 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyamNyangAdsCycle") == 1049)) {
            adsCycle = 0;
            PlayerPrefs.SetInt("NyamNyangAdsCycle", 1049);
        }
        else adsCycle = PlayerPrefs.GetInt("AdsCycle");
    }


    public bool IncreaseCycle() {
        Debug.Log("INCREASECYCLE START");
        adsCycle++;
        PlayerPrefs.SetInt("AdsCycle", adsCycle);
        Debug.Log("INCREASECYCLE END");
        return CheckCycle();
    }

    private bool CheckCycle() {
        Debug.Log("CHECKCYCLE START");
        if (adsCycle != AdsCycle) return false;
        if (AdsManager_Admob.instance.IsRemoveAds) return false;
        adsCycle = 0;
        PlayerPrefs.SetInt("AdsCycle", adsCycle);

        // 광고 호출.
        ShowAds();

        Debug.Log("CHECKCYCLE END");

        return true;
    }

    private void ShowAds() {
        Debug.Log("SHOWADS START");
        TimeManager.Instance.Pause();
        AdsManager_Admob.instance.ShowAd();

        Debug.Log("SHOWADS END");
    }
}