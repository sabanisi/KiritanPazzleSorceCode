using UnityEngine;
using System.Collections;

public class MapMemory : MonoBehaviour
{
    public static MapMemory instance;

    public MapChipEnum[,] stageArray;

    public bool canClear;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
