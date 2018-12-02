using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {

    public float rotateSpeed = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            Vector3 angles = transform.eulerAngles;
            angles.z += rotateSpeed * Time.deltaTime;
            transform.eulerAngles = angles;
        }
	}
}
