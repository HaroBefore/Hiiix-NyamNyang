using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour {

    public static BossManager instance;

    public GameObject roof;
    public GameObject roofBroken;
    public GameObject angry;
    // BossSceneObjects.
    public GameObject bossScene;
    public GameObject bossRecipeBox;
    public GameObject bossRecipe;
    public GameObject bossAngryGauge;
    public Text bossCount;

    public GameObject bone;
    public Sprite boss_bone;
    public Sprite bone_origin;


    public bool isBossStage { set; get; }


    public int maxCount;


    private int count;
    public int Count {
        get { return count; }
        set {
            count = value;
            bossCount.text = value.ToString() + "/" + maxCount.ToString();
            PlayerPrefs.SetInt("Boss_Count", value);
        }
    }


    private Nyang currentBugNyang;

    private int BossTime;
    private float _bossTime;
    private float bossTime {
        get { return _bossTime; }
        set {
            _bossTime = value;
            PlayerPrefs.SetFloat("Boss_AngryTime", value);
        }
    }

    void Awake() {
        if (!instance) instance = this;
        BossTime = TimeManager.instance.bossCloseTime - TimeManager.instance.runtime_AM_Open;
    }
    void Update() {
        if (isBossStage) OverWaitNyang();
    }

    public void SpawnBugNyang() {
        GameObject nyangPrefab = NyangManager.instance.nyangPrefabDic[701];
        Nyang nyang = nyangPrefab.GetComponent<Nyang>();
        if (!nyang.IsCollected) return;
        currentBugNyang = Instantiate(nyangPrefab, NyangManager.instance.nyangPositionDic[nyang.position], Quaternion.identity, UIManager.instance.Main_Objects.transform).GetComponent<Nyang>();
        UIManager.instance.ResizeAndRepositionObject(currentBugNyang.gameObject);
        nyang.VisitCount++;
        NyangManager.instance.orderNyang = currentBugNyang;
        CookManager.instance.TakeOrder();

        bossTime = PlayerPrefs.GetFloat("Boss_AngryTime");
        Count = PlayerPrefs.GetInt("Boss_Count");

        bone.GetComponent<SpriteRenderer>().sprite = boss_bone;

        AngryGaugeOn();
    }



    public void ShowRecipe() {
        Ingredient[] recipe = CookManager.instance.orderedRecipe;
        bossRecipe.GetComponent<SpriteRenderer>().sprite = recipe[0].sprites[6];
        Transform powder = bossRecipe.transform.GetChild(0);
        powder.GetComponent<SpriteRenderer>().sprite = recipe[0].sprites[7];
        powder.localPosition = recipe[0].positions[0];
        powder.Translate(0, 0, -1);
        powder.GetComponent<SpriteRenderer>().color = recipe[1].color;
        Transform sauce = bossRecipe.transform.GetChild(1);
        sauce.GetComponent<SpriteRenderer>().sprite = recipe[0].sprites[8];
        sauce.localPosition = recipe[0].positions[1];
        sauce.Translate(0, 0, -1);
        sauce.GetComponent<SpriteRenderer>().color = recipe[2].color;
        bossRecipeBox.SetActive(true);
        bossRecipe.SetActive(true);
        CookManager.instance.OpenMeatBox();
    }
    public void CloseRecipe() {
        bossRecipe.SetActive(false);
        bossRecipeBox.SetActive(false);
    }

    public void GiveNyang() {
        if (currentBugNyang.State == NyangState.Happy) {
            Count++;
        }
        else if (currentBugNyang.State == NyangState.Angry) {
            bossTime += BossTime * 0.04f;
        }
        if (Count < maxCount)
            Invoke("Reorder", 1.5f);
        else {
            currentBugNyang.GetComponent<SpriteRenderer>().sprite = currentBugNyang.sprite[5];
            Invoke("ClearBugNyang", 1.5f);
        }
    }
    // GiveNyang 뒤 1.5초 후 호출됨.
    public void Reorder() {
        // 제한 시간 다 끝나면 이 함수를 수행하지 않는다.
        if (!isBossStage) return;
        // 다시 대기(주문) 상태로.
        currentBugNyang.State = NyangState.Order;
        // 지난 주문 마무리.
        CookManager.instance.FinishCook();
        CloseRecipe();
        if (currentBugNyang.nyangFood) Destroy(currentBugNyang.nyangFood);
        // 새로운 주문 받기 명령.
        CookManager.instance.TakeOrder();
    }
    public void ClearBugNyang() {
        // 버그냥이가 간다.
        NyangManager.instance.orderNyang = null;
        if (currentBugNyang != null) Destroy(currentBugNyang.gameObject);
        currentBugNyang = null;
        AngryGaugeOff();
        CloseRecipe();
        isBossStage = false;
        // 시간 멈추기.
        TimeManager.instance.SetGameTime_Stop();
        TimeManager.instance.SetTime_Stop();
        TimeManager.instance.SetTime(TimeManager.instance.bossCloseTime);
        // 정산을 시작한다.
        BugNyangResult();
        Count = 0;
        bone.GetComponent<SpriteRenderer>().sprite = bone_origin;
    }
    public void BugNyangResult() {
        // 수익 전달.
        ResultManager.instance.SetIncomeBoss(GoldManager.instance.IncomeBoss);
        bool isClear = Count >= maxCount;
        // 남은 팁 시간 계산.
        int Tip = (isClear) ? (int)(BossTime - bossTime) * 10 : 0;
        ResultManager.instance.SetIncomeTip(Tip);
        // 버그냥이가 힘자랑함. -> 수리비 점점 크게...(TODO)
        int repairCost = (isClear) ? 0 : 3000;
        GoldManager.instance.CurrentGold -= repairCost;
        ResultManager.instance.SetRepairCost(repairCost);
        // 정산창 열기.
        ResultManager.instance.OpenBossResult();

        bossTime = 0;
        Count = 0;
    }
    
    private IEnumerator BreakRoof2() {
        currentBugNyang.GetComponent<SpriteRenderer>().sprite = currentBugNyang.sprite[5];
        float originHeight = currentBugNyang.transform.position.y;

        float per = 0;
        float t = 0;
        float T = 0.2f;
        float A = originHeight;
        float B = 0.9f * A;
        while (per<1) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / T;
            per = 0.25f * Mathf.Log((timePer + Mathf.Exp(-4))) + 1;

            Vector2 v = currentBugNyang.transform.position;
            v.y = Mathf.Lerp(A, B, per);
            currentBugNyang.transform.position = v;

            yield return null;
        }

        t = 0; T = 0.5f; A = B; B = originHeight * 2; per = 0;
        while (per < 1) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / T;
            per = 0.25f * Mathf.Log((timePer + Mathf.Exp(-4))) + 1;

            Vector2 v = currentBugNyang.transform.position;
            v.y = Mathf.Lerp(A, B, per);
            currentBugNyang.transform.position = v;

            yield return null;
        }

        t = 0; T = 0.4f; A = B; B = originHeight; per = 0;
        while (per < 1) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / T;
            float a = 3;
            per = (1 / (Mathf.Exp(a) - 1)) * (Mathf.Exp(a * timePer) - 1);

            Vector2 v = currentBugNyang.transform.position;
            v.y = Mathf.Lerp(A, B, per);
            currentBugNyang.transform.position = v;

            yield return null;
        }

        roofBroken.SetActive(true);

        t = 0; T = 0.4f; A = B; B = 0; per = 0;
        float C = roof.GetComponent<RectTransform>().localPosition.y;
        float D = C * 100 / 408;
        while (per < 1) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / T;
            float a = 1;
            per = (1 / (Mathf.Exp(a) - 1)) * (Mathf.Exp(a * timePer) - 1);

            Vector2 v = currentBugNyang.transform.position;
            v.y = Mathf.Lerp(A, B, per);
            currentBugNyang.transform.position = v;

            v = roof.GetComponent<RectTransform>().localPosition;
            v.y = Mathf.Lerp(C, D, per);
            roof.GetComponent<RectTransform>().localPosition = v;

            yield return null;
        }

        angry.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        roofBroken.SetActive(false);
        angry.SetActive(false);

        // 냥이 퇴장, 정산 시작
        ClearBugNyang();
        Vector2 ve = roof.GetComponent<RectTransform>().localPosition;
        roof.GetComponent<RectTransform>().localPosition = new Vector2(ve.x, C);

    }
    private IEnumerator BreakRoof() {
        float originHeight = currentBugNyang.transform.position.y;

        float per = 0;
        float t = 0;
        float T = 0.2f;
        float A = originHeight;
        float B = 0;

        currentBugNyang.GetComponent<SpriteRenderer>().sprite = currentBugNyang.sprite[6];
        yield return new WaitForSeconds(0.4f);
        currentBugNyang.GetComponent<SpriteRenderer>().sprite = currentBugNyang.sprite[7];
        yield return new WaitForSeconds(0.8f);
        currentBugNyang.GetComponent<SpriteRenderer>().sprite = currentBugNyang.sprite[8];
        AudioManager.instance?.Play(AudioManager.instance.brokenRoof, 1f);
        t = 0; T = 0.4f; per = 0;
        float C = roof.GetComponent<RectTransform>().localPosition.y;
        float D = C + (B - A) * (Screen.width / Camera.main.orthographicSize);
        while (per < 1) {
            t += TimeManager.instance.deltaTime;
            float timePer = t / T;
            float a = 1;
            per = (1 / (Mathf.Exp(a) - 1)) * (Mathf.Exp(a * timePer) - 1);

            Vector2 v = currentBugNyang.transform.position;
            v.y = Mathf.Lerp(A, B, per);
            currentBugNyang.transform.position = v;

            v = roof.GetComponent<RectTransform>().localPosition;
            v.y = Mathf.Lerp(C, D, per);
            roof.GetComponent<RectTransform>().localPosition = v;

            yield return null;
        }

        angry.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        roofBroken.SetActive(false);
        angry.SetActive(false);

        // 냥이 퇴장, 정산 시작
        ClearBugNyang();
        Vector2 ve = roof.GetComponent<RectTransform>().localPosition;
        roof.GetComponent<RectTransform>().localPosition = new Vector2(ve.x, C);

    }

    // OverWaitNyang: 냥이가 너무 오래 기다리면 화를 내고 가버린다.
    public void OverWaitNyang() {
        if(bossTime < BossTime) {
            float deltaTime = (TimeManager.instance.deltaTime != 0) ? Time.deltaTime * 60 / TimeManager.instance.aHour : 0;
            bossTime += deltaTime;
            SetAngryGauge(bossTime);
            return;
        }
        currentBugNyang.State = NyangState.Angry;
        AngryGaugeOff();
        CloseRecipe();
        isBossStage = false;
        bossTime = 0;
        StartCoroutine(BreakRoof());
    }

    
    #region AngryGauge

    public void AngryGaugeOn() {
        bossAngryGauge.SetActive(true);
    }
    public void AngryGaugeOff() {
        bossAngryGauge.SetActive(false);
    }
    public void SetAngryGauge(float f) {
        bossAngryGauge.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(60 + 330 * (f / BossTime), 52);
    }

    #endregion

}