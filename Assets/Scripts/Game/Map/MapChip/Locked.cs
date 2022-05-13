using UnityEngine;
using System.Collections;

public class Locked : MonoBehaviour
{
    [SerializeField]
    private Explosion ExplosionPrefab;
    
    public void Open()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(ExplosionPrefab, GameScene.instance.StockOfTransform).gameObject;
        explosion.transform.position = transform.position;
        GameManager.TackleShake();
        SoundManager.PlaySE(SoundManager.SE_Type.KeyOpen);
    }
}
