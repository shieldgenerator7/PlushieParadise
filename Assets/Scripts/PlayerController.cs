using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Settings
    public bool debugMode = false;
    public float movementSpeed = 3;
    public float jumpHeight = 2.1f;//how high she can jump
    public float jumpDuration = 0.5f;//how long it takes her to get to max jump height
    public float jumpForceModifier = 0;//modifier to adjust the jump force curve
    public Vector2 throwDirection = Vector2.one;//the direction that you throw the plushie
    public float throwForce = 3;//how fast to throw the plushie
    public string ability1 = "Call Plushie";
    public string ability2 = "Throw Plushie";

    public GameObject plushieSpawnPoint;//point the plushie jumps to right before being thrown
    public GameObject plushieContainer;//the object that will store all the plushies

    //Constant Variables
    private float baseJumpForce = 0;
    //Runtime vars
    private float jumpForce;
    private float jumpStartTime;
    private bool grounded = true;
    private RaycastHit2D[] horizontalCastHits = new RaycastHit2D[10];

    [SerializeField]
    private List<GameObject> plushies = new List<GameObject>();
    private int lastUsedPlushieIndex = -1;//the index of the last plushie used

    //Components
    private Rigidbody2D rb2d;
    private Collider2D coll2d;
    private Squishie squishie;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();
        squishie = GetComponent<Squishie>();
        //Set base jump force
        baseJumpForce = (jumpHeight * jumpForceModifier) * (jumpHeight / jumpDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            baseJumpForce = (jumpHeight * jumpForceModifier) * (jumpHeight / jumpDuration);
        }
        //Admin controls
        if (Input.GetButtonDown("Reset") && !NameUpdater.updatingName())
        {
            GameManager.resetLevel();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (!squishie.Alive || NameUpdater.updatingName())
        {
            //don't process player controls if not alive
            //or if you are currently updating a name
            return;
        }
        //Player controls
        float horizontal = Input.GetAxis("Horizontal");
        //Check to make sure you can move
        if (canMove(Vector2.right * horizontal, 0.01f))
        {
            rb2d.velocity = new Vector2(horizontal * movementSpeed, rb2d.velocity.y);
        }
        //Flip sprite
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = (horizontal > 0) ? 1 : -1;
            transform.localScale = scale;
        }
        //Jump
        if (Input.GetButton("Jump"))
        {
            if (grounded)
            {
                if (jumpStartTime == 0)
                {
                    jumpStartTime = Time.time;
                    jumpForce = baseJumpForce;
                    grounded = false;
                }
                if (Time.time <= jumpStartTime + jumpDuration)
                {
                    jumpForce = (1 - ((Time.time - jumpStartTime) / jumpDuration)) * baseJumpForce;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                }
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpStartTime = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Min(0, rb2d.velocity.y));
        }
        if (Input.GetButton(ability1))
        {
            if (hasPlushies())
            {
                if (Input.GetButtonDown(ability1))
                {
                    lastUsedPlushieIndex = getNextPlushieIndex();
                    plushies[lastUsedPlushieIndex].GetComponent<BoxCollider2D>().enabled = false;
                    Rigidbody2D plushieRB2D = plushies[lastUsedPlushieIndex].GetComponent<Rigidbody2D>();
                    plushieRB2D.velocity = Vector2.zero;
                    plushieRB2D.angularVelocity = 0;
                    plushies[lastUsedPlushieIndex].GetComponent<Squishie>().onSpikes = false;
                }
                plushies[lastUsedPlushieIndex].transform.position = plushieSpawnPoint.transform.position;
            }
        }
        else if (Input.GetButtonUp(ability1))
        {
            if (hasPlushies())
            {
                plushies[lastUsedPlushieIndex].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else if (Input.GetButtonDown(ability2))
        {
            if (hasPlushies())
            {
                lastUsedPlushieIndex = getNextPlushieIndex();
                GameObject plushie = plushies[lastUsedPlushieIndex];
                plushie.transform.position = plushieSpawnPoint.transform.position;
                Vector2 throwVector = throwDirection * throwForce;
                throwVector.x *= transform.localScale.x;
                plushie.GetComponent<Rigidbody2D>().velocity = throwVector;
                plushies[lastUsedPlushieIndex].GetComponent<Squishie>().onSpikes = false;
            }
        }
        checkPlushieCalling();
    }

    public bool hasPlushies()
    {
        if (plushies.Count == 0)
        {
            return false;
        }
        else
        {
            foreach (GameObject plushie in plushies)
            {
                if (plushie.GetComponent<Squishie>().Alive)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int getNextPlushieIndex()
    {
        for (int i = 0; i < plushies.Count; i++)
        {
            //here, i is just used to keep the loop from being infinite
            lastUsedPlushieIndex = (lastUsedPlushieIndex + 1) % plushies.Count;
            if (plushies[lastUsedPlushieIndex].GetComponent<Squishie>().Alive)
            {
                break;
            }
        }
        return lastUsedPlushieIndex;
    }

    public void addPlushie(GameObject plushie)
    {
        plushies.Add(plushie);
        plushie.transform.parent = plushieContainer.transform;
    }

    public void removePlushie(Plushie plushie)
    {
        int indexOf = plushies.IndexOf(plushie.gameObject);
        plushies.Remove(plushie.gameObject);
        if (indexOf < lastUsedPlushieIndex)
        {
            lastUsedPlushieIndex--;
        }
    }
    public void resetPlayer()
    {
        grounded = true;
        squishie.resetAlive();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D coll2d = GetComponent<Collider2D>();
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D cp2d = collision.GetContact(i);
            bool contactBelow = cp2d.point.y <= transform.position.y;
            bool contactBetweenX = cp2d.point.x > coll2d.bounds.min.x && cp2d.point.x < coll2d.bounds.max.x;
            if (contactBelow && contactBetweenX)
            {
                grounded = true;
                break;
            }
        }
    }

    bool canMove(Vector2 direction, float distance)
    {
        int count = coll2d.Cast(direction, horizontalCastHits, distance, true);
        if (count == 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D rch2d = horizontalCastHits[i];
                //Don't process trigger colliders
                if (rch2d.collider.isTrigger)
                {
                    continue;
                }
                //Process rigidbodies
                Rigidbody2D rchRb2d = rch2d.collider.gameObject.GetComponent<Rigidbody2D>();
                //If the object ahead is static,
                if (!rchRb2d)
                {
                    //then you can't move
                    return false;
                }
            }
        }
        return true;
    }

    void checkPlushieCalling()
    {
        for (int i = 0; i < 4 && i < plushies.Count; i++)
        {
            if (Input.GetButtonDown("Call Plushie " + (i + 1)))
            {
                lastUsedPlushieIndex = i;
                Rigidbody2D plushieRB2D = plushies[lastUsedPlushieIndex].GetComponent<Rigidbody2D>();
                plushieRB2D.velocity = Vector2.zero;
                plushieRB2D.angularVelocity = 0;
                plushies[lastUsedPlushieIndex].transform.position = plushieSpawnPoint.transform.position;
                plushies[lastUsedPlushieIndex].GetComponent<Squishie>().onSpikes = false;
                //Queue this plushie up to be the next one called or thrown
                lastUsedPlushieIndex = (i - 1 + plushies.Count) % plushies.Count;
            }
        }
    }
}
