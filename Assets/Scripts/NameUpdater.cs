using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUpdater : MonoBehaviour
{
    public Canvas plushieCanvas;
    public InputField inputField;
    public Text messageText;

    public GameObject plushieRevealStar;

    private string originalMessage;

    private GameObject plushie;

    private static NameUpdater instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        inputField.onEndEdit.AddListener(acceptName);
    }

    public static void getNameForNewPlushie(GameObject newPlushie)
    {
        //Canvas
        instance.plushieCanvas.gameObject.SetActive(true);
        instance.inputField.ActivateInputField();
        instance.plushie = newPlushie;
        //Reveal Star
        instance.plushieRevealStar.SetActive(true);
        instance.plushieRevealStar.transform.position = newPlushie.transform.position;
        //Update Message Text
        instance.originalMessage = instance.messageText.text;
        instance.messageText.text += newPlushie.name + "!";
        //Pause game
        Time.timeScale = 0;
    }

    void acceptName(string name)
    {
        plushie.GetComponent<Plushie>().givenName = name;
        //Canvas
        inputField.text = "";
        messageText.text = originalMessage;
        plushieCanvas.gameObject.SetActive(false);
        //Reveal Star
        instance.plushieRevealStar.SetActive(false);
        //Resume game
        Time.timeScale = 1;
    }

}
