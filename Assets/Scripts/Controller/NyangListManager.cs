using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NyangListManager : MonoBehaviour
{
    private AudioManager audioManager;

    [Header("NyangList")] public GameObject NyangListPanel;
    public GameObject NyangList01;
    public GameObject NyangList02;
    public GameObject NyangList03;
    public GameObject NyangList04;
    public GameObject NyangList05;
    public GameObject NyangList06;
    public Sprite nyangList_hideSprite;
    private List<Nyang> nyangList;
    private int nyangList_currentPage;
    private int nyangList_maxPage;
    private Nyang nyangList01;
    private Nyang nyangList02;
    private Nyang nyangList03;
    private Nyang nyangList04;
    private Nyang nyangList05;
    private Nyang nyangList06;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
    }
    
    public void OpenNyangListPanel()
    {
        audioManager.Play(audioManager.box_open, 1f);
        audioManager.PauseCookMeat();

        LoadAndSortNyangList();
        nyangList_currentPage = 0;
        nyangList_maxPage = GetNyangListMaxPage(nyangList);
        ShowNyangList(nyangList_currentPage);

        NyangListPanel.SetActive(true);
    }

    public void CloseNyangListPanel()
    {
        audioManager.Play(audioManager.box_close, 1f);
        audioManager.ResumeCookMeat();

        NyangListPanel.SetActive(false);
    }

    // LoadAndSortNyangList: 냥이 리스트 재정렬.
    private void LoadAndSortNyangList()
    {
        // 냥이 리스트 불러오기.
        Dictionary<int, GameObject> nyangDic = NyangManager.instance.nyangPrefabDic;

        // 랭크별로 냥이 분류하기.
        Dictionary<int, Nyang> normalNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> rareNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> hiddenNyang = new Dictionary<int, Nyang>();
        Dictionary<int, Nyang> bossNyang = new Dictionary<int, Nyang>();
        foreach (int key in nyangDic.Keys)
        {
            Nyang nyang = nyangDic[key].GetComponent<Nyang>();
            switch (nyang.rank)
            {
                case NyangRank.Normal:
                    normalNyang.Add(nyang.index, nyang);
                    break;
                case NyangRank.Rare:
                    rareNyang.Add(nyang.index, nyang);
                    break;
                case NyangRank.Hidden:
                    hiddenNyang.Add(nyang.index, nyang);
                    break;
            }
        }

        // 최종 리스트
        nyangList = new List<Nyang>();
        foreach (int index in normalNyang.Keys)
        {
            nyangList.Add(normalNyang[index]);
        }

        foreach (int index in rareNyang.Keys)
        {
            nyangList.Add(rareNyang[index]);
        }

        foreach (int index in hiddenNyang.Keys)
        {
            nyangList.Add(hiddenNyang[index]);
        }

        foreach (int index in bossNyang.Keys)
        {
            nyangList.Add(bossNyang[index]);
        }
    }

    // GetNyangListMaxPage: 냥이 리스트의 마지막 페이지를 받는다.
    private int GetNyangListMaxPage(List<Nyang> list)
    {
        return Mathf.CeilToInt(list.Count / 6.0f) - 1;
    }

    // GetNyangListofPage: 해당 페이지의 냥이 리스트를 받는다.
    private void GetNyangListofPage(int page)
    {
        nyangList01 = nyangList[6 * page];
        if (6 * page + 1 < nyangList.Count) nyangList02 = nyangList[6 * page + 1];
        else nyangList02 = null;
        if (6 * page + 2 < nyangList.Count) nyangList03 = nyangList[6 * page + 2];
        else nyangList03 = null;
        if (6 * page + 3 < nyangList.Count) nyangList04 = nyangList[6 * page + 3];
        else nyangList04 = null;
        if (6 * page + 4 < nyangList.Count) nyangList05 = nyangList[6 * page + 4];
        else nyangList05 = null;
        if (6 * page + 5 < nyangList.Count) nyangList06 = nyangList[6 * page + 5];
        else nyangList06 = null;
    }

    // ShowNyangList: 해당 페이지의 냥이 리스트를 보여준다.
    private void ShowNyangList(int page)
    {
        GetNyangListofPage(page);

        ShowNyang(NyangList01, nyangList01);
        ShowNyang(NyangList02, nyangList02);
        ShowNyang(NyangList03, nyangList03);
        ShowNyang(NyangList04, nyangList04);
        ShowNyang(NyangList05, nyangList05);
        ShowNyang(NyangList06, nyangList06);
    }

    // ShowNyang: 냥이 보여주기.
    private void ShowNyang(GameObject listObj, Nyang nyang)
    {
        // 냥이가 null값이 아니면 리스트 오브젝트 활성화.
        listObj.SetActive(nyang);
        if (nyang)
        {
            // 냥이가 한 번이라도 방문한 경우에만 냥이 모습을 보여준다.
            Image nyangImage = listObj.transform.GetChild(0).GetComponent<Image>();
            if (nyang.VisitCount > 0)
            {
                nyangImage.sprite = nyang.visitSprite;
            }
            else
                nyangImage.sprite = nyangList_hideSprite;

            // 획득하지 못한 Hidden인 경우에만 이름을 숨긴다.
            if (nyang.rank == NyangRank.Hidden && !(nyang.IsCollected))
                listObj.transform.GetChild(2).GetComponent<Text>().text = "???";
            else
                listObj.transform.GetChild(2).GetComponent<Text>().text = nyang.NyangName;
            // 냥이 방문 조건 달성한 경우 특징을 보여주고,
            if (nyang.IsCollected)
                listObj.transform.GetChild(3).GetComponent<Text>().text = nyang.personality;
            // 냥이 방문 조건을 달성하지 못했을 경우,
            else
            {
                // Normal: 그냥 / 언젠가 옴.      Rare: 등장조건 보여줌.     Hidden: 걍 숨김.
                if (nyang.rank == NyangRank.Normal)
                    listObj.transform.GetChild(3).GetComponent<Text>().text = "그냥\n언젠가 옴.";
                else if (nyang.rank == NyangRank.Rare)
                    listObj.transform.GetChild(3).GetComponent<Text>().text = nyang.condition;
                else if (nyang.rank == NyangRank.Hidden)
                    listObj.transform.GetChild(3).GetComponent<Text>().text = "???\n??";
            }
        }
    }

    public void NextNyangList()
    {
        if (nyangList_currentPage < nyangList_maxPage)
            nyangList_currentPage++;
        else nyangList_currentPage = 0;
        audioManager.Play(audioManager.button01);
        ShowNyangList(nyangList_currentPage);
    }

    public void PrevNyangList()
    {
        if (nyangList_currentPage > 0)
            nyangList_currentPage--;
        else nyangList_currentPage = nyangList_maxPage;
        audioManager.Play(audioManager.button01);
        ShowNyangList(nyangList_currentPage);
    }
}