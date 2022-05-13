using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Kiritan parent;
    private Vector3 Speed;
    private Transform _transform;
    private float animationCount;
    private float finishCount = 0.2f;
    private bool isDelete;
    [SerializeField] private Explosion ExplosionPrefab, ExplosionPrefab2;
    public void Initialize(Kiritan _parent, Vector3 _speed)
    {
        parent = _parent;
        Speed = _speed;
        _transform = transform;
    }

    private void Update()
    {
        if (GameManager.instance.IsPause()) return;
        if (!isDelete)
        {
            _transform.position += Speed * Time.deltaTime;
        }
        if (animationCount > 0)
        {
            animationCount -= Time.deltaTime;
            if (animationCount <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (finishCount > 0)
        {
            finishCount -= Time.deltaTime;
            if (finishCount <= 0)
            {
                parent.FinishShot();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDelete) return;
        if (collision.tag == "Block")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            BlockChip block = collision.gameObject.GetComponent<BlockChip>();
            if (block != null&&block.IsCanDestory())
            {
                GameObject explosion = Instantiate(ExplosionPrefab, GameScene.instance.StockOfTransform).gameObject;
                explosion.transform.position = collision.transform.position;
                Destroy(collision.gameObject);
                animationCount = 0.3f;
                GameManager.TackleShake();
                SoundManager.PlaySE(SoundManager.SE_Type.KnifeHit);
            }
            else
            {
                GameObject explosion = Instantiate(ExplosionPrefab2,GameScene.instance.StockOfTransform).gameObject;
                explosion.transform.position = transform.position;
                animationCount = 0.2f;
                SoundManager.PlaySE(SoundManager.SE_Type.KnifeNotHit);
            }
            isDelete = true;
        }
    }
}
