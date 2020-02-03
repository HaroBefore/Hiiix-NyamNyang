using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_IngUpDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    
    public bool isUpButton;

    public float BtnDelay;
    private float btnDelay;
    private float originBtnDelay;

    private bool isBtnDown;

    private int btnCount;

    private int amount;

    void Start() {
        originBtnDelay = BtnDelay;
    }

    void Update() {
        if (isBtnDown) {
            if(btnDelay < BtnDelay) { btnDelay += Time.deltaTime; return; }
            

            btnDelay = 0;

            btnCount++;
            if (BtnDelay > 0.02f) {
                if ((btnCount % 10) == 0) BtnDelay *= 0.5f;
            }
            if (btnCount < 4) amount = 1;
            else if (btnCount < 9) amount = 2;
            else if (btnCount < 14) amount = 4;
            else if (btnCount < 24) amount = 5;
            else amount = 10;
            AudioManager.instance?.Play(AudioManager.instance.button01);
            UIManager.instance.IngredientPurchase_AmountUpDown(isUpButton, amount);
        }
    }



    public void OnPointerDown(PointerEventData eventData) {
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isBtnDown = false;
        btnCount = 0;
        BtnDelay = originBtnDelay;
        amount = 0;
    }
}