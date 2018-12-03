using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plushie : MonoBehaviour {

    public string typeName;
    public string givenName;

    public override bool Equals(object other)
    {
        if (other is GameObject)
        {
            return equals((GameObject)other);
        }
        else if (other is Plushie)
        {
            return equals((Plushie)other);
        }
        else
        {
            throw new System.InvalidOperationException("Cannot compare Plushie " + this.name + " with object " + other);
        }
    }
    public bool equals(GameObject otherPlushie)
    {
        return equals(otherPlushie.GetComponent<Plushie>());
    }
    public bool equals(Plushie otherPlushie)
    {
        return otherPlushie.typeName == this.typeName;
    }

    public override int GetHashCode()
    {
        return typeName.GetHashCode();
    }
}
