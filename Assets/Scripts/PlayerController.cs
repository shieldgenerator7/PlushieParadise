using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 3;
    public float jumpForce = 2;
    public float jumpDuration = 1.1f;

    private float jumpStartTime = 0;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(horizontal * movementSpeed, rb2d.velocity.y);
        if (Input.GetButton("Jump"))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y + jumpForce);
        }
    }
}
