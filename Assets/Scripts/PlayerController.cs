﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Settings
    public float movementSpeed = 3;
    public float jumpHeight = 2.1f;//how high she can jump
    public float jumpDuration = 0.5f;//how long it takes her to get to max jump height
    public float jumpForceModifier = 0;//modifier to adjust the jump force curve
    public Vector2 throwDirection = Vector2.one;//the direction that you throw the plushie
    public float throwForce = 3;//how fast to throw the plushie

    public GameObject plushieSpawnPoint;//point the plushie jumps to right before being thrown
    public GameObject plushieContainer;//the object that will store all the plushies

    //Constant Variables
    private float baseJumpForce = 0;
    //Runtime vars
    private float jumpForce;
    private float jumpStartTime;

    [SerializeField]
    private List<GameObject> plushies = new List<GameObject>();
    private int lastUsedPlushieIndex = -1;//the index of the last plushie used

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
        if (Input.GetButton("Fire1"))
        {
            if (plushies.Count > 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    lastUsedPlushieIndex = (lastUsedPlushieIndex + 1) % plushies.Count;
                    plushies[lastUsedPlushieIndex].GetComponent<BoxCollider2D>().enabled = false;
                    Rigidbody2D plushieRB2D = plushies[lastUsedPlushieIndex].GetComponent<Rigidbody2D>();
                    plushieRB2D.velocity = Vector2.zero;
                    plushieRB2D.angularVelocity = 0;
                }
                plushies[lastUsedPlushieIndex].transform.position = plushieSpawnPoint.transform.position;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (plushies.Count > 0)
            {
                plushies[lastUsedPlushieIndex].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (plushies.Count > 0)
            {
                lastUsedPlushieIndex = (lastUsedPlushieIndex + 1) % plushies.Count;
                GameObject plushie = plushies[lastUsedPlushieIndex];
                plushie.transform.position = plushieSpawnPoint.transform.position;
                plushie.GetComponent<Rigidbody2D>().velocity = throwDirection * throwForce;
            }
        }
    }

    public void addPlushie(GameObject plushie)
    {
        plushies.Add(plushie);
        plushie.transform.parent = plushieContainer.transform;
    }
}
