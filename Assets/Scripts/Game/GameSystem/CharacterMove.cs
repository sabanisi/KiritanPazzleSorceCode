﻿using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private int identifyNum;
    void Start()
    {
        Vector3 goalPos = new Vector3(0, 0, 0);
        switch (identifyNum)
        {
            case 0:
                goalPos = new Vector3(-1.8f, 1.1f, 10);
                break;
            case 1:
                goalPos = new Vector3(-0.9f, 0.9f, 10);
                break;
            case 2:
                goalPos = new Vector3(0, 1.1f, 10);
                break;
            case 3:
                goalPos = new Vector3(0.9f, 0.9f, 10);
                break;
            case 4:
                goalPos = new Vector3(1.8f, 1.1f, 10);
                break;
        }
        Hashtable moveHash = new Hashtable();
        moveHash.Add("position", goalPos);
        moveHash.Add("time", 0.5);
        moveHash.Add("delay", 0f);
        moveHash.Add("islocal", true);
        moveHash.Add("easeType", "easeOutQuart");
        iTween.MoveTo(gameObject, moveHash);
    }
}
