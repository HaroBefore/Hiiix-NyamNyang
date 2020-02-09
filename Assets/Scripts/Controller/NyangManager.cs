using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum NyangRank {
    Normal,
    Rare,
    Hidden,
}

public enum NyangPosition {
    A, B, C, D, E, F, Boss
}

public enum NyangState {
    Wait = 0,
    Selected,
    Order, // (=Seated)
    Happy,
    Angry,
    Buff,
}

public class NyangManager : MonoBehaviour {

    public static NyangManager instance;

    [Header("Angry Guage")]
    public GameObject AngryGuage;
    [Header("냥이 생성 주기")]
    public float nyangSpawnCycle;
    private float nyangCurrentCycle;

    private bool isTitle = false;
    
    // 현재 대기중인 냥이 리스트.
    public List<Nyang> nyangList { get; protected set; }

    public Dictionary<int, GameObject> nyangPrefabDic { get; protected set; }

    // 위치 번호 (A-F)에 해당하는 좌표값.
    public Dictionary<NyangPosition, Vector2> nyangPositionDic { get; protected set; }
    // 위치 번호 (A-F)에 생성된 냥이.
    public Dictionary<NyangPosition, Nyang> nyangInPositionDic;
    private Dictionary<NyangPosition, Nyang> previousNyangInPositionDic;    // 전에 생성된 냥이.
    private Dictionary<NyangPosition, Nyang> prepreviousNyangInPositionDic; // 전전에 생성된 냥이.
    // 현재 선택된 냥이.
    private Nyang selectedNyang;
    // 손님석에 착석한 냥이. = 주문하고 있는 냥이.
    public Nyang orderNyang { get; set; }

    private Coroutine _spawnRoutine;
    
    void Awake() {
        if (!instance) instance = this;

        if (SceneManager.GetActiveScene().name == "Title") isTitle = true;
    }

    void Start() {
        LoadNyangPrefab();

        nyangList = new List<Nyang>();

        nyangPositionDic = new Dictionary<NyangPosition, Vector2>();
        nyangPositionDic.Add(NyangPosition.A, FindObjectOfType<BorderManager>().nyangPositionA);
        nyangPositionDic.Add(NyangPosition.B, FindObjectOfType<BorderManager>().nyangPositionB);
        nyangPositionDic.Add(NyangPosition.C, FindObjectOfType<BorderManager>().nyangPositionC);
        nyangPositionDic.Add(NyangPosition.D, FindObjectOfType<BorderManager>().nyangPositionD);
        nyangPositionDic.Add(NyangPosition.E, FindObjectOfType<BorderManager>().nyangPositionE);
        nyangPositionDic.Add(NyangPosition.F, FindObjectOfType<BorderManager>().nyangPositionF);
        nyangPositionDic.Add(NyangPosition.Boss, FindObjectOfType<BorderManager>().nyangPositionBoss);
        nyangInPositionDic = new Dictionary<NyangPosition, Nyang>();
        for (int i = 0; i < 6; i++)
            nyangInPositionDic.Add((NyangPosition)i, null);
        previousNyangInPositionDic = new Dictionary<NyangPosition, Nyang>();
        for (int i = 0; i < 6; i++)
            previousNyangInPositionDic.Add((NyangPosition)i, null);
        prepreviousNyangInPositionDic = new Dictionary<NyangPosition, Nyang>();
        for (int i = 0; i < 6; i++)
            prepreviousNyangInPositionDic.Add((NyangPosition)i, null);
        foreach (int key in nyangPrefabDic.Keys)
            if (nyangPrefabDic[key].GetComponent<Nyang>().rank == NyangRank.Normal)
                nyangPrefabDic[key].GetComponent<Nyang>().IsCollected = true;
        nyangPrefabDic[701].GetComponent<Nyang>().IsCollected = true;

        foreach (int i in nyangPrefabDic.Keys) nyangPrefabDic[i].GetComponent<Nyang>().SetData();

        if (isTitle) return;
        
        // Callback 함수 등록.
        FindObjectOfType<InputManager>().RegisterCallback_TouchTargetChanged(SelectNyang);
        FindObjectOfType<InputManager>().RegisterCallback_TouchTargetChanged(DeselectNyang);
    }

    public void BeginSpawn()
    {
        Debug.Log("BeginSpawn");
        _spawnRoutine = StartCoroutine(CoSpawn());
    }

    private IEnumerator CoSpawn()
    {
        while (true)
        {
            yield return null;
            SpawnNyang();
        }
    }

    public void EndSpawn()
    {
        Debug.Log("EndSpawn");
        StopCoroutine(_spawnRoutine);
    }

