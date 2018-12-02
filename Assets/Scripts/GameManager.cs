using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public string firstLevelName = "Level1";

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(firstLevelName, LoadSceneMode.Additive);
	}
}
