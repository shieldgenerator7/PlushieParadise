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
	
    void acceptName(string name)
    {
        plushie.name = name;
        inputField.gameObject.SetActive(false);
    }

	public static void getNameForNewPlushie(GameObject newPlushie)
    {
        instance.inputField.gameObject.SetActive(true);
        instance.plushie = newPlushie;
    }
}
