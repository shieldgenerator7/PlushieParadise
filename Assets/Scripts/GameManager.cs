using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string firstLevelName = "Level1";
    public GameObject plushiePen;

    private string currentLevelName;

    private static GameManager instance;

    // Use this for initialization
    void Start()
    {
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
        //Put plushies "away"
        instance.putAwayPlushies();
        //Update current
        instance.currentLevelName = current = levelName;
        //Load current level
        SceneManager.LoadScene(current, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Puts plushies in a safe place where they won't interfere with the level
    /// </summary>
    void putAwayPlushies()
    {
        foreach (GameObject plushie in GameObject.FindGameObjectsWithTag("Plushie"))
        {
            plushie.transform.position = plushiePen.transform.position;
        }
    }
}
