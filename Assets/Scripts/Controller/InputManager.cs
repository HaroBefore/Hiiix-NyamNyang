using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    public float swipeSensitivity = 1f;

    // 터치할 수 있는 레이어.
    public LayerMask TouchableLayerMask;
    public LayerMask TouchSoundLayerMask;
    // 터치 시작한 위치.
    public Vector2 startTouchPos { get; protected set; }
    // 터치 손 뗀 위치.
    public Vector2 currentTouchPos { get; protected set; }
    // 터치 여부.
    public bool isTouching { get; protected set; }
    // 스와이프 방향.
    public Vector2 swipeDirection { get; protected set; }
    // 스와이프 거리.
    public float swipeDistance { get; protected set; }              // TODO:: swipeDistance > swipeSensitivity 이면 방향 갱신 시작.
    // 스와이프 여부.
    public bool isSwiping { get; protected set; }
    // 드래그 여부: true이면 [isTouching = true] 인 동안 Target이 따라오게 함.
    public bool isDragging { get; protected set; }
    // DragOn,Off
    public void DragOn() { isDragging = true; }
    public void DragOff() { isDragging = false; }




    private Action<GameObject> cbTouchTargetChanged;
    private GameObject touchTarget;
    public GameObject TouchTarget {
        get { return touchTarget; }
        protected set {
            touchTarget = value;
            cbTouchTargetChanged?.Invoke(TouchTarget);
            //if (touchTarget != null) Debug.Log("TOUCH: " + touchTarget.name);
        }
    }
    private Action<GameObject> cbSwipeTargetChanged;
    private GameObject swipeTarget;
    public GameObject SwipeTarget {
        get { return swipeTarget; }
        protected set {
            swipeTarget = value;
            cbSwipeTargetChanged?.Invoke(SwipeTarget);
        }
    }



    void Awake() {
        if (!instance) instance = this;
        isTouching = false;
    }

    void Update() {
        TouchDown();
        TouchOn();
        TouchUp();
        Swipe();
        ResetMeatSwiping();
        //if (Input.GetKeyDown(KeyCode.Z)) Debug.Log(Input.mousePosition);
        //if (isTouching) SwipeTarget = DetectTarget();
    }


    private void TouchDown() {
        if (Input.GetMouseButtonDown(0)) {
            startTouchPos = ScreenToWorldPoint(Input.mousePosition);
            currentTouchPos = startTouchPos;
            TouchTarget = DetectTarget();
            //Debug.Log(TouchTarget.name);
            isTouching = true;

            asdad2();
        }
    }

    private void TouchOn() {
        if (Input.GetMouseButton(0)) {
            currentTouchPos = ScreenToWorldPoint(Input.mousePosition);
            if (isSwipable()) SwipeTarget = DetectTarget();
            // 드래그중일 때: Target이 따라오게 함.
            if (isDragging) {
                if ((Input.mousePosition.y / Screen.height) < 0.33f) return;
                if (TouchTarget) TouchTarget.transform.position = currentTouchPos;
                else if (SwipeTarget) SwipeTarget.transform.position = currentTouchPos;
            }
        }
    }

    private void TouchUp() {
        if (Input.GetMouseButtonUp(0)) {
            //asdad();
            currentTouchPos = ScreenToWorldPoint(Input.mousePosition);
            TouchTarget = null;
            SwipeTarget = null;
            isTouching = false;
            isSwiping = false;
            DragOff();
            CookManager.instance.ResetSwipe();
        }
    }

    private void Swipe() {
        if (isSwiping || (isTouching && Vector2.Distance(startTouchPos, currentTouchPos) > swipeSensitivity)) {
            swipeDistance = Vector2.Distance(startTouchPos, currentTouchPos);
            swipeDirection = (currentTouchPos - startTouchPos).normalized;
            isSwiping = true;
        }
    }

    private GameObject DetectTarget() {
        RaycastHit2D hit = Physics2D.Raycast(currentTouchPos, transform.forward, 0, TouchableLayerMask);
        if (hit) return hit.transform.gameObject;
        else return null;
    }

    public void resetSwipe() {
        swipeDirection = Vector2.zero;
        swipeDistance = 0;
        startTouchPos = ScreenToWorldPoint(Input.mousePosition);
        currentTouchPos = ScreenToWorldPoint(Input.mousePosition);
    }

    public void ResetMeatSwiping() {
        if (isSwiping) {
            Vector2 dir = swipeDirection;
            if (dir.y <= 0) return;
            if (!CookManager.instance.isMeatSwipingReset) {
                if (((Mathf.Abs(swipeDirection.y / swipeDirection.x) >= 0.001f) && swipeDirection.y > 0) || swipeDirection == Vector2.up) {
                    CookManager.instance.ResetSwipe();
                }
            }
        }
    }


    #region callback
    // 콜백 함수 등록.
    public void RegisterCallback_TouchTargetChanged(Action<GameObject> callback) {
        cbTouchTargetChanged += callback;
    }
    public void UnregisterCallback_TouchTargetChanged(Action<GameObject> callback) {
        cbTouchTargetChanged -= callback;
    }
    public void RegisterCallback_SwipeTargetChanged(Action<GameObject> callback) {
        cbSwipeTargetChanged += callback;
    }
    public void UnregisterCallback_SwipeTargetChanged(Action<GameObject> callback) {
        cbSwipeTargetChanged -= callback;
    }
    #endregion
    public Vector2 ScreenToWorldPoint(Vector2 screenPoint) {
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }
    public bool isSwipable() {
        if (TouchTarget?.tag == "Food") return true;
        if (!TouchTarget) return true;
        return false;
    }





    public Canvas canvas; // raycast가 될 캔버스
    public Canvas canvas_not1;
    public Canvas canvas_not2;
    public Canvas canvas_not3;
    private GraphicRaycaster graphicRaycaster;
    private GraphicRaycaster graphicRaycaster_not1;
    private GraphicRaycaster graphicRaycaster_not2;
    private GraphicRaycaster graphicRaycaster_not3;
    private PointerEventData pointerEventData;

    // Use this for initialization
    void Start() {
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster_not1 = canvas_not1.GetComponent<GraphicRaycaster>();
        graphicRaycaster_not2 = canvas_not2.GetComponent<GraphicRaycaster>();
        graphicRaycaster_not3 = canvas_not3.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
    }


    bool asdadSwitch;
    public void AsdadSwitch() {
        asdadSwitch = true;
    }
    void asdad() {
        if(asdadSwitch) { asdadSwitch = false; return; }
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        List<RaycastResult> results_no1 = new List<RaycastResult>();
        List<RaycastResult> results_no2 = new List<RaycastResult>();
        List<RaycastResult> results_no3 = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);
        graphicRaycaster_not1.Raycast(pointerEventData, results_no1);
        graphicRaycaster_not2.Raycast(pointerEventData, results_no2);
        graphicRaycaster_not3.Raycast(pointerEventData, results_no3);
        if (results.Count != 0 && results_no1.Count == 0 && results_no2.Count == 0 && results_no3.Count == 0 && !SwipeTarget && !TouchTarget) {
            GameObject obj = results[0].gameObject;
            if (obj.name == "Background_Sand") AudioManager.instance?.PlaySand();
            else if (obj.name == "Sea") AudioManager.instance?.PlayWave();
        }
    }

    void asdad2() {
        if (asdadSwitch) { asdadSwitch = false; return; }
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        List<RaycastResult> results_no1 = new List<RaycastResult>();
        List<RaycastResult> results_no2 = new List<RaycastResult>();
        List<RaycastResult> results_no3 = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);
        graphicRaycaster_not1.Raycast(pointerEventData, results_no1);
        graphicRaycaster_not2.Raycast(pointerEventData, results_no2);
        graphicRaycaster_not3.Raycast(pointerEventData, results_no3);
        if (results_no1.Count != 0 || results_no2.Count != 0 || results_no3.Count != 0 || SwipeTarget || TouchTarget) return;





        RaycastHit2D hit = Physics2D.Raycast(currentTouchPos, transform.forward, 0, TouchSoundLayerMask);
        if (hit) {
            if (hit.transform.name == "backgroundWave")
                AudioManager.instance?.PlayWave();
            else if (hit.transform.name == "backgroundSand")
                AudioManager.instance?.PlaySand();
        }
    }
}