using UnityEngine;
using System.Collections;

public class UpDownButtonNode : MonoBehaviour
{
    [SerializeField] private ExtraMapPlayManager parent;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private bool isUp;

    public void OnMouseEnter()
    {
        sprite.color = new Color(1,0.521f,0.275f);
        transform.localScale = new Vector3(0.9f, 0.9f, 0);
    }

    public void OnMouseExit()
    {
        sprite.color = new Color(1, 1, 1);
        transform.localScale = new Vector3(0.7f,0.7f, 0);
    }

    public void OnMouseClick()
    {
        if (isUp)
        {
            parent.UpIndex();
        }
        else
        {
            parent.DownIndex();
        }
    }
}