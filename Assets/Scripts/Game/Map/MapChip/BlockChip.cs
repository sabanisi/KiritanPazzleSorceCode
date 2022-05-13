using UnityEngine;
using System.Collections;

public class BlockChip : MonoBehaviour
{
    [SerializeField] private bool canDestroy;
    public bool IsCanDestory()
    {
        return canDestroy;
    }

    [SerializeField] private bool canMove;
    public bool IsCanMove()
    {
        return canMove;
    }

    [SerializeField] private bool canKill;
    public bool IsCanKill()
    {
        return canKill;
    }
}
