using UnityEngine;
using System.Collections;

public class Pentagon : MonoBehaviour
{
    private Vector3 Speed;
    private float rotateSpeed;
    private bool isMove;
    public void SetSpeed(Vector3 _speed, float _rotate)
    {
        Speed = _speed;
        rotateSpeed = _rotate;
    }
    public void SetIsMove(bool _isMove)
    {
        isMove = _isMove;
    }

    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }


    void Update()
    {
        if (isMove)
        {
            _transform.position += Speed * Time.deltaTime;
            _transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));

            if (_transform.position.x >= 16 && _transform.position.y <= -10)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
