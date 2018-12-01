using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Settings
    public float movementSpeed = 3;
    public float jumpHeight = 2.1f;//how high she can jump
    public float jumpDuration = 0.5f;//how long it takes her to get to max jump height
    public float jumpForceModifier = 0;//modifier to adjust the jump force curve

    //Static Variables
    private float jumpDecay = 0;
    private float baseJumpForce = 0;
    //Runtime vars
    private float jumpForce;
    private float jumpStartTime;

    //Components
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //Set base jump force
        baseJumpForce = (jumpHeight * jumpForceModifier) * (jumpHeight / jumpDuration);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(horizontal * movementSpeed, rb2d.velocity.y);
        if (Input.GetButton("Jump"))
        {
            if (jumpStartTime == 0)
            {
                jumpStartTime = Time.time;
                jumpForce = baseJumpForce;
            }
            if (Time.time <= jumpStartTime + jumpDuration)
            {
                jumpForce = (1 - ((Time.time - jumpStartTime) / jumpDuration)) * baseJumpForce;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpStartTime = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Min(0, rb2d.velocity.y));
        }
    }
}
