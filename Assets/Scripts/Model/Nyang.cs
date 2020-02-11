using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = System.Random;

public class Nyang : MonoBehaviour {

    [Header("냥이 번호 및 이름")]
    // 냥이 번호. (101~199: Normal, 201~299: Rare, 301~399: Hidden)
    public int index;
    // 냥이 이름.
    public string nyangName;
    // 냥이 한글 이름.
    public string NyangName;
    [SerializeField]
    private int nyangNameIndex;

    public int NyangNameIndex => nyangNameIndex;
    
    [Header("냥이 이미지: Wait, Selected, Order, Happy, Angry, Buff")]
    // 냥이 이미지. (wait, selected, order, happy, angry, bufforBugBye, BugFucking1, BugFucking2, BugFucking3)
    public Sprite[] sprite;
    [Header("냥이 이미지: 방명록")]
    public Sprite visitSprite;
    [Header("냥이 랭크")]
    // 냥이 랭크.
    public NyangRank rank;
    [Header("냥이 위치")]
    // 냥이 위치.
    public NyangPosition position;
    // 냥이 획득 조건.

    // 냥이 획득 여부.
    private bool isCollected;
    public bool IsCollected {
        get { return isCollected; }
        set {
            bool tempa = isCollected;
            isCollected = value;
            //TipManager.instance.TipOn(TipType.CatList);
            PlayerPrefs.SetInt("Nyang_" + index + "_isCollected", (value ? 1 : 0));
            FindObjectOfType<AchievementManager>()?.newNyangCount();
        }
    }
    
    // 냥이 방문 횟수.
    private int visitCount;
    public int VisitCount {
        get {
            return visitCount;
        }
        set {
            visitCount = value;
            PlayerPrefs.SetInt("Nyang_" + index + "_VisitCount", VisitCount);

            if (value > 0) NyangCondition.Instance.NyangCodeCondition(index);
        }
    }

    [Header("냥이 조건")] [TextArea]
    public string condition;
    [Header("냥이 특징")] [TextArea]
    // 냥이 특징.
    public string personality;

    [SerializeField]
    private int personalityIndex;
    public int PersonalityIndex => personalityIndex;

    [Header("냥이 스토리")]
    [TextArea]
    // 냥이 스토리.
    public string story;

    [SerializeField]
    private int storyIndex;

    public int StoryIndex => storyIndex;

    // 냥이 인게임 오브젝트.
    public GameObject nyangObject { get; protected set; }
    // 냥이가 받은 요리.
    public GameObject nyangFood { get; protected set; }
    // 냥이 콜라이더.
    public Collider2D nyangCollider { get; protected set; }
    // 냥이 상태. 상태가 바뀌면 sprite도 바로 교체. Wait일때만 Collider를 켬.
    private NyangState state;
    public NyangState State {
        get { return state; }
        set {
            state = value;
            spriteRenderer.sprite = sprite[(int)state];
            nyangCollider.enabled = (state == NyangState.Wait) || (state == NyangState.Buff) ? true : false;
        }
    }

    // 냥이가 기다린 시간.
    public float waitingTime { get; set; }
    // 냥이가 너무 오래 기다렸는지 여부.
    private bool isOverwait;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject canvasObject;

    [SerializeField]
    private Slider sliderWait;
    
    private bool _isWaitOrder = true;

    public bool IsWaitOrder
    {
        get => _isWaitOrder;
        set
        {
            _isWaitOrder = value;
            canvasObject.SetActive(value);
        }
    }

    private float _leftWaitOrderTime;
    
    void Awake() {
        nyangObject = this.gameObject;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        nyangCollider = this.GetComponent<Collider2D>();
        canvasObject = transform.GetChild(0).gameObject;
        sliderWait = GetComponentInChildren<Slider>();

        int selectWaitTime = UnityEngine.Random.Range(0, 3);
        Color color = Color.white;
        switch (selectWaitTime)
        {
            case 0:
                _leftWaitOrderTime = 12f;
                ColorUtility.TryParseHtmlString("#e1005b", out color);
                break;
            case 1:
                _leftWaitOrderTime = 18f;
                ColorUtility.TryParseHtmlString("#ff6b00", out color);
                break;
            case 2:
                _leftWaitOrderTime = 24f;
                ColorUtility.TryParseHtmlString("#00a887", out color);
                break;
        }
        //TODO: Color
        sliderWait.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = color;
        sliderWait.maxValue = _leftWaitOrderTime;
        sliderWait.value = _leftWaitOrderTime;
        
        canvasObject.SetActive(true);
    }

