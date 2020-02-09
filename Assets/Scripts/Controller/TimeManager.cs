using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;

public class TimeManager : MonoBehaviour
{
    public event Action EventPause = () => { };
    public event Action EventResume = () => { };
    
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

    public event EventHandler EventDayChanged = (s, a) => { };

    public class EventDayChangeArgs : EventArgs
    {
        public int Day;
    }

    public event Action<float> EventLeftTimeChanged = (f) => { };
    public event Action EventTimeOver = () => { Debug.Log("EventTimeOver"); };

    public static float DeltaTime => Time.deltaTime;
    
    // 현재 날짜.
    private int day;
    public int Day {
        get { return day; }
        set {
            day = value;
            cbOnDayChanged?.Invoke(value);
            PlayerPrefs.SetInt("DayCount", value);
            UIManager.instance.Calender.transform.GetChild(0).GetComponent<Text>().text = day.ToString();
            NyangCondition.Instance.DayCondition(value);
            EventDayChanged(this, new EventDayChangeArgs {Day = day});
        }
    }

    [Header("플레이 타임")]
    [SerializeField] private float playTime = 90f;
    public float PlayTime => playTime;

    private float _leftTime;

    public float LeftTime
    {
        get => _leftTime;
        set
        {
            _leftTime = value;
            EventLeftTimeChanged(_leftTime);
            if (_leftTime < 0f)
            {
                _leftTime = 0f;
                EventTimeOver();
            }
        }
    }

    private bool _isPause;
    public bool IsPause
    {
        get => _isPause;
        set => _isPause = value;
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

    // 콜백함수들.
    private Action<int> cbOnDayChanged;

    private void Start()
    {
        SetTime();
    }

    void Update() {
        TimeProcess();
        //TanningTime();
        //RandomTip();
        //BuffCooltimeRun();
    }

    public void SetTime()
    {
        LeftTime = PlayTime;
    }

    public void TimeProcess()
    {
        if(IsPause) return;

        if (Math.Abs(LeftTime) < Mathf.Epsilon)
        {
            return;
        }
        LeftTime -= Time.deltaTime;
    }
    
    public void Pause()
    {
        // TODO: PAUSE구현
        IsPause = true;
        EventPause();

    }

    public void Resume()
    {
        // TODO: RESUME구현
        IsPause = false;
        EventResume();
    }

    public void GameStartOrContinue() {
        ScenarioManager.instance.FadeMaskDeactive();
        if (!(PlayerPrefs.GetInt("NyamNyangTutorial") == 1049)) {
            TutorialManager.instance.PlayTutorial();
            GameManager.Instance.IsTutorial = true;
            PlayerPrefs.SetInt("NyamNyangTutorial", 1049);
            return;
        }
        else
        {
            GameManager.Instance.IsTutorial = false;
        }
        Day = 1;
    }

    #region BuffCooltime
    public void ResetBuffCooltime() {
        buffCooltime = 0;
    }
    #endregion
    

    #region Callback Manager
    public void RegisterCallback_OnDayChanged(Action<int> func) {
        cbOnDayChanged += func;
    }
    public void UnregisterCallback_OnDayChanged(Action<int> func) {
        cbOnDayChanged -= func;
    }
    #endregion
}