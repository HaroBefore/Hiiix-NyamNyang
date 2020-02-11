using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {

    public ScenarioType type;

    public string scenarioName;

    public int sceneCount;

    public GameObject[] sceneImage;
    public int[] nameIndexArray;
    public string[] sceneScripts_name;
    public int[] scriptIndexArray;
    [TextArea]
    public string[] sceneScripts;

    public Color[] sceneTextBoardColors_1;
    public Color[] sceneTextBoardColors_2;

    public virtual void OnCloseScenario() { }



}