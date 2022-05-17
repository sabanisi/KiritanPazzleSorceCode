using UnityEngine;
using System.Collections;

public class BackCollider: MonoBehaviour
{
    private MapCreateManager parent;
    private Vector2Int position;
    [SerializeField]private SpriteRenderer sprite;

    public void SetSprite(Sprite _sprite)
    {
        sprite.sprite = _sprite;
    }

    public Sprite GetSprite()
    {
        return sprite.sprite;
    }

    public void SetAlpha(float alpha)
    {
        sprite.color = new Color(1, 1, 1, alpha);
    }


    public void Initialize(MapCreateManager _parent,int x,int y)
    {
        parent = _parent;
        position = new Vector2Int(x,y);
    }

    public void OnClick()
    {
        if (parent.isDisplayUploadPanel) return;
        parent.PutBlock(position.x, position.y,this,true);
    }
    public void OnEnter()
    {
        if (parent.isDisplayUploadPanel) return;
        parent.PutBlock(position.x, position.y, this, false);
    }

    public void OnExit()
    {
        if (parent.isDisplayUploadPanel) return;
        parent.MouseExit(position.x, position.y, this);
    }
}
