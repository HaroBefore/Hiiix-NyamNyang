using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour {

    public Image FadeMask;

    void Awake() {

    }

    void Start() {
        AudioManager.instance?.PlayBGM();
    }
    public void StartGame() {
        StartCoroutine("FadeOut");
    }


    private IEnumerator FadeOut() {
        FadeMask.gameObject.SetActive(true);
        Color color = new Color(44 / 255f, 41 / 255f, 42 / 255f, 0);
        float curTime = 0;
        while (curTime < 1) {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, curTime);
            FadeMask.color = color;
            yield return null;
        }
        SceneManager.LoadScene("MainGame");
    }

}