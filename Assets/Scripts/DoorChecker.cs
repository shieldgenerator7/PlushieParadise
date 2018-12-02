using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour {

    [Tooltip("The plushie that opens this door.")]
    public GameObject plushieKey;
    public GameObject door;//the part of the door that swings

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
        if (collision.gameObject == plushieKey)
        {
            activate(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == plushieKey)
        {
            activate(true);
        }
    }

    void activate(bool active)
    {
        Open = active;
    }
}
