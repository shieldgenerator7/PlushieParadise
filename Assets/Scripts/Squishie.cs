using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishie : MonoBehaviour
{
    public bool requiredToContinue = false;//true if its death would cause the game to end

    [Range(0, 1)]
    public float squishTolerancePercent = 0.7f;//what percent of its height it can get to before being squished
    [Range(0, 1)]
    public float squishResistance = 0;//what percent of the squish it can resist
    [Range(0, 1)]
    public float overlapAllowance = 0.1f;//used to keep side swipes from crushing

    //Runtime constants
    private Vector3 originalSize;
    private Vector2 originalBoundsSize;
    //Runtime vars
    private bool alive = true;
    public bool Alive
    {
        get { return alive; }
        private set { alive = value; }
    }

    private Collider2D coll2d;

    private void Start()
    {
        coll2d = GetComponent<Collider2D>();
        originalSize = transform.localScale;
        originalBoundsSize = coll2d.bounds.size;
    }

    public void checkSquish(Collider2D impendingColl, bool upDown)
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
                percent = Mathf.Abs(max - min) / originalBoundsSize.y;
            }
            else
            {
                percent = Mathf.Abs(max - min) / originalBoundsSize.x;
            }
            if (percent < 1)
            {
                //Factor in squish resistance
                float currentPercent = transform.localScale.y / originalSize.y;
                percent = Mathf.Lerp(percent, currentPercent, squishResistance);
                //Squish
                if (upDown) {
                    Vector3 scale = transform.localScale;
                    scale.y = Mathf.Min(scale.y, originalSize.y * percent);
                    transform.localScale = scale;
                    checkSquish(scale.y);
                }
                else
                {
                    Vector3 scale = transform.localScale;
                    scale.x = originalSize.x * percent;
                    transform.localScale = scale;
                    checkSquish(scale.x);
                }
            }
        }
    }
    void checkDirection(bool upDown, ref List<float> aboveRightDirs, ref List<float> belowLeftDirs, Vector2 direction, float distance = 0.1f)
    {
        RaycastHit2D[] rch2ds = new RaycastHit2D[10];
        int count = coll2d.Cast(direction, rch2ds, distance, true);
        Debug.Log("count: " + count + ", dir: " + direction * distance);
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
                    && rch2d.collider.bounds.max.y <= coll2d.bounds.min.y + overlapAllowance)
                {
                    dir.y = Mathf.Max(dir.y, rch2d.collider.bounds.max.y);
                    belowLeftDirs.Add(dir.y);
                }
                else if (rch2d.collider.bounds.min.y >= coll2d.bounds.max.y - overlapAllowance)
                {
                    dir.y = Mathf.Min(dir.y, rch2d.collider.bounds.min.y);
                    aboveRightDirs.Add(dir.y);
                }
            }
            else
            {
                if (rch2d.point.x < transform.position.x
                    && rch2d.collider.bounds.max.x < coll2d.bounds.min.x + overlapAllowance)
                {
                    dir.x = Mathf.Max(dir.x, rch2d.collider.bounds.max.x);
                    belowLeftDirs.Add(dir.x);
                }
                else if (rch2d.collider.bounds.min.x > coll2d.bounds.max.x - overlapAllowance)
                {
                    dir.x = Mathf.Min(dir.x, rch2d.collider.bounds.min.x);
                    aboveRightDirs.Add(dir.x);
                }
            }
        }
    }

    private void checkSquish(float sizeY)
    {
        if (sizeY < originalSize.y * squishTolerancePercent)
        {
            coll2d.enabled = false;
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            rb2d.gravityScale = 0;
            alive = false;
            if (requiredToContinue)
            {
                Time.timeScale = 0;
            }
        }
    }
    public void resetAlive()
    {
        coll2d.enabled = true;
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = false;
        rb2d.gravityScale = 1;
        alive = true;
        transform.localScale = originalSize;
        if (requiredToContinue)
        {
            Time.timeScale = 1;
        }
    }
}
