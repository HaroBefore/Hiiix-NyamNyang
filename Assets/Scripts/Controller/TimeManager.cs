using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum TimeType {
    Ready,
    AMOpentime,
    AM,
    Breaktime,
    PMOpenTime,
    PM,
    Closetime,
}

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;

    [Header("게임 시간 1시간 현실시간 몇초인지")]
    // 게임 시간 1시간에 해당하는 실제 시간. (단위: 초)
    public int aHour;
    private float aHour_Time;
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
    // 현재 게임 시간. (단위: 분)
    private int currentTime;
    public int CurrentTime {
        get { return currentTime; }
        protected set {
            currentTime = value;
            if (!isOpen) {
                if (value == runtime_AM_Open) {
                    OpenReady();
                }
            }
            else {
                if (!(BossManager.instance.isBossStage)) {
                    if (value == runtime_AM_Close) {
                        Close();
                    }
                    if (value == runtime_PM_Open) {
                        Open(false);
                    }
                    if (value == runtime_PM_Close) {
                        Close();
                    }
                }
            }
            if (!(BossManager.instance.isBossStage)) {
                if (value < runtime_AM_Open) { timeType = TimeType.Ready; }
                else if (runtime_AM_Open == value) timeType = TimeType.AMOpentime;
                else if (runtime_AM_Open < value && value < runtime_AM_Close) timeType = TimeType.AM;
                else if (runtime_AM_Close <= value && value < runtime_PM_Open) timeType = TimeType.Breaktime;
                else if (runtime_PM_Open == value) timeType = TimeType.PMOpenTime;
                else if (runtime_PM_Open < value && value < runtime_PM_Close) timeType = TimeType.PM;
                else timeType = TimeType.Closetime;
            }
            int h = (int)(currentTime / 60);
            int m = currentTime - 60 * h;
            if (h > 12) h -= 12;
            UIManager.instance.Watch.transform.GetChild(1).GetComponent<Text>().text = string.Format("{0:00}:{1:00}", h, m);
            UIManager.instance.Watch.transform.GetChild(2).GetComponent<Text>().text = (value < 720) ? "am" : "pm";

            PlayerPrefs.SetInt("Time", value);
        }
    }
    // 현재 게임 시간 구분.
    public TimeType timeType { get; protected set; }
    [Header("오전 장사 시간")]
    // 오전 장사 시간. (단위: 분)
    public int runtime_AM_Open;
    public int runtime_AM_Close;
    [Header("오후 장사 시간")]
    // 오후 장사 시간. (단위: 분)
    public int runtime_PM_Open;
    public int runtime_PM_Close;
    [Header("보스가 가는 시간")]
    public int bossCloseTime;
    [Header("손님 대기 시간")]
    // 주문받은 손님이 대기하는 시간. (단위: 초)
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
    // 룰렛중인지 여부.
    public bool isRoulette { get; set; }


    // 게임 내 시간 deltaTime.
    public float deltaGameTime { get; protected set; }
    // 시스템 시간 deltaTime.
    public float deltaTime { get; protected set; }
    private bool isPause;
    private bool isGamePause;

    // 장사 시작했는지 여부.
    private bool isOpen;

    // 튜토리얼중인지 여부.
    private bool isTutorial;
    public bool IsTutorial {
        get { return isTutorial; }
        set {
            isTutorial = value;
            if (value) {
                SetGameTime_Stop();
                SetTime_Stop();
            }
            else {
                SetGameTime_Go();
                SetTime_Go();
            }
        }
    }

    private bool daySwitch;
    private AchievementManager achievementManager;

    // 콜백함수들.
    private Action<int> cbOnDayChanged;

    void Awake() {
        if (!instance) instance = this;
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

        BuffTipTime = -1;


        SetGameTime_Stop();

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
        if (!isPause) deltaTime = Time.deltaTime;
        if (!isGamePause) deltaGameTime = Time.deltaTime;
        TimeisRunningOut();
        TanningTime();
        RouletteTime();
        RandomTip();
        BuffCooltimeRun();
    }

    #region Time Pause/Resume
    // 게임 시간 진행/정지.
    public void SetGameTime_Go() {
        if (currentTime == runtime_AM_Close || currentTime == runtime_PM_Close) return;
        deltaGameTime = Time.deltaTime;
        isGamePause = false;
    }
    public void SetGameTime_Stop() {
        deltaGameTime = 0;
        isGamePause = true;
    }
    // 시스템 시간 진행/정지.
    public void SetTime_Go() {
        deltaTime = Time.deltaTime;
        isPause = false;
    }
    public void SetTime_Stop() {
        deltaTime = 0;
        isPause = true;
    }
    // TimeIsRuningOut: 게임 시간이 10분씩 흐르게 한다.
    private void TimeisRunningOut() {
        // 게임 시간 10분이 지날 때까지 대기.
        if (aHour_Time < aHour / 6) {
            aHour_Time += deltaGameTime;
            return;
        }
        // 게임 시간 1시간이 흐름.
        CurrentTime += 10;
        // 주기 초기화.
        aHour_Time = 0;
    }

    #endregion

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
    // RouletteTime: 룰렛 미니게임 하러 가기.
    private void RouletteTime() {
        // 오후 장사가 끝난 후, 모든 손님이 갔을 때
        if (timeType == TimeType.Closetime && NyangManager.instance.nyangList.Count == 0 && !NyangManager.instance.orderNyang && !isRoulette) {
            // 버프 끄기.
            GoldManager.instance.IsBuff = false;
            // 수익 전달.
            ResultManager.instance.SetIncome(GoldManager.instance.Income);
            // 수익 전달.
            ResultManager.instance.SetIncomeMinus(GoldManager.instance.IncomeMinus);
            // 룰렛 팝업을 띄운다.
            UIManager.instance.OpenMiniGameRoulettePopup();
            // 룰렛 게임을 시작한다.
            isRoulette = true;
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

    // 오픈 준비.
    private void OpenReady() {
        Debug.Log("OPENREADY START");
        isOpen = true;
        if (daySwitch) Day++;
        // 버프 팁 타임 설정.
        RandomTipOn();
        // 광고 주기 설정.
        if (AdsManager.instance.IncreaseCycle()) return;
        // 장사 시작.
        if (ScenarioManager.instance.lastScenarioType == ScenarioType.Boss) BossOpen();
        else Open(true);
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
        if (!isTutorial) {
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
        if (!isTutorial) {
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