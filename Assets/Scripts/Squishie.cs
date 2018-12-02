using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishie : MonoBehaviour
{

    [Range(0, 1)]
    public float squishTolerancePercent = 0.7f;//what percent of its height it can get to before being squished

    private Vector2 originalSize;

    private Collider2D coll2d;

    private void Start()
    {
        coll2d = GetComponent<Collider2D>();
        originalSize = transform.localScale;
    }

    public void squish(Collider2D impendingColl, bool upDown)
    {
        List<Vector2> pushDirs = new List<Vector2>();
        RaycastHit2D[] rch2ds = new RaycastHit2D[10];
        int count = coll2d.Cast(Vector2.zero, rch2ds, 0, true);
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D rch2d = rch2ds[i];
            if (true || coll2d.OverlapPoint(rch2d.point))
            {
                Vector2 pushDir = (Vector2)transform.position - rch2d.point;
                if (upDown)
                {
                    pushDir.x = 0;
                }
                else
                {
                    pushDir.y = 0;
                }
                if (pushDir == Vector2.zero)
                {
                    Rigidbody2D rchRbd2d = rch2d.collider.gameObject.GetComponent<Rigidbody2D>();
                    if (rchRbd2d)
                    {
                        pushDir = rchRbd2d.velocity;
                    }
                }
                pushDirs.Add(pushDir);
            }
        }
        float pushDirTotalPos = 0;
        float pushDirTotalNeg = 0;
        foreach (Vector2 pushDir in pushDirs)
        {
            float pushValue = 0;
            if (upDown)
            {
                pushValue = pushDir.y;
            }
            else
            {
                pushValue = pushDir.x;
            }
            if (pushValue > 0)
            {
                pushDirTotalPos += Mathf.Abs(pushValue);
            }
            if (pushValue < 0)
            {
                pushDirTotalNeg += Mathf.Abs(pushValue);
            }
        }
        if (pushDirTotalPos == 0 || pushDirTotalNeg == 0)
        {
            //we're all good, it's only pushing in one direction
            //Vector2 pos = transform.position;
            //float moveAmount = pushDirTotalPos - pushDirTotalNeg;
            //if (upDown)
            //{
            //    //transform.position += Vector3.up * moveAmount;
            //}
            //else
            //{
            //    transform.position += Vector3.right * moveAmount;
            //}
        }
        else
        {
            //we're squished and need to reduce size
            float squishAmount = 0.1f;
            Vector3 scale = transform.localScale;
            scale.y -= squishAmount;
            transform.localScale = scale;
        }
    }
}
