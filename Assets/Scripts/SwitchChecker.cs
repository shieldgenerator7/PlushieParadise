using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchChecker : MonoBehaviour
{

    public List<PoweredObject> poweredObjects;//the object to power

    public Sprite unpressedSprite;
    public Sprite pressedSprite;

    private bool pressed = false;
    public bool Pressed
    {
        get { return pressed; }
        set
        {
            bool changed = pressed != value;
            pressed = value;
            if (changed)
            {
                sr.sprite = (pressed) ? pressedSprite : unpressedSprite;
                foreach (PoweredObject po in poweredObjects)
                {
                    po.Active = pressed;
                }
            }
        }
    }

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (unpressedSprite == null)
        {
            unpressedSprite = sr.sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>()
            && !collision.gameObject.GetComponent<CrusherController>())
        {
            Pressed = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>()
               && !collision.gameObject.GetComponent<CrusherController>())
        {
            Pressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>()
               && !collision.gameObject.GetComponent<CrusherController>())
        {
            Pressed = false;
        }
    }
}
