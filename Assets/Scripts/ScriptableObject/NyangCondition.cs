
using UnityEditor.iOS;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = nameof(NyangCondition), menuName = nameof(NyangCondition), order = 0)]
public class NyangCondition : UnityEngine.ScriptableObject
{
    private static NyangCondition _instance;

    public static NyangCondition Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<NyangCondition>($"ScriptableObject/{nameof(NyangCondition)}");
            }

            return _instance;
        }
    }

    public void DayCondition(int day) {
        switch (day) {
            case 7: CollectNyang(201); break;
            case 10: CollectNyang(203); break;
            case 30: CollectNyang(204);break;
            default: break;
        }
    }
    public void NyangCodeCondition(int nyangCode) {
        switch (nyangCode) {
            case 204: CollectNyang(202); break;
            case 208: CollectNyang(207); break;            
            default: break;
        }
    }
    public void ItemCondition(int itemCode) {
        switch (itemCode) {
            case 1103: CollectNyang(205); break;
            case 1404: CollectNyang(210); break;
            case 1405: CollectNyang(208); break;
            case 1406: CollectNyang(206); break;
            case 1409: CollectNyang(212); break;
            case 1509: CollectNyang(212); break;
            case 1510: CollectNyang(211); break;
            // case AAAA: if (Item2Check(AAAA, BBBB)) CollectNyang(CCC); break;
            // case BBBB: if (Item2Check(AAAA, BBBB)) CollectNyang(CCC); break;
            default: break;
        }
    }
    public void SellCondition(int ingredientCode) {
        switch (ingredientCode) {
            case 406: CollectNyang(209); break;
            default: break;
        }
    }

    
    private bool Item2Check(int code1, int code2) {
        return (DecoManager.instance.decoDic[code1].IsGet && DecoManager.instance.decoDic[code2].IsGet);
    }
    private void CollectNyang(int index) {
        NyangManager.Instance.nyangPrefabDic[index].GetComponent<Nyang>().IsCollected = true;
    }
}