using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public string firstLevelName = "Level1";

    private string currentLevelName;

    private static GameManager instance;

	// Use this for initialization
	void Start () {
        currentLevelName = firstLevelName;
        SceneManager.LoadScene(firstLevelName, LoadSceneMode.Additive);
        if (instance == null)
        {
            instance = this;
        }
	}

    public static void loadNextLevel(string levelName)
    {
        //Unload current level first
        string current = instance.currentLevelName;
        if (SceneManager.GetSceneByName(current).isLoaded)
        {
            SceneManager.UnloadSceneAsync(current);
        }
        //Update current
        instance.currentLevelName = current = levelName;
        //Load current level
        SceneManager.LoadScene(current, LoadSceneMode.Additive);
    }
}
