﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {

    public float rotateSpeed = 3;
    public bool spinWhilePaused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            Vector3 angles = transform.eulerAngles;
            float deltaTime = (spinWhilePaused) ? Time.unscaledDeltaTime : Time.deltaTime;
            angles.z += rotateSpeed * deltaTime;
            transform.eulerAngles = angles;
        }
	}
}
