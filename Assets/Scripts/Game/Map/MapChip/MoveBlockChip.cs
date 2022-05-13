using System;
using UnityEngine;

public class MoveBlockChip:BlockChip
{
    [SerializeField] private float Speed;

    private bool isMoving;//移動中かどうか
    private Direction MoveDirection;

    public void SetIsMoving(bool _isMoving, Direction _moveDirection)
    {
        isMoving = _isMoving;
        MoveDirection = _moveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPause())
        {
            if (isMoving)
            {
                Vector3 speed = Vector3.zero;
                switch (MoveDirection)
                {
                    case Direction.Up:
                        speed = new Vector3(0, Speed, 0);
                        break;
                    case Direction.Down:
                        speed = new Vector3(0, -Speed, 0);
                        break;
                    case Direction.Left:
                        speed = new Vector3(-Speed, 0, 0);
                        break;
                    case Direction.Right:
                        speed = new Vector3(Speed, 0, 0);
                        break;
                }
                transform.position += speed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving&&collision.gameObject.tag == "Block")
        {
            Stop(collision.gameObject.transform.position, collision.gameObject.transform.localScale);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMoving && collision.gameObject.tag == "Block")
        {
            Stop(collision.gameObject.transform.position, collision.gameObject.transform.localScale);
        }
    }

    private void Stop(Vector3 pos,Vector3 scale)
    {
        isMoving = false;
        switch (MoveDirection)
        {
            case Direction.Down:
                float y = pos.y + transform.localScale.y;
                transform.position = new Vector3(transform.position.x, y, 0);
                break;
            case Direction.Up:
                float y2 = pos.y - scale.y;
                transform.position = new Vector3(transform.position.x, y2, 0);
                break;
            case Direction.Right:
                float x = pos.x - transform.localScale.x;
                transform.position = new Vector3(x, transform.position.y, 0);
                break;
            case Direction.Left:
                float x2 = pos.x + scale.x;
                transform.position = new Vector3(x2, transform.position.y, 0);
                break;
        }
        SoundManager.PlaySE(SoundManager.SE_Type.Explosion);
    }
}
