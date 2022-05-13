using UnityEngine;
using System.Collections;

public class FlyCheck : MonoBehaviour
{
    [SerializeField] private FlyEnum flyEnum;
    [SerializeField] private Kiritan parent;
    [SerializeField] private GameObject ExplosionPrefab;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!parent.GetFlyEnum().Equals(flyEnum)) return;
        if (collision.gameObject.tag != "Block") return;
        if (!parent.IsFly()) return;

        BlockChip blockChip = collision.gameObject.GetComponent<BlockChip>();
        if (blockChip!=null&&blockChip.IsCanMove())
        {
            Direction direction = Direction.Up;
            if (parent.GetFlyEnum() == FlyEnum.Horizontal)
            {
                if (parent.GetFlySpeed().x > 0)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Left;
                }
            }
            else
            {
                if (parent.GetFlySpeed().y < 0)
                {
                    direction = Direction.Down;
                }
            }
            ((MoveBlockChip)blockChip).SetIsMoving(true, direction);
        }
        parent.FinishFly();
        Instantiate(ExplosionPrefab,GameScene.instance.StockOfTransform).transform.position = transform.position;
        GameManager.TackleShake();
        SoundManager.PlaySE(SoundManager.SE_Type.Explosion2);
    }
}
