using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite door1;
    [SerializeField] private Sprite door2;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private bool isFinalDoor = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFinalDoor && !ClearFragManager.IsAllClear()) return;
        
        if (collision.tag == "Player")
        {
            sprite.sprite = door1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sprite.sprite = door2;
        }
    }
}
