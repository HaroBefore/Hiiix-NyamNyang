using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CookFood : MonoBehaviour {

    // 재료.
    public Ingredient meat;
    public Ingredient powder;
    public Ingredient sauce;

    private GameObject powderObject;
    private GameObject sauceObject;
    
    // 요리 가격.
    public int price;
    // 요리 단계.
    public CookStep step { get; set; }
    // 요리가 탔는지 여부.
    private bool isOvercooked;
    // 요리 조리 시간.
    private float cookTime;
    // 요리가 맞는지 여부.
    public bool isRecipe { get; protected set; }


    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        InputManager.instance.RegisterCallback_TouchTargetChanged(SellFood);
    }

    void Update() {
        FoodOverCook();
    }

    // SetMeat: Meat 설정 및 sprite 설정.
    public void SetMeat(Ingredient _meat) {
        AudioManager.Instance?.Play(AudioManager.Instance.select_meat, 1f);
        AudioManager.Instance?.PlayCookMeat();
        meat = _meat;
        spriteRenderer.sprite = meat.sprites[0];
        isRecipe = CookManager.instance.orderedRecipe[0] == meat;
        price += (int)(meat.price * GoldManager.instance.getFactor());
    }
    // SetPowder: Powder 설정, 조리.
    public void SetPowder(Ingredient _powder) {
        AudioManager.Instance?.Play(AudioManager.Instance.select_powder, 1f);
        if (step == CookStep.Overcooked) return;
        powder = _powder;
        Cook_Powder();
        isRecipe = CookManager.instance.orderedRecipe[1] == powder;
        price += (int)(powder.price * GoldManager.instance.getFactor());
    }
    // SetSauce: Sauce 설정, 조리.
    public void SetSauce(Ingredient _sauce) {
        AudioManager.Instance?.Play(AudioManager.Instance.select_sauce, 1f);
        if (step == CookStep.Overcooked) return;
        sauce = _sauce;
        Cook_Sauce();
        isRecipe = CookManager.instance.orderedRecipe[2] == sauce;
        price += (int)(sauce.price * GoldManager.instance.getFactor());
    }

    public void Cook() {
        if (isOvercooked) return;
        if ((int)step > 5) return;
        step++;
        spriteRenderer.sprite = meat.sprites[(int)step];
        if (AudioManager.Instance) AudioManager.Instance.PlayGrill();
        if (step == CookStep.Turn07) Cook_FinishGrill();
    }
    public void Cook_FinishGrill() {
        powderObject = new GameObject("Powder");
        powderObject.transform.parent = this.transform;
        powderObject.transform.localPosition = meat.positions[0];
        UIManager.instance.ResizeAndRepositionObject(powderObject, false);
        SpriteRenderer spriteRenderer = powderObject.AddComponent<SpriteRenderer>().GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = meat.sprites[7];
        spriteRenderer.color = Color.black;
        spriteRenderer.sortingLayerName = "OnNyang";
        spriteRenderer.sortingOrder = 3;
        sauceObject = new GameObject("Sauce");
        sauceObject.transform.parent = this.transform;
        sauceObject.transform.localPosition = meat.positions[1];
        UIManager.instance.ResizeAndRepositionObject(sauceObject, false);
        spriteRenderer = sauceObject.AddComponent<SpriteRenderer>().GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = meat.sprites[8];
        spriteRenderer.color = Color.black;
        spriteRenderer.sortingLayerName = "OnNyang";
        spriteRenderer.sortingOrder = 3;
        // 소스 선택 창 띄우기.
        if (!GameManager.Instance.IsTutorial) UIManager.instance.OpenSauceSelectPanel();
        else TutorialManager.instance.OpenSauceSelectPanel();
    }
    public void Cook_Powder() {
        powderObject.GetComponent<SpriteRenderer>().color = powder.color;
        Cook_FinishAll();
    }
    public void Cook_Sauce() {
        sauceObject.GetComponent<SpriteRenderer>().color = sauce.color;
        Cook_FinishAll();
    }
    public void Cook_FinishAll() {
        if (powder && sauce) {
            UIManager.instance.CloseSauceSelectPanel();
            step++;
        }
    }

    public void SellFood()
    {
        SellFood(this.gameObject);
    }
    
    // SellFood: InputManager의 TouchTarget이 바뀔 때, Target이 조리완료된 요리면 요리 판매.
    public void SellFood(GameObject target) {
        // Target이 Food인지 확인.
        if (!target) return;
        if (!(target.tag == "Food")) return;

        // 조리가 끝난 Food인지 확인.
        if (!(target.GetComponent<CookFood>().step == CookStep.Complete)) return;
        AudioManager.Instance?.StopCookMeat();
        // 요리가 레시피와 맞는지 검사.
        isRecipe = (CookManager.instance.orderedRecipe[0] == meat) && (CookManager.instance.orderedRecipe[1] == powder) && (CookManager.instance.orderedRecipe[2] == sauce);

        // 요리를 태웠는지 검사.
        if (step == CookStep.Overcooked) isRecipe = false;

        InputManager.instance.UnregisterCallback_TouchTargetChanged(SellFood);

        // 결과를 냥이에게 전달.
        if (NyangManager.Instance.orderNyang) NyangManager.Instance.orderNyang.GiveNyang(isRecipe, this);
    }

    // FoodOverCook: 요리 시간이 너무 오래 지나면 고기가 탄다.
    private void FoodOverCook() {
        if (GameManager.Instance.IsTutorial) return;
        if (cookTime < TimeManager.Instance.cookTime) {
            cookTime += TimeManager.DeltaTime;
            return;
        }
        if (!isOvercooked) {
            AudioManager.Instance?.Play(AudioManager.Instance.over_meat, 6f);
            // 외형 변경.
            spriteRenderer.sprite = meat.sprites[9];
            // 단계 변경.
            step = CookStep.Overcooked;
            isOvercooked = true;
        }
    }
}