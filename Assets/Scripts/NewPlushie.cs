using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlushie : MonoBehaviour {
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NameUpdater.getNameForNewPlushie(gameObject);
            Destroy(this);
        }
    }
}
