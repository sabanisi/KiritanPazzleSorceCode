using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    private float count;
    // Use this for initialization
    void Start()
    {
        count = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPause())
        {
            _sprite.color = new Color((1 - count * 5), 1, 1);
            count -= Time.deltaTime;
            if (count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
