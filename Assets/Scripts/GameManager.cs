﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Range(-1, 10)]
    public int firstLevelIndex = 1;
    public string firstLevelName = "Level1";
    public GameObject plushiePen;

    private string currentLevelName;

    private static GameManager instance;

    // Use this for initialization
    void Start()
    {
        if (firstLevelIndex > -1)
        {
            firstLevelName = "Level" + firstLevelIndex;
        }
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
        cleanPlushies();
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
    /// <summary>
    /// Remove any dead plushies
    /// </summary>
    static void cleanPlushies()
    {
        PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
        foreach (Plushie plushie in GameObject.FindObjectsOfType<Plushie>())
        {
            Squishie squishie = plushie.GetComponent<Squishie>();
            if (!squishie.Alive)
            {
                pc.removePlushie(plushie);
                Destroy(plushie.gameObject);
            }
        }
    }

    public static void resetLevel()
    {
        //Reset Squishies
        foreach (Squishie squishie in FindObjectsOfType<Squishie>())
        {
            squishie.resetAlive();
        }
        //Unload and reload the current level
        loadNextLevel(instance.currentLevelName);
        //Add delegate for things that need the scene to be already loaded
        SceneManager.sceneLoaded += instance.continueResetLevel;
        //Reset player settings if need be
        PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
        pc.resetPlayer();
    }

    void continueResetLevel(Scene scene, LoadSceneMode loadSceneMode)
    {
        PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
        //Keep player from stock piling duplicate plushies
        NewPlushie newPlushie = GameObject.FindObjectOfType<NewPlushie>();
        if (newPlushie)
        {
            Debug.Log("newPlushie: " + newPlushie.name);
            foreach (Plushie plushie in GameObject.FindObjectsOfType<Plushie>())
            {
                Debug.Log("plushie: " + plushie.name);
                if (plushie.gameObject != newPlushie.gameObject)
                {
                    if (plushie.Equals(newPlushie.GetComponent<Plushie>()))
                    {
                        pc.removePlushie(plushie);
                        Destroy(plushie.gameObject);
                    }
                }
            }
        }
        //Remove delegate
        SceneManager.sceneLoaded -= continueResetLevel;
    }
}
