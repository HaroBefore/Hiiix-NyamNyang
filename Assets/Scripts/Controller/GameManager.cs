using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }
    
    private TimeManager _timeManager;
    
    // 튜토리얼중인지 여부.
    private bool _isTutorial;
    public bool IsTutorial {
        get { return _isTutorial; }
        set {
            _isTutorial = value;
            if (value) {
                _timeManager.Pause();
            }
            else {
                _timeManager.Resume();
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _timeManager = TimeManager.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
