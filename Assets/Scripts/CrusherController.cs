using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherController : PoweredObject
{

    public float moveSpeed = 1;

    public Collider2D triggerColl;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //}

    protected override void updateActiveState()
    {
        rb2d.velocity = Vector2.up * moveSpeed * ((Active) ? 1 : -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkSquishie(collision.gameObject);
        rb2d.velocity = Vector2.up * moveSpeed * ((Active) ? 1 : -1);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        checkSquishie(collision.gameObject);
        rb2d.velocity = Vector2.up * moveSpeed * ((Active) ? 1 : -1);
    }
    void checkSquishie(GameObject go)
    {
        Squishie squishie = go.GetComponent<Squishie>();
        if (squishie)
        {
            squishie.checkSquish(triggerColl, true);
        }
    }
}
