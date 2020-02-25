using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffTimeText : MonoBehaviour
{
    private Text textTime;

    private void Start()
    {
        textTime = GetComponent<Text>();
    }

    private void Update()
    {
        textTime.text = $"{(int) TimeManager.Instance.LeftBuffCollTime} Sec";
    }
}
