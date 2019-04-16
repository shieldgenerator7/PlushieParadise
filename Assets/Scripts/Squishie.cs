using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishie : MonoBehaviour
{
    public bool requiredToContinue = false;//true if its death would cause the game to end

    [Header("Crusher")]
    [Range(0, 1)]
    public float squishTolerancePercent = 0.7f;//what percent of its height it can get to before being squished
    [Range(0, 1)]
    public float squishResistance = 0;//what percent of the squish it can resist
    [Range(0, 1)]
    public float overlapAllowance = 0.1f;//used to keep side swipes from crushing

    [Header("Spikes")]
    public bool canStandOnSpikes = false;

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
    public bool onSpikes = false;
    private Vector2 spikePosition;

    private Collider2D coll2d;

    private void Start()
    {
        coll2d = GetComponent<Collider2D>();
        originalSize = transform.localScale;
        originalBoundsSize = coll2d.bounds.size;
    }

    private void Update()
    {
        if (onSpikes && !Alive)
        {
            if ((Vector2)transform.position != spikePosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, spikePosition, Time.deltaTime);
            }
        }
    }

    public void checkSquish(Collider2D impendingColl, bool upDown)
    {
        List<float> aboveRightDirs = new List<float>();
        List<float> belowLeftDirs = new List<float>();
        float distanceToCheck = 1.0f;
        checkDirection(upDown, ref aboveRightDirs, ref belowLeftDirs, Vector2.up, distanceToCheck);
        checkDirection(upDown, ref aboveRightDirs, ref belowLeftDirs, -Vector2.up, distanceToCheck);
        Debug.Log("aboveRight: " + aboveRightDirs.Count + ", belowLeft: " + belowLeftDirs.Count);
        //If only pushing in one direction
        if (aboveRightDirs.Count == 0 || belowLeftDirs.Count == 0)
        {
            //we'll be moved automatically, so do nothing
        }
        else
        {
            //We're squished and need to reduce size
            Debug.Log("We're squished! squisher: " + impendingColl.name);
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
            Debug.Log("Pre-Resist Percent: " + percent + ", min: " + min + ", max: " + max
                + ", diff: " + Mathf.Abs(max - min) + ", size: " + originalBoundsSize);

            if (percent < 1)
            {
                //Factor in squish resistance
                float currentPercent = transform.localScale.y / originalSize.y;
                percent = Mathf.Lerp(percent, currentPercent, squishResistance);
                Debug.Log("Percent: " + percent + ", min: " + min + ", max: " + max);
                //Squish
                if (upDown)
                {
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
                Debug.Log("object: " + rch2d.collider.name
                    + ": rch2d.y: " + rch2d.point.y
                    + ", my y: " + transform.position.y
                    + ", dir: " + direction * distance);
                Debug.Log(">>>"
                    + " rch coll: (" + rch2d.collider.bounds.min.y + ", " + rch2d.collider.bounds.max.y + ")"
                    + ", coll2d: (" + coll2d.bounds.min.y + ", " + coll2d.bounds.max.y + ")");
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
            kill();
        }
    }

    public void kill()
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
        onSpikes = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            GameObject spikes = collision.gameObject;
            Debug.Log("Colliding with spikes");
            if (collision.GetContact(0).point.y <= coll2d.bounds.min.y + overlapAllowance)
            {
                onSpikes = true;
                //Set spikePosition so the sprite will move there over time
                spikePosition = spikes.transform.position +
                    (Vector3.up * (2 * spikes.GetComponent<Collider2D>().bounds.size.y / 6));
                spikePosition.x = collision.GetContact(0).point.x;
                //Check to see if they can stand on them
                if (!canStandOnSpikes)
                {
                    spikeKill();
                }
            }
        }
        else if (onSpikes)
        {
            if (collision.GetContact(0).point.y >= coll2d.bounds.max.y - overlapAllowance)
            {
                spikeKill();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            onSpikes = false;
        }
    }

    void spikeKill()
    {
        kill();
        coll2d.enabled = true;
    }
}
