using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsPause())
        {
            _animator.speed = 0;
        }
        else
        {
            _animator.speed = 1;
        }
    }
}
