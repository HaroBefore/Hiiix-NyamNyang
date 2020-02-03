using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border {
    public Vector2[] Corner;    // topLeft, topRight, bottomRight, bottomLeft.

    public Border(BoxCollider2D col) {
        Bounds b = col.bounds;
        Vector2 tL = new Vector2(b.min.x, b.max.y);
        Vector2 tR = new Vector2(b.max.x, b.max.y);
        Vector2 bR = new Vector2(b.max.x, b.min.y);
        Vector2 bL = new Vector2(b.min.x, b.min.y);

        Corner = new Vector2[4] { tL, tR, bR, bL };
    }
}

public class BorderManager : MonoBehaviour {

    public static BorderManager instance;

    [Header("Border")]
    public BoxCollider2D customerSeat;
    public Border customerSeatBorder { get; protected set; }

    [Header("Position")]
    public Vector2 nyangPositionA;
    public Vector2 nyangPositionB;
    public Vector2 nyangPositionC;
    public Vector2 nyangPositionD;
    public Vector2 nyangPositionE;
    public Vector2 nyangPositionF;
    public Vector2 nyangPositionBoss;
    public Vector2 customerSeatPosition;
    public Vector2 stovePosition;

    void Awake() {
        if (!instance) instance = this;

        customerSeatBorder = new Border(customerSeat);
    }



    public bool IsInBorder(Border b) {
        Vector2 currentPosition = InputManager.instance.currentTouchPos;
        return (b.Corner[0].x <= currentPosition.x) && (currentPosition.y <= b.Corner[1].x) && (b.Corner[3].y <= currentPosition.y) && (currentPosition.y <= b.Corner[0].y);
    }
}