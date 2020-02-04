using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public enum TimeType
{
    AM,
    PM
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    public event Action EventBuffActivate = () => { };
    public event Action EventBuffDeactivate = () => { };

    private TimeManager _timeManager;
    private AchievementManager _achievementManager;
    private BackgroundManager _backgroundManager;
    private UIManager _uiManager;
    
    // 튜토리얼중인지 여부.
    private bool _isTutorial;
    public bool IsTutorial {
        get { return _isTutorial; }
        set {
            _isTutorial = value;
            if (value) {
                _timeManager.Pause();
            }
            else {
                _timeManager.Resume();
            }
        }
    }
    
    // 썬탠중인지 여부.
    public bool isTanning { get; set; }
    
    // 장사 시작했는지 여부.
    private bool isOpen;
    
    private bool daySwitch;

    private TimeType _timeType;
    public TimeType TimeType => _timeType;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeManager = TimeManager.Instance;
        _achievementManager = FindObjectOfType<AchievementManager>();
        _backgroundManager = FindObjectOfType<BackgroundManager>();
        _uiManager = FindObjectOfType<UIManager>();
        
        SetUpEvent();
        StartCoroutine(CoStart());
    }

    IEnumerator CoStart()
    {
        yield return null;
        Init();
    }

    private void OnDestroy()
    {
        CleanUpEvent();
    }

    public bool isBuffTipOn { set; get; }
    private int BuffTipTime;
    // TODO RandomTipOn: 하루에 한 번 랜덤하게 버프팁을 띄운다.
    private void RandomTipOn() {
        /*
        isBuffTipOn = false;
        int time = runtime_AM_Close + 10;
        int nobug = 0;
        while (((runtime_AM_Close < time) && (time < runtime_PM_Open))) {
            if (nobug > 1000) { Debug.LogError("BUGBUGZZZZZ"); return; }
            int time1 = runtime_AM_Open / 10;
            int time2 = runtime_PM_Close / 10;
            time = 10 * UnityEngine.Random.Range(time1 + 1, time2);
        }
        BuffTipTime = time;
        Debug.Log("BuffTip 뜨는 시간: " + BuffTipTime / 60 + ":" + (BuffTipTime - 60 * (BuffTipTime / 60)));
    */
    }
    
    private void RandomTip() {
        /*
        if (GoldManager.instance.IsBuff) return;
        if (isBuffTipOn) return;
        if (CurrentTime != BuffTipTime) return;
        if (BossManager.instance.isBossStage) return;
        TipManager.instance.ShowTip(TipType.Buff);
        isBuffTipOn = true;
    */
    }

    public void StartTanning()
    {
        //TODO 테닝시작 프로세스
        GoldManager.instance.IsBuff = false;
        // 썬탠 팝업을 띄운다.
        UIManager.instance.OpenMiniGameTanningPopup();
        // 썬탠 게임을 시작한다.
        //isTanning = true;
    }

    public void StopTanning()
    {
        
    }
    
    public void StartMainGame(bool isAM) {
        // TODO 메인게임시작
        Debug.Log("OPEN START");
        
        
        
        AudioManager.instance?.PlayBGM();
        // 마스터냥 초기화.
        if (isAM) MasterNyang.instance.SetTanningBack(0);
        // 배경 설정.
        if (isAM) BackgroundManager.instance.SetAM();
        else BackgroundManager.instance.SetPM();
        // 손님 스위치 On.
        Debug.Log("OPEN END");
    }

    public void Init()
    {
        _uiManager.SetMaxTime(_timeManager.PlayTime);
    }
    
    public void SetUpEvent()
    {
        EventBuffActivate += _backgroundManager.OnBuffActivate;
        EventBuffDeactivate += _backgroundManager.OnBuffDeactivate;
        _timeManager.EventLeftTimeChanged += _uiManager.OnLeftTimeChanged;
    }

    public void CleanUpEvent()
    {
        EventBuffActivate -= _backgroundManager.OnBuffActivate;
        EventBuffDeactivate -= _backgroundManager.OnBuffDeactivate;
        _timeManager.EventLeftTimeChanged -= _uiManager.OnLeftTimeChanged;
    }

    public bool IsBuffAvailable { get; private set; }
    
    public void BuffActivate()
    {
        IsBuffAvailable = true;
        EventBuffActivate();
    }

    public void BuffDeactivate()
    {
        EventBuffDeactivate();
        IsBuffAvailable = false;
    }

}
