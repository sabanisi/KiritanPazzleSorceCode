using UnityEngine;
using System.Collections;

public class MapChipNode : MonoBehaviour
{
    [SerializeField] private MapChipEnum mapChipEnum;
    [SerializeField] private MapCreateManager parent;
    [SerializeField] private SpriteRenderer backImage;

    private bool isSelect;
    public void SetIsSelect(bool _isSelect)
    {
        isSelect = _isSelect;
    }
    public MapChipEnum GetMapChipEnum()
    {
        return mapChipEnum;
    }

    public void OnMouseClick()
    {
        if (parent.isDisplayUploadPanel) return;
        parent.ChangeNowChipEnum(mapChipEnum);
        backImage.color = new Color(1.0f,0.34f, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 0);
        isSelect = true;
    }

    public void OnMouseEnter()
    {
        if (parent.isDisplayUploadPanel) return;
        if (isSelect) return;
        backImage.color = new Color(1.0f,0.64f,0.46f);
        transform.localScale = new Vector3(1.1f, 1.1f, 0);
    }

    public void OnMouseExit()
    {
        if (parent.isDisplayUploadPanel) return;
        if (isSelect) return;
        backImage.color = new Color(1, 1, 1);
        transform.localScale = new Vector3(0.95f, 0.95f, 0);
    }
}
