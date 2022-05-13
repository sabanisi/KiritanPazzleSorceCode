﻿using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    [SerializeField] Locked locked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked == null) return;
        if (collision.gameObject.tag == "Shot")
        {
            locked.Open();
        }
    }
}
