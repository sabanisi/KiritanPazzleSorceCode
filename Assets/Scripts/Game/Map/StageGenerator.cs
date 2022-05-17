using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] private  string BlockName = "Block1";
    [SerializeField] private BlockChip BlockPrefab;

    [SerializeField] private string CanDestroyBlockName = "Block2";
    [SerializeField] private BlockChip BlockPrefab2;

    [SerializeField] private string CanMoveBlockName = "Block3";
    [SerializeField] private MoveBlockChip BlockPrefab3;

    [SerializeField] private string CanKillBlockName = "Block4";
    [SerializeField] private BlockChip BlockPrefab4;

    [SerializeField] private string StartName = "Start";
    [SerializeField] private StartChip StartPrefab;

    [SerializeField] private string GoalName = "Goal";
    [SerializeField] private GoalChip GoalPrefab;

    [SerializeField] private Key KeyPrefab;
    [SerializeField] private Locked LockedPrefab;
    [SerializeField] private Warp WarpPrefab;
    private Key keyInstance;
    private Locked lockedInstance;
    private Warp warpInstance;

    [SerializeField] private Transform _transform;

    private StartChip _startChip;


    public Kiritan Initialize(Tilemap tile)
    {
        keyInstance = null;
        lockedInstance = null;
        warpInstance = null;

        DestroyBlock();
        CreateMap(tile);
        return _startChip.Initialize();
    }

    private void DestroyBlock()
    {
        foreach(Transform childTransform in transform)
        {
            if (childTransform != null)
            {
                Destroy(childTransform.gameObject);
            }
        }
    }

    public void CreateMap(Tilemap _tileMap)
    {
        BoundsInt bounds = _tileMap.cellBounds;
        TileBase[] allBlocks = _tileMap.GetTilesBlock(bounds);
        float sX = bounds.x+0.5f;
        float sY = bounds.y+0.5f;
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tileBase = allBlocks[x + y * bounds.size.x];
                if (tileBase != null)
                {
                    if (tileBase.name == BlockName)
                    {
                        CreateMapChip(MapChipEnum.Block,new Vector3(sX+x,sY+ y, 0));
                    }
                    else if(tileBase.name==StartName)
                    {
                        CreateMapChip(MapChipEnum.Start, new Vector3(sX + x, sY + y, 0));
                    }
                    else if (tileBase.name == CanDestroyBlockName)
                    {
                        CreateMapChip(MapChipEnum.CanDestroyBlock, new Vector3(sX + x, sY + y, 0));
                    }
                    else if (tileBase.name == CanMoveBlockName)
                    {
                        CreateMapChip(MapChipEnum.CanMoveBlock, new Vector3(sX + x, sY + y, 0));
                    }
                    else if (tileBase.name == CanKillBlockName)
                    {
                        CreateMapChip(MapChipEnum.CanKillBlock, new Vector3(sX + x, sY + y, 0));
                    }else if (tileBase.name == GoalName)
                    {
                        CreateMapChip(MapChipEnum.Goal, new Vector3(sX + x, sY + y, 0));
                    }
                }
            }
        }
    }

    private void CreateMapChip(MapChipEnum chipEnum,Vector3 pos)
    {
        switch (chipEnum)
        {
            case MapChipEnum.Block:
                BlockChip block = Instantiate(BlockPrefab, _transform);
                block.transform.position = pos;
                break;
            case MapChipEnum.CanDestroyBlock:
                BlockChip block2 = Instantiate(BlockPrefab2, _transform);
                block2.transform.position = pos;
                break;
            case MapChipEnum.CanMoveBlock:
                BlockChip block3 = Instantiate(BlockPrefab3, _transform);
                block3.transform.position = pos;
                break;
            case MapChipEnum.Start:
                StartChip start = Instantiate(StartPrefab,_transform);
                start.transform.position = pos;
                _startChip = start;
                break;
            case MapChipEnum.CanKillBlock:
                BlockChip block4 = Instantiate(BlockPrefab4, _transform);
                block4.transform.position = pos;
                break;
            case MapChipEnum.Goal:
                GoalChip goal = Instantiate(GoalPrefab, _transform);
                goal.transform.position = pos;
                break;
            default:
                Debug.Log(chipEnum + "がない");
                break;
        }
    }

    //MapMemoryの情報を元にマップを作成する
    public Kiritan InitializeByMapMemory()
    {
        DestroyBlock();
        keyInstance = null;
        lockedInstance = null;
        warpInstance = null;
        MapChipEnum[,] stageArray = MapMemory.instance.stageArray;

        //外周にブロックを配置
        for(int x = -1; x <= 16; x++)
        {
            BlockChip block = Instantiate(BlockPrefab, _transform);
            block.transform.localPosition = new Vector3(x,-1, 0);
            BlockChip block2 = Instantiate(BlockPrefab, _transform);
            block2.transform.localPosition = new Vector3(x,9, 0);
        }
        for(int y = 0; y <= 8; y++)
        {
            BlockChip block = Instantiate(BlockPrefab, _transform);
            block.transform.localPosition = new Vector3(-1,y, 0);
            BlockChip block2 = Instantiate(BlockPrefab, _transform);
            block2.transform.localPosition = new Vector3(16,y, 0);
        }

        for (int x = 0; x <stageArray.GetLength(0); x++)
        {
            for(int y = 0; y < stageArray.GetLength(1); y++)
            {
                switch (stageArray[x, y])
                {
                    case MapChipEnum.Block:
                        BlockChip block = Instantiate(BlockPrefab, _transform);
                        block.transform.localPosition = new Vector3(x, y, 0);
                        break;
                    case MapChipEnum.CanDestroyBlock:
                        BlockChip block2 = Instantiate(BlockPrefab2, _transform);
                        block2.transform.localPosition = new Vector3(x, y, 0);
                        break;
                    case MapChipEnum.CanMoveBlock:
                        BlockChip block3 = Instantiate(BlockPrefab3, _transform);
                        block3.transform.localPosition = new Vector3(x, y, 0);
                        break;
                    case MapChipEnum.Start:
                        StartChip start = Instantiate(StartPrefab, _transform);
                        start.transform.localPosition = new Vector3(x,y,0);
                        _startChip = start;
                        break;
                    case MapChipEnum.Goal:
                        GoalChip goal = Instantiate(GoalPrefab, _transform);
                        goal.transform.localPosition = new Vector3(x, y, 0);
                        break;
                    case MapChipEnum.CanKillBlock:
                        BlockChip block4 = Instantiate(BlockPrefab4, _transform);
                        block4.transform.localPosition = new Vector3(x,y,0);
                        break;
                    case MapChipEnum.Key:
                        Key key = Instantiate(KeyPrefab, _transform);
                        key.transform.localPosition = new Vector3(x, y, 0);
                        keyInstance = key;
                        break;
                    case MapChipEnum.Lock:
                        Locked locked = Instantiate(LockedPrefab, _transform);
                        locked.transform.localPosition = new Vector3(x, y, 0);
                        lockedInstance = locked;
                        break;
                    case MapChipEnum.Warp:
                        Warp warp = Instantiate(WarpPrefab, _transform);
                        warp.transform.localPosition = new Vector3(x, y, 0);
                        if (warpInstance == null)
                        {
                            warpInstance = warp;
                        }
                        else
                        {
                            warpInstance._warp = warp;
                            warp._warp = warpInstance;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        if (keyInstance != null && lockedInstance != null)
        {
            keyInstance.locked = lockedInstance;
        }
        return _startChip.Initialize();
    }
}
