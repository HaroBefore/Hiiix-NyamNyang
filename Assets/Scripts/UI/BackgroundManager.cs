using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    public static BackgroundManager instance;
    
    [Header("Letterbox")]
    public Image letter_top;
    public Image letter_bot;
    public Image letter_wave;
    public Image letter_moon;
    public Sprite letter_top_am;
    public Sprite letter_top_pm_1;
    public Sprite letter_top_pm_2;
    public Sprite letter_top_boss;
    public Sprite letter_bot_am;
    public Sprite letter_bot_pm;
    public Sprite letter_bot_boss;
    public Sprite letter_wave_am;
    public Sprite letter_wave_pm;
    public Sprite[] letter_moon_sprites;
    private IEnumerator Coroutine_LetterPMAnimation;

    public GameObject sea;
    public GameObject parasol01;
    public GameObject parasol02;
    public GameObject parasol03;
    public GameObject parasol04;
    public GameObject sharks;

    public Color color_AM_background;
    public Color color_AM_Sea;
    public Color color_PM_background;
    public Color color_PM_Sea;
    public Color color_Boss_background;
    public Color color_Boss_Sea;
    public Color color_Buff_background;
    public Color color_Buff_Sea;

    private Camera mainCamera;
    private Image seaImage;
    private RectTransform seaRT;
    private RectTransform parasol01RT;
    private RectTransform parasol02RT;

    void Awake() {
        if (!instance) instance = this;
        mainCamera = FindObjectOfType<Camera>();
        seaImage = sea.GetComponent<Image>();
        seaRT = sea.GetComponent<RectTransform>();
        parasol01RT = parasol01.GetComponent<RectTransform>();
        parasol02RT = parasol02.GetComponent<RectTransform>();
        Coroutine_LetterPMAnimation = LetterPMAnimation();
    }

    void Start() {
        if(((float)Screen.width / (float)Screen.height) > 0.5625) {
            letter_wave.gameObject.SetActive(false);
            Canvas[] canvas = FindObjectsOfType<Canvas>();
            for(int i = 0; i < canvas.Length; i++) {
                canvas[i].GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            }
        }
    }

    public void SetAM() {
        mainCamera.backgroundColor = color_AM_background;
        seaImage.color = color_AM_Sea;
        SetLetterAM();

        SetSize_Default();
    }
    public void SetPM() {
        mainCamera.backgroundColor = color_PM_background;
        seaImage.color = color_PM_Sea;
        SetLetterPM();

        SetSize_Default();
    }
    public void SetSize_Default() {
        //seaRT.sizeDelta = new Vector2(2048, 420);
        //seaRT.localPosition = new Vector2(0, 760);

        parasol01.SetActive(true);
        parasol02.SetActive(true);
        sharks.SetActive(false);

        //parasol01RT.localPosition = new Vector2(-395.5f, 577.5f);
        //parasol02RT.localPosition = new Vector2(400, 535);
        //parasol03RT.localPosition = new Vector2();
        //parasol04RT.localPosition = new Vector2();
    }
    public void SetBoss() {
        mainCamera.backgroundColor = color_Boss_background;
        seaImage.color = color_Boss_Sea;
        SetLetterBoss();

        //seaRT.sizeDelta = new Vector2(2048, 670);
        //seaRT.localPosition = new Vector2(0, 630);

        parasol01.SetActive(false);
        parasol02.SetActive(false);
        parasol03.SetActive(false);
        parasol04.SetActive(false);
        sharks.SetActive(true);
    }
    public void SetBuff() {
        mainCamera.backgroundColor = color_Buff_background;
        seaImage.color = color_Buff_Sea;
    }
    public void SetDeco() {
        seaRT.sizeDelta = new Vector2(2048, 770);
        seaRT.localPosition = new Vector2(0, 580);
        //parasol01RT.localPosition = new Vector2(-395.5f, 245f);
        //parasol02RT.localPosition = new Vector2(400, 211);
        //parasol03RT.localPosition = new Vector2(-395.5f, 245f);
        //parasol04RT.localPosition = new Vector2(400, 211);
        sharks.SetActive(false);
    }

    public void OnBuffActivate()
    {
        SetBuff();
    }

    public void OnBuffDeactivate()
    {
        if (GameManager.Instance.TimeType == TimeType.PM)
        {
            SetPM();
        }
        else
        {
            SetAM();
        }
    }


    #region Letterbox

    private void SetLetterAM() {
        letter_top.sprite = letter_top_am;
        letter_bot.sprite = letter_bot_am;
        letter_wave.sprite = letter_wave_am;
        letter_moon.gameObject.SetActive(false);
        if (Coroutine_LetterPMAnimation != null) StopCoroutine(Coroutine_LetterPMAnimation);
    }

    private void SetLetterPM() {
        letter_top.sprite = letter_top_pm_1;
        letter_bot.sprite = letter_bot_pm;
        letter_wave.sprite = letter_wave_pm;
        letter_moon.gameObject.SetActive(true);
        SetLetterMoon();
        if (Coroutine_LetterPMAnimation != null) StartCoroutine(Coroutine_LetterPMAnimation);
    }

    private void SetLetterBoss() {
        letter_top.sprite = letter_top_boss;
        letter_bot.sprite = letter_bot_boss;
        letter_wave.sprite = letter_wave_am;
        letter_moon.gameObject.SetActive(false);
        if (Coroutine_LetterPMAnimation != null) StopCoroutine(Coroutine_LetterPMAnimation);
    }

    private void SetLetterMoon() {
        int day = TimeManager.Instance.Day % 15;

        if (day == 0) // 0 = 15: 보름달
            letter_moon.sprite = letter_moon_sprites[3];
        else if (day <= 2) // 1, 2:
            letter_moon.sprite = letter_moon_sprites[2];
        else if (day <= 5) // 3, 4, 5:
            letter_moon.sprite = letter_moon_sprites[1];
        else if (day <= 7) // 6, 7:
            letter_moon.sprite = letter_moon_sprites[0];
        else if (day <= 9) // 8, 9:
            letter_moon.sprite = letter_moon_sprites[0];
        else if (day <= 12) // 10, 11, 12:
            letter_moon.sprite = letter_moon_sprites[1];
        else if (day <= 14) // 13, 14:
            letter_moon.sprite = letter_moon_sprites[2];

        if (day > 0 && day < 8) letter_moon.gameObject.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        else letter_moon.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator LetterPMAnimation() {
        while (true) {
            letter_top.sprite = letter_top_pm_1;
            yield return new WaitForSeconds(0.75f);
            letter_top.sprite = letter_top_pm_2;
            yield return new WaitForSeconds(0.75f);
        }
    }
    #endregion
}