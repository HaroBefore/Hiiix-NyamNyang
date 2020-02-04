using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeManager : MonoBehaviour
{

    private static TimeManager _instance;

    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
            }

            return _instance;
        }
    }

    // 현재 날짜.
    private int day;
    public int Day {
        get { return day; }
        set {
            day = value;
            cbOnDayChanged?.Invoke(value);
            PlayerPrefs.SetInt("DayCount", value);
            UIManager.instance.Calender.transform.GetChild(0).GetComponent<Text>().text = day.ToString();
            NyangCondition.instance.DayCondition(value);
            if (value >= 15) achievementManager.Achievement_DayCount(15);
            else if (value >= 30) achievementManager.Achievement_DayCount(30);
        }
    }

    [Header("손님 대기 시간")]
    // 주문받은 손님이 대기하는 시간. (단위: 초)
    // TODO 손님 대기 시간 세분화
    public int waitingTime;
    [Header("요리 제한 시간")]
    // 요리 제한 시간. (단위: 초)
    public int cookTime;
    [Header("버프 대기시간")]
    public float BuffCooltime;
    private float _buffCooltime;
    private float buffCooltime {
        get { return _buffCooltime; }
        set {
            _buffCooltime = value;
            PlayerPrefs.SetFloat("BuffCoolTime", value);
            float remain = BuffCooltime - buffCooltime;
            if (remain <= 0) UIManager.instance.ToggleBuffPopup();
            int min = (int)(remain / 60);
            int sec = (int)(remain - min * 60);
            UIManager.instance.BuffCoolTimer.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }
    private bool isBuffAvailable;


    // 손님이 오는 시간인지 여부.
    public bool isNyangable { get; protected set; }

    // 썬탠중인지 여부.
    public bool isTanning { get; set; }
    
    // 장사 시작했는지 여부.
    private bool isOpen;

    private bool daySwitch;
    private AchievementManager achievementManager;

    // 콜백함수들.
    private Action<int> cbOnDayChanged;

    void Awake() {
        achievementManager = FindObjectOfType<AchievementManager>();
    }
    void Start() {
        // 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyamNyangDay") == 1049)) {
            Day = 0;
            PlayerPrefs.SetInt("NyamNyangDay", 1049);
        }
        else Day = PlayerPrefs.GetInt("DayCount");
        // 버프 쿨타임 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyamNyangBuff") == 1049)) {
            buffCooltime = BuffCooltime;
            PlayerPrefs.SetInt("NyamNyangBuff", 1049);
        }
        else buffCooltime = PlayerPrefs.GetFloat("BuffCoolTime");
        // 알람 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyangNyangAlarm") == 1049)) {
            Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/topics/alarm");
        }

        Pause();

        if (Day == 0) {
            BackgroundManager.instance.SetPM();
            ScenarioManager.instance.PlayScenario();
        }
        else {
            GameStartOrContinue();
        }
        //if (PlayerPrefs.GetInt("NyamNyangTime") == 1049)
        //    TimeManager.instance.GameContinue();
        //else
        //    TimeManager.instance.GameStart();

    }

    void Update() {
        TimeProcess();
        //TanningTime();
        //RandomTip();
        //BuffCooltimeRun();
    }
    

    public void TimeProcess()
    {
        
    }
    
    public void Pause()
    {
        // TODO: PAUSE구현
    }

    public void Resume()
    {
        // TODO: RESUME구현
    }


    #region Setting Time
    // 게임 시간을 오전오픈/오후오픈 시간으로 만든다.
    public void SetTime_AMOpen() {
        isOpen = false;
        CurrentTime = runtime_AM_Open;
    }
    public void SetTime_PMOpen() {
        CurrentTime = runtime_PM_Open;
    }

    // 게임 시간을 강제로 설정한다.
    public void SetTime(int time) {
        CurrentTime = time;
    }

    #endregion

    public void GameStartOrContinue() {
        ScenarioManager.instance.FadeMaskDeactive();
        if (!(PlayerPrefs.GetInt("NyamNyangTutorial") == 1049)) {
            TutorialManager.instance.PlayTutorial();
            IsTutorial = true;
            PlayerPrefs.SetInt("NyamNyangTutorial", 1049);
            return;
        }
        else {
            IsTutorial = false;
        }
        if (!(PlayerPrefs.GetInt("NyamNyangTime") == 1049)) {
            CurrentTime = 660;
            Day = 1;
            GameStart();

            PlayerPrefs.SetInt("NyamNyangTime", 1049);
        }
        else {
            int time = PlayerPrefs.GetInt("Time");
            if (time == runtime_AM_Open) GameStart();
            else {
                SetTime(PlayerPrefs.GetInt("Time"));
                GameContinue();
            }
        }
    }
    public void GameStart() {
        SetGameTime_Go();
        SetTime_AMOpen();
        daySwitch = true;
    }
    public void GameContinue() {
        isOpen = true;
        daySwitch = true;
        if (ScenarioManager.instance.lastScenarioType == ScenarioType.Boss) BossOpen();
        else Open(CurrentTime <= 900);

        if (CurrentTime == runtime_AM_Close || CurrentTime == runtime_PM_Close) Close();

    }

    // TanningTime: 썬탠 미니게임 하러 가기.
    private void TanningTime() {
        // 오전 장사가 끝난 후, 모든 손님이 갔을 때
        if (timeType == TimeType.Breaktime && NyangManager.instance.nyangList.Count == 0 && !NyangManager.instance.orderNyang && !isTanning) {
            // 버프 끄기.
            GoldManager.instance.IsBuff = false;
            // 썬탠 팝업을 띄운다.
            UIManager.instance.OpenMiniGameTanningPopup();
            // 썬탠 게임을 시작한다.
            isTanning = true;
        }
    }

    #region BuffCooltime
    private void BuffCooltimeRun() {
        if (isBuffAvailable) return;
        if (buffCooltime < BuffCooltime) {
            buffCooltime += Time.deltaTime;
            return;
        }
        isBuffAvailable = true;
    }
    public void ResetBuffCooltime() {
        isBuffAvailable = false;
        buffCooltime = 0;
    }
    public bool IsBuffAvailable() { return isBuffAvailable; }
    #endregion

    /*
    public bool isBuffTipOn { set; get; }
    private int BuffTipTime;
    // RandomTipOn: 하루에 한 번 랜덤하게 버프팁을 띄운다.
    private void RandomTipOn() {
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
    }
    
    private void RandomTip() {
        if (GoldManager.instance.IsBuff) return;
        if (isBuffTipOn) return;
        if (CurrentTime != BuffTipTime) return;
        if (BossManager.instance.isBossStage) return;
        TipManager.instance.ShowTip(TipType.Buff);
        isBuffTipOn = true;
    }
    */

    // 오픈 준비.
    private void OpenReady() {
        Debug.Log("OPENREADY START");
        isOpen = true;
        if (daySwitch) Day++;
        // 버프 팁 타임 설정.
        //RandomTipOn();
        // 광고 주기 설정.
        if (AdsManager.instance.IncreaseCycle()) return;
        // 장사 시작.
        Open(true);
        Debug.Log("OPENREADY END");
    }
    // 장사 시작.
    public void Open(bool isAM) {
        Debug.Log("OPEN START");
        AudioManager.instance?.PlayBGM();
        // 마스터냥 초기화.
        if (isAM) MasterNyang.instance.SetTanningBack(0);
        // 배경 설정.
        if (isAM) BackgroundManager.instance.SetAM();
        else BackgroundManager.instance.SetPM();
        // 시간 흐르기.
        if (!_isTutorial) {
            SetTime_Go();
            SetGameTime_Go();
        }
        // 손님 스위치 On.
        isNyangable = true;
        Debug.Log("OPEN END");
    }
    private void Close() {
        // 게임 시간 정지.
        SetGameTime_Stop();
        // 손님 스위치 Off.
        isNyangable = false;
    }
    public void BossOpen() {
        AudioManager.instance?.PlayBGM(AudioManager.instance.background_boss);
        // 마스터냥 초기화.
        MasterNyang.instance.SetTanningBack(0);
        // 배경 설정.
        BackgroundManager.instance.SetBoss();
        // 시간 흐르기.
        if (!_isTutorial) {
            SetTime_Go();
            SetGameTime_Go();
        }
        BossManager.instance.isBossStage = true;
        BossManager.instance.SpawnBugNyang();
        BossManager.instance.bossScene.SetActive(true);
        BackgroundManager.instance.SetBoss();
    }


    #region Callback Manager
    public void RegisterCallback_OnDayChanged(Action<int> func) {
        cbOnDayChanged += func;
    }
    public void UnregisterCallback_OnDayChanged(Action<int> func) {
        cbOnDayChanged -= func;
    }
    #endregion
}