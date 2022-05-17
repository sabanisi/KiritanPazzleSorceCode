using UnityEngine;
using System.Collections;

public class NoNode2 : MonoBehaviour
{
    [SerializeField] private MapCreateManager parent;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;

    public void OnMouseEnter()
    {
        sprite.sprite = sprite2;
        transform.localScale = new Vector3(2.5f, 2.5f, 0);
    }

    public void OnMouseExit()
    {
        sprite.sprite = sprite1;
        transform.localScale = new Vector3(2, 2, 0);
    }

    public void OnMouseClick()
    {
        parent.GoBack();
    }
}