    private void LoadNyangPrefab() {
        nyangPrefabDic = new Dictionary<int, GameObject>();
        Nyang[] objs = Resources.LoadAll<Nyang>("Prefabs/Nyang/") as Nyang[];
        for (int i = 0; i < objs.Length; i++) {
            nyangPrefabDic.Add(objs[i].index, objs[i].gameObject);
        }
    }

    #region Spawn Nyang

    // SpawnNyang: 냥이 생성.
    private void SpawnNyang() {
        // 냥이 생성 주기 처리. (대기냥이가 없으면 주기와 관계없이 0.5초 후 스폰한다.)
        if (nyangCurrentCycle < nyangSpawnCycle) {
            if (nyangList.Count != 0) nyangCurrentCycle += TimeManager.DeltaTime;
            else nyangCurrentCycle += (TimeManager.DeltaTime * 2 * nyangSpawnCycle);
            return;
        }
        // 위치 랜덤 설정.
        NyangPosition position = (NyangPosition)(Random.Range(0, 6));
        // 위치에 해당하는 냥이 찾기.
        List<Nyang> availableNyangList = new List<Nyang>();
        foreach (int i in nyangPrefabDic.Keys) {
            Nyang nyang = nyangPrefabDic[i].GetComponent<Nyang>();
            if (nyang.position == position) availableNyangList.Add(nyang);
        }
        if (availableNyangList.Count == 0) return;
        // 획득 못한 냥이 거르기.
        for (int i = 0; i < availableNyangList.Count; i++) {
            Nyang nyang = availableNyangList[i];
            if (!nyang.IsCollected) availableNyangList.Remove(nyang);
        }
        if (availableNyangList.Count == 0) return;
        // 등급별로 냥이 나누기.
        List<Nyang> normalNyangList = new List<Nyang>();
        List<Nyang> rareNyangList = new List<Nyang>();
        List<Nyang> hiddenNyangList = new List<Nyang>();
        for (int i = 0; i < availableNyangList.Count; i++) {
            Nyang nyang = availableNyangList[i];
            switch (nyang.rank) {
                case NyangRank.Normal:
                    normalNyangList.Add(nyang);
                    break;
                case NyangRank.Rare:
                    rareNyangList.Add(nyang);
                    break;
                case NyangRank.Hidden:
                    hiddenNyangList.Add(nyang);
                    break;
            }
        }
        // 확률에 따라 등장 랭크 결정.
        List<Nyang> spawnNyangList = null;
        int rankNumber = Random.Range(0, 100);
        if (rankNumber < 40) spawnNyangList = normalNyangList;
        else if (rankNumber < 70) spawnNyangList = rareNyangList;
        else spawnNyangList = hiddenNyangList;

        // 선별된 냥이를 더 선별.
        Nyang newNyang = null;
        if (spawnNyangList.Count == 0)
            return;
        else if (spawnNyangList.Count == 1)
            newNyang = SpawnNyang(spawnNyangList[0].index);
        else if (spawnNyangList.Count == 2) {
            // 선별된 냥이 중 이전에 등장한 냥이가 있다면 제외.
            Nyang preNyang = previousNyangInPositionDic[position];
            if (preNyang != null) {
                if (spawnNyangList[0].index == preNyang.index) spawnNyangList.RemoveAt(0);
                else if (spawnNyangList[1].index == preNyang.index) spawnNyangList.RemoveAt(1);
            }
            // 남은 냥이 중 무작위로 등장.
            newNyang = SpawnNyang(spawnNyangList[Random.Range(0, spawnNyangList.Count)].index);
        }
        else {
            // 선별된 냥이 중 전에, 전전에 등장한 냥이가 있다면 제외.
            Nyang preNyang = previousNyangInPositionDic[position];
            Nyang prepreNyang = prepreviousNyangInPositionDic[position];

            if(preNyang != null)
                for(int i = 0; i < spawnNyangList.Count; i++)
                    if (spawnNyangList[i].index == preNyang.index) spawnNyangList.RemoveAt(i);
            if (prepreNyang != null)
                for (int i = 0; i < spawnNyangList.Count; i++)
                    if (spawnNyangList[i].index == prepreNyang.index) spawnNyangList.RemoveAt(i);
            // 남은 냥이 중 무작위로 등장.
            newNyang = SpawnNyang(spawnNyangList[Random.Range(0, spawnNyangList.Count)].index);
        }

        // 냥이 생성 주기 초기화.
        if(newNyang) nyangCurrentCycle = 0;
    }
    private Nyang SpawnNyang(int index) {
        // 해당 냥이 정보 불러오기.
        Nyang nyang = nyangPrefabDic[index].GetComponent<Nyang>();
        // 해당 냥이를 획득했는지 검사.
        if (!nyang.IsCollected) return null;
        // 냥이를 생성할 자리가 비어있는지 검사.
        if (nyangInPositionDic[nyang.position]) return null;
        // 냥이를 생성.
        Nyang newNyang = Instantiate(nyang.gameObject, nyangPositionDic[nyang.position], Quaternion.identity, UIManager.instance.Main_Objects.transform).GetComponent<Nyang>();
        UIManager.instance.ResizeAndRepositionObject(newNyang.gameObject, false);
        nyangList.Add(newNyang);
        nyangInPositionDic[nyang.position] = nyang;
        nyangPrefabDic[index].GetComponent<Nyang>().VisitCount++;

        // 버프중일 때: 냥이 상태 변경.
        if (GoldManager.instance.IsBuff) newNyang.State = NyangState.Buff;
        //newNyang.gameObject.GetComponent<SpriteRenderer>().sprite = nyang.sprite[5];
        //Debug.Log(newNyang.position + "위치에 " + newNyang.name + " 생성.");
        return newNyang;
    }

