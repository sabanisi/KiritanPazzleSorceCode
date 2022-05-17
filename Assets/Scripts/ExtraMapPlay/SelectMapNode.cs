using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SelectMapNode : MonoBehaviour
{
    [SerializeField] private TextMesh Name;
    [SerializeField] private Transform BlockParent;
    [SerializeField] private GameObject Node;
    [SerializeField] private SpriteRenderer backNode;
    [SerializeField] private BlockChipImage BlockImage;
    [SerializeField] private GameObject BlockPrefab;

    private MapChipEnum[,] mapData;
    public MapChipEnum[,] GetMapData()
    {
        return mapData;
    }

    [SerializeField] private ExtraMapPlayManager parent;
    [SerializeField] private int index;


    [System.Serializable]
    [SerializeField]
    private struct BlockChipImage
    {
        public Sprite Start, Goal, Block, CanDestroyBlock, CanMoveBlock, CanKillBlock, Key, Lock, Warp;
    }

    public void Initialize(string name,int[] _mapData)
    {
        Delete();
        SetInfo(name, _mapData);

        CreateMapData();
    }

    private void Delete()
    {
        if (mapData != null)
        {
            mapData = null;
        }
        mapData = new MapChipEnum[16, 9];
        foreach (Transform tf in BlockParent)
        {
            Destroy(tf.gameObject);
        }

    }

    private void SetInfo(string name,int[] _mapData)
    {
        Name.text= name;

        for(int x = 0; x <= 15; x++)
        {
            for(int y = 0; y <= 8; y++)
            {
                mapData[x, y] = (MapChipEnum)Enum.ToObject(typeof(MapChipEnum), _mapData[x + y * 15]);
            }
        }
    }

    private void CreateMapData()
    {
        for(int x = 0; x <= 15; x++)
        {
            for(int y = 0; y <= 8; y++)
            {
                if (!mapData[x, y].Equals(MapChipEnum.Transparent))
                {
                    Sprite sprite = ChooseSprite(mapData[x, y]);
                    GameObject gameObject = Instantiate(BlockPrefab);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                    gameObject.transform.parent = BlockParent;
                    gameObject.transform.localPosition = new Vector3(-1.125f + x * 0.15f, -0.5f + y * 0.15f);
                }
            }
        }
    }

    private Sprite ChooseSprite(MapChipEnum mapChip)
    {
        switch (mapChip)
        {
            case MapChipEnum.Start:
                return BlockImage.Start;
            case MapChipEnum.Goal:
                return BlockImage.Goal;
            case MapChipEnum.Block:
                return BlockImage.Block;
            case MapChipEnum.CanDestroyBlock:
                return BlockImage.CanDestroyBlock;
            case MapChipEnum.CanMoveBlock:
                return BlockImage.CanMoveBlock;
            case MapChipEnum.CanKillBlock:
                return BlockImage.CanKillBlock;
            case MapChipEnum.Key:
                return BlockImage.Key;
            case MapChipEnum.Lock:
                return BlockImage.Lock;
            case MapChipEnum.Warp:
                return BlockImage.Warp;
            default:
                Debug.Log(mapChip + "がない");
                return null;
        }
    }

    public void OnMouseEnter()
    {
        Node.transform.localScale = new Vector3(1.1f, 1.1f, 0);
        backNode.color = new Color(1, 0.521f, 0.275f,(float)200/255);
    }

    public void OnMouseExit()
    {
        Node.transform.localScale = new Vector3(1, 1, 0);
        backNode.color = new Color(1, 1, 1, (float)200 / 255);
    }

    public void OnClick()
    {
        OnMouseExit();
        parent.StartGame(index);
    }
}
