using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipLanguage : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, 1f)
            .SetDelay(3f)
            .OnComplete(() => gameObject.SetActive(false));
    }
}