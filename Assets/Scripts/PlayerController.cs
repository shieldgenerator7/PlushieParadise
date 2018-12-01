using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 3;
    public float jumpForce = 2;
    public float jumpDuration = 1.1f;

    private float jumpStartTime = 0;

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb2d.AddForce(new Vector2(horizontal, 0) * movementSpeed);
        rb2d.AddForce(Vector2.up * jumpForce);
    }
}
