using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherController : PoweredObject
{

    public float moveSpeed = 1;
    public float powerDownDelay = 0;

    private float powerDownTime = 0;

    public Collider2D triggerColl;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (powerDownTime > 0)
        {
            if (Time.time > powerDownTime + powerDownDelay)
            {
                powerDownTime = 0;
                rb2d.velocity = Vector2.up * moveSpeed * ((Active) ? 1 : -1);
            }
        }
    }

    protected override void updateActiveState()
    {
        if (!Active && powerDownDelay > 0)
        {
            powerDownTime = Time.time;
        }
        else
        {
            rb2d.velocity = Vector2.up * moveSpeed * ((Active) ? 1 : -1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collidesWithCrushTrigger(collision))
        {
            checkSquishie(collision.gameObject);
        }
        updateActiveState();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(">>>collision object:lo: " + collision.gameObject);
        if (collidesWithCrushTrigger(collision))
        {
            checkSquishie(collision.gameObject);
        }
        updateActiveState();
    }
    bool collidesWithCrushTrigger(Collider2D coll)
    {
        RaycastHit2D[] rch2ds = new RaycastHit2D[10];
        int count = triggerColl.Cast(Vector2.zero, rch2ds, 0, true);
        for (int i = 0; i < count; i++)
        {
            if (rch2ds[i].collider == coll)
            {
                return true;
            }
        }
        return false;
    }
    void checkSquishie(GameObject go)
    {
        Squishie squishie = go.GetComponent<Squishie>();
        if (squishie)
        {
            //If the squishie is in direction of travel
            if (Mathf.Sign((squishie.transform.position - transform.position).y) == ((Active) ? 1 : -1))
            {
                //check to see if it gets squished
                squishie.checkSquish(triggerColl, true);
            }
        }
    }
}
