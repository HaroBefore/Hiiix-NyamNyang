using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private InputManager _inputManager;
    private TimeManager _timeManager;
    private AchievementManager _achievementManager;
    private BackgroundManager _backgroundManager;
    private UIManager _uiManager;
    private NyangListManager _nyangListManager;
    private NyangManager _nyangManager;
    private ResultManager _resultManager;
    private TanningManager _tanningManager;
    private TipManager _tipManager;
    private GoldManager _goldManager;
    private CookManager _cookManager;
    private TutorialManager _tutorialManager;
    private ScenarioManager _scenarioManager;

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
    public TimeType TimeType
    {
        get => _timeType;
        private set => _timeType = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = InputManager.instance;
        _timeManager = TimeManager.Instance;
        _achievementManager = FindObjectOfType<AchievementManager>();
        _backgroundManager = FindObjectOfType<BackgroundManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _nyangListManager = FindObjectOfType<NyangListManager>();
        _nyangManager = FindObjectOfType<NyangManager>();
        _resultManager = FindObjectOfType<ResultManager>();
        _tanningManager = FindObjectOfType<TanningManager>();
        _tipManager = FindObjectOfType<TipManager>();
        _goldManager = FindObjectOfType<GoldManager>();
        _cookManager = FindObjectOfType<CookManager>();
        _tutorialManager = FindObjectOfType<TutorialManager>();
        _scenarioManager = FindObjectOfType<ScenarioManager>();
        
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

    [Button]
    public void StartMainGame(TimeType timeType) {
        // TODO 메인게임시작
        Debug.Log("OPEN START");

        AudioManager.Instance?.PlayBGM();
        _timeManager.SetTime();
        _goldManager.IncomeAm = 0;
        _goldManager.IncomePm = 0;

        _timeManager.Resume();
        
        switch (timeType)
        {
            case TimeType.AM:
                // 마스터냥 초기화.
                MasterNyang.instance.SetTanningBack(0);
                // 배경 설정.
                BackgroundManager.instance.SetAM();
                break;
            case TimeType.PM:
                BackgroundManager.instance.SetPM();
                break;
        }
        
        _nyangManager.BeginSpawn();
        // 손님 스위치 On.
        Debug.Log("OPEN END");
    }

    [Button]
    public void EndMainGame(TimeType timeType)
    {
        _nyangManager.EndSpawn();
        _nyangManager.ClearAllNyang();
        _cookManager.ThrowOut();
        _uiManager.CloseAllUI();
        _uiManager.CloseRecipe();
        _uiManager.AngryGuage.SetActive(false);
        
        switch (timeType)
        {
            case TimeType.AM:
                StartTanning();
                TimeType = TimeType.PM;
                break;
            case TimeType.PM:
                _resultManager.OpenResult();
                break;
        }
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetScore(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_leaderboard, success =>
        {
            if (success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void Init()
    {
        _uiManager.SetMaxTime(_timeManager.PlayTime); 
        //StartMainGame(TimeType.AM);
        
        // Callback 함수 등록.
        _inputManager.RegisterCallback_TouchTargetChanged(_nyangManager.SelectNyang);
        _inputManager.RegisterCallback_TouchTargetChanged(_nyangManager.DeselectNyang);

        _timeManager.Pause();
        _timeManager.Day = PlayerPrefs.GetInt("DayCount", 0);
        _scenarioManager.PlayScenario();
        //_tutorialManager.PlayTutorial();
    }
    
    public void SetUpEvent()
    {
        EventBuffActivate += _backgroundManager.OnBuffActivate;
        EventBuffDeactivate += _backgroundManager.OnBuffDeactivate;
        _timeManager.EventLeftTimeChanged += _uiManager.OnLeftTimeChanged;
        _timeManager.EventTimeOver += OnTimeOver;

        _resultManager.EventOpenResult += OnOpenResult;
        _tanningManager.EventEndTanning += OnEndTanning;

        _goldManager.EventIncomeAmChanged += _resultManager.SetIncomeAm;
        _goldManager.EventIncomePmChanged += _resultManager.SetIncomePm;
        _goldManager.EventTotalGoldChanged += _resultManager.SetIncomeTotal;

        _scenarioManager.EventCloseScenario += OnCloseScenario;

        _tutorialManager.EventEndTutorial += OnEndTutorial;
    }

    public void CleanUpEvent()
    {
        EventBuffActivate -= _backgroundManager.OnBuffActivate;
        EventBuffDeactivate -= _backgroundManager.OnBuffDeactivate;
        _timeManager.EventLeftTimeChanged -= _uiManager.OnLeftTimeChanged;
        _timeManager.EventTimeOver -= OnTimeOver;

        _resultManager.EventOpenResult -= OnOpenResult;
        _tanningManager.EventEndTanning -= OnEndTanning;
        
        _goldManager.EventIncomeAmChanged -= _resultManager.SetIncomeAm;
        _goldManager.EventIncomePmChanged -= _resultManager.SetIncomePm;
        _goldManager.EventTotalGoldChanged -= _resultManager.SetIncomeTotal;

        _scenarioManager.EventCloseScenario -= OnCloseScenario;
        
        _tutorialManager.EventEndTutorial += OnEndTutorial;

    }

    public bool IsBuffAvailable { get; private set; }

    private void OnTimeOver()
    {
        EndMainGame(TimeType);
    }

    private void OnOpenResult()
    {
        _uiManager.SetActiveAllMainUi(false);
    }

    private void OnEndTanning()
    {
        Debug.Log("OnEndTanning");
        _uiManager.SetActiveAllMainUi(false);
        _uiManager.MiniGame_Tanning_UI.SetActive(false);
        _tipManager.UnhideTip();
    }

    private void OnCloseScenario()
    {
        _tutorialManager.PlayTutorial();
    }

    private void OnEndTutorial()
    {
        StartMainGame(TimeType.AM);
    }
    
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
