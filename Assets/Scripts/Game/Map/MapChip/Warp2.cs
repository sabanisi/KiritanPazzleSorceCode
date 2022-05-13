using UnityEngine;
using System.Collections;

public class Warp2 : MonoBehaviour
{
    [SerializeField] private Warp warp;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (warp.isWarpOff)
            {
                warp.isWarpOff = false;
                collision.gameObject.GetComponent<Kiritan>().SetIsInWarpBlock(false);
            }
        }
    }
}
