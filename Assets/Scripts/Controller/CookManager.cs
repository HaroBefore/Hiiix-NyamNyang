using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookStep {
    Turn01 = 0,
    Turn02,
    Turn03,
    Turn04,
    Turn05,
    Turn06,
    Turn07,
    Complete,
    Overcooked,
}

public class CookManager : MonoBehaviour {

    public static CookManager instance;

    [Header("쿡팁시간")]
    public float cooktipTime;
    private float CooktipTime;
    [Header("CookFood Prefab")]
    public GameObject cookFoodPrefab;

    // 고기 조리를 시작했는지 여부.
    public bool isMeatCooking { get; protected set; }
    // 주문 받은 메뉴 레시피.
    public Ingredient[] orderedRecipe;
    // 조리중인 요리.
    public CookFood cookFood;

    public bool isMeatSwipingReset { get; protected set; }


    void Awake() {
        if (!instance) instance = this;

        // Callback 함수 등록.
        FindObjectOfType<InputManager>().RegisterCallback_SwipeTargetChanged(TurnMeat);
        isMeatSwipingReset = true;
    }
    void Update() {
        ShowCookTip();
    }

    // TakeOrder: 주문을 받는다. (일단 랜덤으로...)
    public void TakeOrder() {
        // 주문받은 레시피 설정. (일단 랜덤)
        Ingredient meat = null;
        for (int i = 0; i < 1000; i++) {
            meat = IngredientManager.instance.meatDic[Random.Range(0, IngredientManager.instance.meatDic.Count) + 401];
            if (meat.IsAvailable) break;
        }
        Ingredient powder = null;
        for (int i = 0; i < 1000; i++) {
            powder = IngredientManager.instance.powderDic[Random.Range(0, IngredientManager.instance.powderDic.Count) + 501];
            if (powder.IsAvailable) break;
        }
        Ingredient sauce = null;
        for (int i = 0; i < 1000; i++) {
            sauce = IngredientManager.instance.sauceDic[Random.Range(0, IngredientManager.instance.sauceDic.Count) + 601];
            if (sauce.IsAvailable) break;
        }
        if (GameManager.Instance.IsTutorial) {
            meat = IngredientManager.instance.meatDic[401];
            powder = IngredientManager.instance.powderDic[502];
            sauce = IngredientManager.instance.sauceDic[602];
        }
        // 레시피 설정.
        SetRecipe(meat.index, powder.index, sauce.index);
    }
    // SetRecipe: 레시피 결정.
    public void SetRecipe(int meatIndex, int powderIndex, int sauceIndex) {
        orderedRecipe = new Ingredient[3];
        orderedRecipe[0] = IngredientManager.instance.meatDic[meatIndex];
        orderedRecipe[1] = IngredientManager.instance.powderDic[powderIndex];
        orderedRecipe[2] = IngredientManager.instance.sauceDic[sauceIndex];
        OpenRecipe();
    }
    public void OpenRecipe() {
        UIManager.instance.OpenRecipe(orderedRecipe[0].index, orderedRecipe[1].index, orderedRecipe[2].index);
        if (GameManager.Instance.IsBuff)
        {
            SetFinishFood();
        }
        else
        {
            OpenMeatBox();
        }
    }

    public void SetFinishFood()
    {
        SelectMeat(orderedRecipe[0]);
        cookFood.Cook_FinishGrill();
        cookFood.SetSauce(orderedRecipe[1]);
        cookFood.SetPowder(orderedRecipe[2]);
        cookFood.Cook_FinishAll();
        cookFood.step = CookStep.Complete;
        cookFood.Invoke("SellFood", 0.5f);
    }
    
    public void OpenMeatBox() {
        // 고기 선택 창을 띄운다.
        UIManager.instance.OpenMeatSelectPanel();
    }


    // SelectMeat: 고기 선택.
    public void SelectMeat(Ingredient meat) {
        // 스토브에 고기를 올린다.
        GameObject newFood = Instantiate(cookFoodPrefab, BorderManager.instance.stovePosition, Quaternion.identity, UIManager.instance.Main_Objects.transform);
        UIManager.instance.ResizeAndRepositionObject(newFood, false);
        if (GameManager.Instance.IsTutorial) newFood.transform.parent = null;
        cookFood = newFood.GetComponent<CookFood>();
        cookFood.SetMeat(meat);
        // 고기 조리 시작.
        isMeatCooking = true;
    }

    // FinishCook: 요리 끝.
    public void FinishCook() {
        isMeatCooking = false;
        if (cookFood) Destroy(cookFood.gameObject);
        cookFood = null;
        TipManager.instance.CloseTip(TipType.Cook);
    }


    public void ResetSwipe() {
        isMeatSwipingReset = true;
    }
    private bool CheckDirection(Vector2 dir) {
        float x = dir.x;
        float y = dir.y;
        if (x == 0 && y < 0) return true;
        //if (x == 0) for (int i = 0; i < 5; i++) Debug.Log("ERROR");
        if ((Mathf.Abs(y / x) >= Mathf.Sqrt(3)) && y < 0) return true;
        else return false;
    }
    public void TurnMeat(GameObject Target) {
        if (!Target) return;
        if (!(Target.tag == "Food")) return;
        if (!isMeatSwipingReset) return;
        Vector2 dir = InputManager.instance.swipeDirection;
        if (!CheckDirection(dir)) return;
        if (!(InputManager.instance.swipeDistance >= InputManager.instance.swipeSensitivity)) return;
        if (cookFood.step < CookStep.Turn07) cookFood.Cook();

        TipManager.instance.CloseTip(TipType.Cook);
        CooktipTime = 0;
        isMeatSwipingReset = false;
        InputManager.instance.resetSwipe();
    }

    // ThrowOut: 요리 버리기.
    public void ThrowOut() {
        if (cookFood) {
            AudioManager.Instance?.StopCookMeat();
            AudioManager.Instance?.Play(AudioManager.Instance.trash, 1f);
            isMeatCooking = false;
            InputManager.instance.UnregisterCallback_TouchTargetChanged(cookFood.SellFood);
            Destroy(cookFood.gameObject);
            cookFood = null;
            TipManager.instance.CloseTip(TipType.Cook);
        }
    }


    private void ShowCookTip() {
        if (!cookFood) return;
        if (cookFood.step > CookStep.Turn06) return;
        if (GameManager.Instance.IsTutorial) return;
        if (CooktipTime < cooktipTime)
        {
            CooktipTime += TimeManager.DeltaTime;
            return;
        }
        TipManager.instance.ShowTip(TipType.Cook);
        CooktipTime = 0;
    }
}