    #endregion

    #region Nyang Manage

    // OutNyang: 냥이 퇴장.
    public void OutNyang() {
        NyangPosition pos = orderNyang.position;
        prepreviousNyangInPositionDic[pos] = previousNyangInPositionDic[pos];
        previousNyangInPositionDic[pos] = nyangInPositionDic[pos];
        nyangInPositionDic[pos] = null;
        orderNyang = null;
        if (!GameManager.Instance.IsTutorial) AngryGuageOff();
        else TutorialManager.instance.AngryGuageOff();
    }

    // SitNyang: 냥이 앉히기. 
    private void SitNyang() {
        // 냥이를 손님석에 고정시키고, 대기리스트에서 뺀 후,
        selectedNyang.transform.position = BorderManager.instance.customerSeatPosition;
        orderNyang = selectedNyang;
        nyangList.Remove(selectedNyang);
        // 주문을 받는다.
        TakeOrder();
    }

    // TakeOrder: 냥이에게 주문받기.
    public void TakeOrder() {
        // 상태를 바꾸고 CookManager에게 주문 전달.
        orderNyang.State = NyangState.Order;
        CookManager.instance.TakeOrder();
    }

    // BuffNyang: 현재 대기중인 냥이 스프라이트 -> 버프로
    public void BuffNyang(bool isBuff) {
        foreach (Nyang nyang in nyangList) {
            if (isBuff)
                nyang.State = NyangState.Buff;
            //nyang.gameObject.GetComponent<SpriteRenderer>().sprite = nyang.sprite[5];
            else
                nyang.State = NyangState.Wait;
            //nyang.gameObject.GetComponent<SpriteRenderer>().sprite = nyang.sprite[0];
        }
    }

    // SelectNyang: InputManager의 TouchTarget이 바뀔 때, Target이 냥이면 냥이 선택.
    private void SelectNyang(GameObject target) {
        // Target이 Nyang이면 냥이 선택. (대기중인 냥이만!)
        if (!target) return;
        if (!(target.tag == "Nyang")) return;
        if (!((target.GetComponent<Nyang>().State == NyangState.Wait)||(target.GetComponent<Nyang>().State == NyangState.Buff))) return;
        AudioManager.Instance?.PlayNyang();
        selectedNyang = target.GetComponent<Nyang>();
        
        // 냥이 상태 변경 -> Selected.
        selectedNyang.State = NyangState.Selected;
        // 냥이 드래그 On.
        InputManager.instance.DragOn();
    }
    // DeselectNyang: 냥이가 선택된 상태이고 Target이 Null로 바뀔 때, 냥이 선택 해제.
    private void DeselectNyang(GameObject target) {
        if(selectedNyang && !target) {
            // 냥이를 손님석에 앉혔을 때: 커서가 손님석에 위치하고 있을 때. (손님석이 비었을 때)
            if (BorderManager.instance.IsInBorder(BorderManager.instance.customerSeatBorder) && !orderNyang) {
                // 냥이를 앉힌다.
                SitNyang();
                // 분노 게이지 On.
                AngryGuageOn();
            }
            // 냥이를 손님석에 앉히지 못했을 때:
            else {
                // 냥이를 대기상태로 되돌리고, 위치를 되돌린다.
                selectedNyang.State = (GoldManager.instance.IsBuff) ? NyangState.Buff : NyangState.Wait;
                selectedNyang.transform.position = nyangPositionDic[selectedNyang.position];
            }
            selectedNyang = null;
        }
    }

    #endregion

    #region AngryGuage

    public void AngryGuageOn() {
        AngryGuage.SetActive(true);
    }
    public void AngryGuageOff() {
        AngryGuage.SetActive(false);
    }
    public void SetAngryGuage(float f) {
        AngryGuage.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(28, 34 + 56 * (f / TimeManager.Instance.waitingTime));
    }

    #endregion
}
