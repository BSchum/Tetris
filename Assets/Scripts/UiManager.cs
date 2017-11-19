using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
    GameScript gameScript;
    public Text Pause;
	// Use this for initialization
	void Start () {
        gameScript = GameObject.Find("Spawner").GetComponent<GameScript>();
    }

    // Update is called once per frame
    void Update () {
        showPauseUI();
	}

    void showPauseUI()
    {
        if (gameScript.isPaused)
        {
            Pause.enabled = true;
        }
        else if (!gameScript.isPaused)
        {
            Pause.enabled = false;
        }
    }
}
