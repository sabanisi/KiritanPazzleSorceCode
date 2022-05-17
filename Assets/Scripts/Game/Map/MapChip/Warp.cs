using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour
{
    public Warp _warp;
    public bool isWarpOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isWarpOff)
            {
                collision.gameObject.transform.position = _warp.transform.position;
                _warp.isWarpOff = true;
                collision.gameObject.GetComponent<Kiritan>().SetIsInWarpBlock(true);
                SoundManager.PlaySE(SoundManager.SE_Type.Warp);
            }
        }
        
    }
}