    private void Update()
    {
        if (_isWaitOrder)
        {
            if (_leftWaitOrderTime > 0f)
            {
                _leftWaitOrderTime -= TimeManager.DeltaTime;
            }
            else
            {
                _leftWaitOrderTime = 0f;
                _isWaitOrder = false;
                canvasObject.SetActive(false);
                NyangManager.Instance.OutWaitOrderNyang(this);
            }
            sliderWait.value = _leftWaitOrderTime;
        }
        
        OverWaitNyang();
    }

    public void SetData() {
        VisitCount = PlayerPrefs.GetInt("Nyang_" + index + "_VisitCount");
        IsCollected = (PlayerPrefs.GetInt("Nyang_" + index + "_isCollected")) == 1;
    }

    // GiveNyang: 냥이가 요리를 받고 떠날 때 까지의 행동.
    public void GiveNyang(bool result, CookFood food) {
        if (!isOverwait) {
            UIManager.instance.StopIngredientSelectAnimation();
            // 냥이가 요리를 받는다.
            nyangFood = food.gameObject;
            food.transform.parent = this.transform;
            food.transform.localScale = Vector2.one / 400;
            food.transform.localPosition = new Vector2(1, 1);
            // 결과에 따라 냥이의 상태를 바꿈. (Happy / Angry)
            State = result ? NyangState.Happy : NyangState.Angry;
            if (GoldManager.instance.IsBuff)
            {
                State = NyangState.Happy;
            }
            else
            {
                if (State == NyangState.Happy)
                {
                    //버프가 아니고 성공 주문 시 2초 추가
                    TimeManager.Instance.LeftTime += 2f;
                }
            }
            
            if (State == NyangState.Happy)
            {
                AudioManager.Instance?.PlayNyang_Happy();
            }
            else if (State == NyangState.Angry)
            {
                AudioManager.Instance?.PlayNyang_Angry();
            }
            // 냥이가 돈을 냄.
            NyangPay(food);
            // 1.5초 뒤 냥이 퇴장. (보스냥이라면 퇴장하지 않고 카운트만 올라감.)
            if (!(GameManager.Instance.IsTutorial)) Invoke("OutNyang", 1.5f);
        }
    }
    public void NyangPay(CookFood food) {
        int price = food.price;

        switch (GameManager.Instance.TimeType)
        {
            case TimeType.AM:
                GoldManager.instance.IncomeAm += price;
                break;
            case TimeType.PM:
                GoldManager.instance.IncomePm += price;
                break;
        }
        if (!GameManager.Instance.IsTutorial)
            UIManager.instance.ShowNyangMoneyForSeconds(price, 1.5f);
    }

    // Outnyang: 냥이 퇴장.
    public void OutNyang() {
        NyangManager.Instance.OutNyang();
        CookManager.instance.FinishCook();
        UIManager.instance.CloseRecipe();
        if (UIManager.instance.meatSelectPanel.activeSelf) UIManager.instance.CloseMeatSelectPanel();
        if (UIManager.instance.sauceSelectPanel.activeSelf) UIManager.instance.CloseSauceSelectPanel();
        if(nyangFood) Destroy(nyangFood);
        Destroy(this.gameObject);
    }

    // OverWaitNyang: 냥이가 너무 오래 기다리면 화를 내고 가버린다.
    public void OverWaitNyang() {
        if (GameManager.Instance.IsTutorial) return;
        if (State == NyangState.Order) {
            if (waitingTime < TimeManager.Instance.waitingTime) {
                waitingTime += TimeManager.DeltaTime;
                NyangManager.Instance.SetAngryGuage(waitingTime);
                return;
            }
            if (!isOverwait) {
                // 상태 변경.
                State = NyangState.Angry;
                isOverwait = true;
                // 1.5초 뒤 냥이 퇴장.
                canvasObject.SetActive(false);
                Invoke("OutNyang", 1.5f);
            }
        }
    }
}