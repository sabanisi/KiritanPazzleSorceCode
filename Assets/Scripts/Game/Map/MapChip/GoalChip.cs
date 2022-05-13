using UnityEngine;
using System.Collections;

public class GoalChip : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.IsPause()) return;
        if (collision.gameObject.tag == "Player")
        {
            Kiritan player = collision.GetComponent<Kiritan>();
            if (player == null) return;
            if (GameManager.instance.IsClear()) return;
            if (player.IsDamage()) return;
            if (player.IsFly()) return;
            GameManager.instance.ClearDeal(transform.position);
        }
    }
}
