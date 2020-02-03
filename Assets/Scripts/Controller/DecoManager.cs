using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecoType {
    Roof,
    Stove,
    Bulb,
    Sign,
    Accessory
}

public class DecoManager : MonoBehaviour {

    public static DecoManager instance;

    private GameObject[] objects;
    public GameObject roof;
    public GameObject stove;
    public GameObject bulb;
    public GameObject sign;
    public GameObject accessory;

    private Dictionary<int, Deco>[] dictionarys;
    public Dictionary<int, Deco> decoDic;
    public Dictionary<int, Deco> roofDic;
    public Dictionary<int, Deco> stoveDic;
    public Dictionary<int, Deco> bulbDic;
    public Dictionary<int, Deco> signDic;
    public Dictionary<int, Deco> accessoryDic;

    public Deco[] appliedDeco { get; protected set; }


    void Awake() {
        if (!instance) instance = this;
        LoadDecoPrefab();
    }

    void Start() {
        dictionarys = new Dictionary<int, Deco>[5] { roofDic, stoveDic, bulbDic, signDic, accessoryDic };
        appliedDeco = new Deco[5];
        objects = new GameObject[5] { roof, stove, bulb, sign, accessory };
        // 초기값 세팅.
        if (!(PlayerPrefs.GetInt("NyamNyangDeco") == 1049)) {
            roofDic[1101].IsAvailable = true;
            roofDic[1101].IsGet = true;
            roofDic[1101].IsApply = true;
            stoveDic[1201].IsAvailable = true;
            stoveDic[1201].IsGet = true;
            stoveDic[1201].IsApply = true;
            bulbDic[1301].IsGet = false;
            bulbDic[1301].IsApply = false;
            signDic[1401].IsGet = false;
            signDic[1401].IsApply = false;
            accessoryDic[1501].IsGet = false;
            accessoryDic[1501].IsApply = false;
            {
                roofDic[1102].IsAvailable = true;
                roofDic[1103].IsAvailable = true;
                roofDic[1104].IsAvailable = true;
                roofDic[1105].IsAvailable = true;
                roofDic[1106].IsAvailable = true;
                roofDic[1107].IsAvailable = true;
                roofDic[1108].IsAvailable = true;
                roofDic[1109].IsAvailable = true;
                stoveDic[1202].IsAvailable = true;
                stoveDic[1203].IsAvailable = true;
                stoveDic[1204].IsAvailable = true;
                stoveDic[1205].IsAvailable = true;
                stoveDic[1206].IsAvailable = true;
                bulbDic[1301].IsAvailable = true;
                bulbDic[1302].IsAvailable = true;
                bulbDic[1303].IsAvailable = true;
                bulbDic[1304].IsAvailable = true;
                bulbDic[1305].IsAvailable = true;
                bulbDic[1306].IsAvailable = true;
                bulbDic[1307].IsAvailable = true;
                signDic[1401].IsAvailable = true;
                signDic[1402].IsAvailable = true;
                signDic[1403].IsAvailable = true;
                signDic[1404].IsAvailable = true;
                signDic[1405].IsAvailable = true;
                signDic[1406].IsAvailable = true;
                signDic[1407].IsAvailable = true;
                signDic[1408].IsAvailable = true;
                signDic[1409].IsAvailable = true;
                signDic[1410].IsAvailable = true;
                signDic[1411].IsAvailable = true;
                signDic[1412].IsAvailable = true;
                signDic[1413].IsAvailable = true;
                accessoryDic[1501].IsAvailable = true;
                accessoryDic[1502].IsAvailable = true;
                accessoryDic[1503].IsAvailable = true;
                accessoryDic[1504].IsAvailable = true;
                accessoryDic[1505].IsAvailable = true;
                accessoryDic[1506].IsAvailable = true;
                accessoryDic[1507].IsAvailable = true;
                accessoryDic[1508].IsAvailable = true;
                accessoryDic[1509].IsAvailable = true;
                accessoryDic[1510].IsAvailable = true;
                accessoryDic[1511].IsAvailable = true;
            }
            PlayerPrefs.SetInt("NyamNyangDeco", 1049);
        }

        foreach (int i in roofDic.Keys) roofDic[i].SetData();
        foreach (int i in stoveDic.Keys) stoveDic[i].SetData();
        foreach (int i in bulbDic.Keys) bulbDic[i].SetData();
        foreach (int i in signDic.Keys) signDic[i].SetData();
        foreach (int i in accessoryDic.Keys) accessoryDic[i].SetData();
        

        for (int i = 0; i < 5; i++) {
            int index = PlayerPrefs.GetInt("Applied_" + (DecoType)i);
            if (dictionarys[i].ContainsKey(index)) {
                if(!(dictionarys[i][index].IsGet)) {
                    dictionarys[i][index].IsApply = false;
                    DeapplyDeco((DecoType)i);
                    return;
                }

                appliedDeco[i] = dictionarys[i][index];
                ApplyDeco(appliedDeco[i]);
            }
            else {
                if (i == 0) {
                    appliedDeco[i] = dictionarys[i][1101];
                    ApplyDeco(appliedDeco[i]);
                }
                else if (i == 1) {
                    appliedDeco[i] = dictionarys[i][1201];
                    ApplyDeco(appliedDeco[i]);
                }
            }
        }
    }

