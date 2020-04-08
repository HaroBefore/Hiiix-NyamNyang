using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TipScreenManager : MonoBehaviour
{
    private static TipScreenManager _instance;

    public static TipScreenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TipScreenManager>();
            }

            return _instance;
        }
    }

    public CanvasGroup canvasGroup;
    public TextLocalizer textLocalizer;

    [SerializeField] private int[] arrStringIndex;

    public void Start()
    {
        canvasGroup.alpha = 0;
    }

    public static void Show()
    {
        Instance.textLocalizer.SetStringIndex(Instance.arrStringIndex[Random.Range(0, Instance.arrStringIndex.Length)]);
        Instance.textLocalizer.ReloadStrText();
        Instance.canvasGroup.gameObject.SetActive(true);
        Instance.canvasGroup.DOFade(1f, 0.5f);
    }

    public static void Hide()
    {
        Instance.canvasGroup.DOFade(0f, 0.5f).OnComplete(() => Instance.canvasGroup.gameObject.SetActive(false));
    }
}