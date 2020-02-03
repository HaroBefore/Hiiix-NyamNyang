using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour {

    public Sprite[] moonSprites;

    private Image spriteRenderer;

    void Awake() {
        spriteRenderer = this.GetComponent<Image>();
        SetLetterMoon();
    }

    private void SetLetterMoon() {
        int day = TimeManager.instance.Day % 15;

        if (day == 0) // 0 = 15: 보름달
            spriteRenderer.sprite = moonSprites[3];
        else if (day <= 2) // 1, 2:
            spriteRenderer.sprite = moonSprites[2];
        else if (day <= 5) // 3, 4, 5:
            spriteRenderer.sprite = moonSprites[1];
        else if (day <= 7) // 6, 7:
            spriteRenderer.sprite = moonSprites[0];
        else if (day <= 9) // 8, 9:
            spriteRenderer.sprite = moonSprites[0];
        else if (day <= 12) // 10, 11, 12:
            spriteRenderer.sprite = moonSprites[1];
        else if (day <= 14) // 13, 14:
            spriteRenderer.sprite = moonSprites[2];

        if (day > 0 && day < 8) spriteRenderer.gameObject.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        else spriteRenderer.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}