    private void LoadDecoPrefab() {

        decoDic = new Dictionary<int, Deco>();
        roofDic = new Dictionary<int, Deco>();
        stoveDic = new Dictionary<int, Deco>();
        bulbDic = new Dictionary<int, Deco>();
        signDic = new Dictionary<int, Deco>();
        accessoryDic = new Dictionary<int, Deco>();

        Deco[] objs = Resources.LoadAll<Deco>("Prefabs/Deco/") as Deco[];

        for (int i = 0; i < objs.Length; i++) {
            decoDic.Add(objs[i].index, objs[i]);
            switch (objs[i].type) {
                case DecoType.Roof:
                    roofDic.Add(objs[i].index, objs[i]);
                    break;
                case DecoType.Stove:
                    stoveDic.Add(objs[i].index, objs[i]);
                    break;
                case DecoType.Bulb:
                    bulbDic.Add(objs[i].index, objs[i]);
                    break;
                case DecoType.Sign:
                    signDic.Add(objs[i].index, objs[i]);
                    break;
                case DecoType.Accessory:
                    accessoryDic.Add(objs[i].index, objs[i]);
                    break;
            }
        }

    }


    // ApplyDeco: 꾸미기 아이템 적용.
    public void ApplyDeco(Deco deco) {
        int type = (int)(deco.type);
        // 기존 꾸미기 아이템 적용 해제.
        if (appliedDeco[type]) appliedDeco[type].IsApply = false;
        // 새로운 꾸미기 아이템 적용.
        appliedDeco[type] = deco;
        appliedDeco[type].IsApply = true;
        objects[type].GetComponent<SpriteRenderer>().sprite = deco.sprite;
        objects[type].SetActive(true);

        PlayerPrefs.SetInt("Applied_" + deco.type, deco.index);
    }
    // DeapplyDeco: 꾸미기 아이템 해제. (기본값으로 되돌린다.)
    public void DeapplyDeco(DecoType type) {
        if (appliedDeco[(int)type]) appliedDeco[(int)type].IsApply = false;
        switch (type) {
            case DecoType.Roof:
                appliedDeco[0] = roofDic[1101];
                ApplyDeco(roofDic[1101]);
                break;
            case DecoType.Stove:
                appliedDeco[1] = stoveDic[1201];
                ApplyDeco(stoveDic[1201]);
                break;
            case DecoType.Bulb:
                if (appliedDeco[2]) {
                    appliedDeco[2].IsApply = false;
                    appliedDeco[2] = null;
                }
                bulb.SetActive(false);
                break;
            case DecoType.Sign:
                if (appliedDeco[3]) {
                    appliedDeco[3].IsApply = false;
                    appliedDeco[3] = null;
                }
                sign.SetActive(false);
                break;
            case DecoType.Accessory:
                if (appliedDeco[4]) {
                    appliedDeco[4].IsApply = false;
                    appliedDeco[4] = null;
                }
                accessory.SetActive(false);
                break;
        }
    }

}