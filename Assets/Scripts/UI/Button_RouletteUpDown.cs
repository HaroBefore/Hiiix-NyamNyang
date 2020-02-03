using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_RouletteUpDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

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

            InputManager.instance.AsdadSwitch();
            if (btnDelay < BtnDelay) { btnDelay += Time.deltaTime; return; }

            

            btnDelay = 0;

            btnCount++;
            if (BtnDelay > 0.02f) {
                if ((btnCount % 10) == 0) BtnDelay *= 0.5f;
            }
            if (btnCount < 11) amount = 1;
            else if (btnCount < 20) amount = 10;
            else if (btnCount < 29) amount = 100;
            else if (btnCount < 38) amount = 1000;
            else amount = 10000;
            AudioManager.instance?.Play(AudioManager.instance.button01);
            if (isUpButton) RouletteManager.instance.BettingUp(amount);
            else RouletteManager.instance.BettingDown(amount);
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