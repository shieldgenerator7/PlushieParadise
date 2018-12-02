using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to spawn the player at a certain location
public class BedChecker : MonoBehaviour {

    public GameObject playerSpawnPoint;

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("Player").transform.position = playerSpawnPoint.transform.position;
        Destroy(this);
	}
}
