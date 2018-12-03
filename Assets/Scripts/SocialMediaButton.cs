﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SocialMediaButton : MonoBehaviour
{

    public Sprite normalSprite;
    public Sprite mouseOverSprite;
    public Sprite clickSprite;

    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log(getDescription());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (sr.bounds.Contains(mousePos))
        {
            postToSocialMedia();
        }
    }

    public abstract void postToSocialMedia();

    public Plushie[] getPlushieList()
    {
        return GameObject.FindObjectsOfType<Plushie>();
    }

    public string getDescription()
    {
        Plushie[] plushieList = getPlushieList();
        string nounAndVerb = (plushieList.Length > 1) ? "plushies are" : "plushie is";
        string desc = "my favorite " + nounAndVerb + " ";
        bool onlyTwo = plushieList.Length == 2;
        for (int i = 0; i < plushieList.Length; i++)
        {
            Plushie plushie = plushieList[i];
            bool firstOne = i == 0;
            bool lastOne = i == plushieList.Length - 1;
            desc += (lastOne && !firstOne) ? "and " : "";
            desc += plushie.givenName + " the " + plushie.typeName;
            desc += (lastOne) ? " :3" :
                (onlyTwo) ? " " : ", ";
        }
        return desc;
    }
}