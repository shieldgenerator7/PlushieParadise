using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherColliderFixer : MonoBehaviour {

	public void fixAllCrusherColliders()
    {
        foreach (CrusherController cc in FindObjectsOfType<CrusherController>())
        {
            SpriteRenderer sr = cc.GetComponent<SpriteRenderer>();
            Bounds spriteBounds = sr.bounds;
            foreach(BoxCollider2D bc2d in cc.GetComponents<BoxCollider2D>())
            {
                bc2d.size = spriteBounds.size;
                bc2d.offset = Vector2.zero;
                Vector2 offset = bc2d.offset;
                offset.y = spriteBounds.size.y / 4;
            }
        }
    }
}
