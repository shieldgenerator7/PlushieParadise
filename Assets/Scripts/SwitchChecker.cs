using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchChecker : MonoBehaviour
{

    public GameObject poweredObject;//the object to power

    public Sprite unpressedSprite;
    public Sprite pressedSprite;

    private bool pressed = false;
    public bool Pressed
    {
        get { return pressed; }
        set
        {
            pressed = value;
            Sprite spriteToUse = (pressed) ? pressedSprite : unpressedSprite;
            if (sr.sprite != spriteToUse)
            {
                sr.sprite = spriteToUse;
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
        Pressed = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Pressed = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Pressed = false;
    }
}
