using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonEffect : MonoBehaviour {

    public bool debugMode = false;

    public float endScale = 0.7f;
    public float timePerCycle = 0.5f;

    //Runtime constants
    private Vector3 origScale;
    private float startScale = 1;
    //Runtime vars
    private float currentScale;
    private float nextScale;
    private float speed;

	// Use this for initialization
	void Start () {
        origScale = transform.localScale;
        currentScale = startScale;
        nextScale = endScale;
        speed = Mathf.Abs(startScale - endScale) / timePerCycle;
    }
	
	// Update is called once per frame
	void Update () {
        if (debugMode)
        {
            speed = Mathf.Abs(startScale - endScale) / timePerCycle;
            nextScale = endScale;
            currentScale = startScale;
        }
        Vector3 targetScale = origScale * nextScale;
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            targetScale,
            speed * Time.deltaTime
            );		
        if (transform.localScale == targetScale)
        {
            float tempScale = nextScale;
            nextScale = currentScale;
            currentScale = tempScale;
        }
	}
}
