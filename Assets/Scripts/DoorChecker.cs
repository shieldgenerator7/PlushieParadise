using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour
{

    [Tooltip("The plushie that opens this door.")]
    public GameObject plushieKey;
    public GameObject door;//the part of the door that swings
    public string nextLevelName;

    bool open = false;
    bool Open
    {
        get { return open; }
        set
        {
            open = value;
            //Open or close the door sprite
            Vector3 scale = door.transform.localScale;
            scale.x = (open) ? -1 : 1;
            door.transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkForPlushie(collision.gameObject);
        checkForPlayer(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkForPlushie(collision.gameObject);
        checkForPlayer(collision.gameObject);
    }

    void checkForPlushie(GameObject go)
    {
        if (go == plushieKey
            || (plushieKey == null && go.CompareTag("Plushie")))
        {
            activate(true);
        }
    }

    void activate(bool active)
    {
        Open = active;
    }

    void checkForPlayer(GameObject go)
    {
        if (Open)
        {
            if (go.CompareTag("Player"))
            {
                if (nextLevelName != null && nextLevelName != "")
                {
                    GameManager.loadNextLevel(nextLevelName);
                }
            }
        }
    }
}
