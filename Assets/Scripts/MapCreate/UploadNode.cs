using UnityEngine;
using System.Collections;

public class UploadNode : MonoBehaviour
{
    [SerializeField] private MapCreateManager parent;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;

    public void OnMouseEnter()
    {
        if (parent.isDisplayUploadPanel) return;
        if (!MapMemory.instance.canClear) return;
        sprite.sprite = sprite2;
        transform.localScale = new Vector3(2.5f, 2.5f, 0);
    }

    public void OnMouseExit()
    {
        if (!MapMemory.instance.canClear) return;
        sprite.sprite = sprite1;
        transform.localScale = new Vector3(2, 2, 0);
    }

    public void CannotClick()
    {
        sprite.sprite = sprite3;
        transform.localScale = new Vector3(2, 2, 0);
    }

    public void OnMouseClick()
    {
        if (parent.isDisplayUploadPanel) return;
        if (MapMemory.instance.canClear)
        {
            OnMouseExit();
            parent.DisplayUploadPanel();
        }
        else
        {
            parent.DisplayUploadAlart();
        }
    }
}