using UnityEngine;
using System.Collections;

public class StartChip : MonoBehaviour
{
    [SerializeField] private Kiritan PlayerPrefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Kiritan Initialize()
    {
        Kiritan player = Instantiate(PlayerPrefab,transform.parent.parent);
        player.transform.position = transform.localPosition;
        return player;
    }
}
