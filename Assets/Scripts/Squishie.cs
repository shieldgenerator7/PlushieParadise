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
        List<float> aboveRightDirs = new List<float>();
        List<float> belowLeftDirs = new List<float>();
        float distanceToCheck = 1.0f;
        checkDirection(upDown, ref aboveRightDirs, ref belowLeftDirs, Vector2.up, distanceToCheck);
        checkDirection(upDown, ref aboveRightDirs, ref belowLeftDirs, -Vector2.up, distanceToCheck);
        //If only pushing in one direction
        if (aboveRightDirs.Count == 0 || belowLeftDirs.Count == 0)
        {
            //we'll be moved automatically, so do nothing
        }
        else
        {
            //We're squished and need to reduce size
            //Find min and max of the area we can be in
            float min = belowLeftDirs[0];
            float max = aboveRightDirs[0];
            foreach (float f in belowLeftDirs)
            {
                min = Mathf.Max(min, f);
            }
            foreach (float f in aboveRightDirs)
            {
                max = Mathf.Min(max, f);
            }
            //Find percent of current bounds that it is
            float percent = 1;
            if (upDown)
            {
                percent = Mathf.Abs(max - min) / coll2d.bounds.size.y;
            }
            else
            {
                percent = Mathf.Abs(max - min) / coll2d.bounds.size.x;
            }
            if (percent < 1)
            {
                //Squish
                Vector3 scale = transform.localScale;
                scale.y *= percent;
                transform.localScale = scale;
            }
        }
    }
    void checkDirection(bool upDown, ref List<float> aboveRightDirs, ref List<float> belowLeftDirs, Vector2 direction, float distance = 0.1f)
    {
        RaycastHit2D[] rch2ds = new RaycastHit2D[10];
        int count = coll2d.Cast(direction, rch2ds, distance, true);
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D rch2d = rch2ds[i];
            //Don't process our own colliders
            if (rch2d.collider.gameObject == gameObject)
            {
                continue;
            }
            //Add the object's collision point to the appropriate list
            Vector2 dir = rch2d.point;
            if (upDown)
            {
                if (rch2d.point.y < transform.position.y
                    && rch2d.collider.bounds.max.y < transform.position.y)
                {
                    belowLeftDirs.Add(dir.y);
                }
                else if (rch2d.collider.bounds.min.y > transform.position.y)
                {
                    aboveRightDirs.Add(dir.y);
                }
            }
            else
            {
                if (rch2d.point.x < transform.position.x
                    && rch2d.collider.bounds.max.x < transform.position.x)
                {
                    belowLeftDirs.Add(dir.x);
                }
                else if (rch2d.collider.bounds.min.y > transform.position.y)
                {
                    aboveRightDirs.Add(dir.x);
                }
            }
        }
    }
}
