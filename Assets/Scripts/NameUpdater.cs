using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUpdater : MonoBehaviour {

    public InputField inputField;

    private GameObject plushie;

    private static NameUpdater instance;
    
	void Awake () {
		if (instance == null)
        {
            instance = this;
        }
        inputField.onEndEdit.AddListener(acceptName);
	}

    public static void getNameForNewPlushie(GameObject newPlushie)
    {
        instance.inputField.gameObject.SetActive(true);
        instance.inputField.ActivateInputField();
        instance.plushie = newPlushie;
        //Pause game
        Time.timeScale = 0;
    }

    void acceptName(string name)
    {
        plushie.name = name;
        inputField.text = "";
        inputField.gameObject.SetActive(false);
        //Resume game
        Time.timeScale = 1;
    }

}
