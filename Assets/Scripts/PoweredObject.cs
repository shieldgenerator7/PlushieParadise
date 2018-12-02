using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoweredObject : MonoBehaviour
{

    private bool active = false;
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            updateActiveState();
        }
    }
    protected abstract void updateActiveState();
}
