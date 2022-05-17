using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private BackCollider BackColliderPrefab;
    [SerializeField] private Transform BackColliderParent;

    [SerializeField] private BlockChipImage BlockImage;
    [SerializeField] private PlayNode playButtonNode;
    [SerializeField] private UploadNode uploadButtonNode;
    [SerializeField] private FinishNode finishButtonNode;
    [SerializeField] private GameObject UploadPanel;
    [SerializeField] private GameObject UploadFinishPanel;
    [SerializeField] private InputField stageNameInputField;
    [SerializeField] private NoNode noNode;
    [SerializeField] private YesNode yesNode;
    [SerializeField] private YesNode2 yesNode2;
    [SerializeField] private NoNode2 noNode2;


    [System.Serializable]
    [SerializeField]
    private struct BlockChipImage
    {
        public Sprite NotBrush,Start,Goal,Block,CanDestroyBlock,CanMoveBlock,CanKillBlock,Key,Lock,Warp,Eraser,Transparent;
    }

    //現在のブラシ
    private MapChipEnum nowChipEnum;

    //ブロックボタンs
    [SerializeField] private List<MapChipNode> mapChipNodes;

    //ステージ配列
    private MapChipEnum[,] stageArray = new MapChipEnum[16,9];
    private BackCollider[,] backColliders = new BackCollider[16, 9];

    //警告文
    [SerializeField]private Text AlartSentence;
    private float alartCount;

    //ステージ公開用パネル表示の有無
    public bool isDisplayUploadPanel { get; private set; }

    //初期化処理
    public void Initialize()
    {
        mapChipNodes[0].OnMouseClick();

        //ブロック設置の当たり判定作成
        for (int x = 0; x <= 15; x++)
        {
            for (int y = 0; y <= 8; y++)
            {
                if (backColliders[x, y] != null)
                {
                    Destroy(backColliders[x, y].gameObject);
                    backColliders[x, y] = null;
                }

                BackCollider collider = Instantiate(BackColliderPrefab);
                collider.Initialize(this, x, y);
                collider.transform.parent = BackColliderParent;
                collider.transform.localScale = new Vector3(1, 1, 0);
                collider.transform.localPosition = new Vector3(x, y, 0);
                collider.SetSprite(ChooseMapChipImage(stageArray[x, y]));
                backColliders[x, y] = collider;
            }
        }

        Restart();
    }

    //再開処理(テスト画面から帰ってきた時)
    public void Restart()
    {
        AbondonUpload();
        GoOnCreateStage();

        AlartSentence.enabled = false;
        alartCount = 0;
        for(int x = 0; x <= 15; x++)
        {
            for(int y = 0; y <= 8; y++)
            {
                backColliders[x, y].SetAlpha(1.0f);
                backColliders[x, y].SetSprite(ChooseMapChipImage(stageArray[x, y]));
            }
        }
        playButtonNode.OnMouseExit();
        finishButtonNode.OnMouseExit();
        if (MapMemory.instance.canClear)
        {
            uploadButtonNode.OnMouseExit();
        }
        else
        {
            uploadButtonNode.CannotClick();
        }
        stageNameInputField.text = "";
    }

    private void Update()
    {
        if (alartCount > 0)
        {
            alartCount -= Time.deltaTime;
            if (alartCount <= 0)
            {
                alartCount = 0;
                AlartSentence.enabled = false;
            }
        }
    }

    //プレイスタート
    public void PlayStart()
    {
        //外周がブロックか髑髏で覆われているか
        if (!IsExitBlockOutside())
        {
            DisplayAlart("外周が覆われていません");
            return;
        }

        //スタート、ゴールがあるか
        Vector2Int startGoalNum = CountStartAndGoalChip();
        if (startGoalNum.x == 0)
        {
            DisplayAlart("きりたんがいません");
            return;
        }else if (startGoalNum.x >= 2)
        {
            DisplayAlart("きりたんは一人しか設置できません");
            return;
        }
        if (startGoalNum.y == 0)
        {
            DisplayAlart("ゴールがありません");
            return;
        }
        else if (startGoalNum.y >= 2)
        {
            DisplayAlart("ゴールは二つ以上設置できません");
            return;
        }

        //鍵、ワープのチェック
        Vector3Int keyWarpNum = CountKeyWarpChip();
        if (keyWarpNum.x >= 2 || keyWarpNum.y >= 2)
        {
            DisplayAlart("鍵ブロックは一組しか\n設置できません");
            return;
        }else if (keyWarpNum.x != keyWarpNum.y)
        {
            DisplayAlart("鍵ブロックは必ずセットで\n設置しなければなりません");
            return;
        }
        if (keyWarpNum.z ==1)
        {
            DisplayAlart("ワープブロックは必ず二個セットで\n設置しなければなりません");
            return;
        }else if (keyWarpNum.z >=3)
        {
            DisplayAlart("ワープブロックは一組しか\n設置できません");
            return;
        }

        MapMemory.instance.stageArray = stageArray;

        SceneChangeManager.SceneChange(SceneEnum.MapCreate, SceneEnum.StageMapCreate);

    }

    //警告文を表示する
    private void DisplayAlart(string sentence)
    {
        AlartSentence.enabled = true;
        AlartSentence.text = sentence;
        alartCount = 1.5f;
    }
    
    //鍵ブロック、ワープブロックの数を数える
    private Vector3Int CountKeyWarpChip()
    {
        Vector3Int chipNum = new Vector3Int();
        for (int x = 0; x <= 15; x++)
        {
            for (int y = 0; y <= 8; y++)
            {
                if (stageArray[x, y] == MapChipEnum.Key)
                {
                    chipNum.x++;
                }
                if (stageArray[x, y] == MapChipEnum.Lock)
                {
                    chipNum.y++;
                }
                if (stageArray[x, y] == MapChipEnum.Warp)
                {
                    chipNum.z++;
                }
            }
        }
        return chipNum;
    }

    //スタート・ゴール地点の数を数える
    private Vector2Int CountStartAndGoalChip()
    {
        Vector2Int chipNum=new Vector2Int();
        for(int x = 0; x <= 15; x++)
        {
            for(int y = 0; y <= 8; y++)
            {
                if (stageArray[x, y] == MapChipEnum.Start)
                {
                    chipNum.x++;
                }
                if (stageArray[x, y] == MapChipEnum.Goal)
                {
                    chipNum.y++;
                }
            }
        }
        return chipNum;
    }

    //外周がブロックか髑髏で覆われているか
    private bool IsExitBlockOutside()
    {
        bool outsideCheckFlag = false;
        for (int x = 0; x <= 15; x++)
        {
            if (stageArray[x, 0] != MapChipEnum.Block && stageArray[x, 0] != MapChipEnum.CanKillBlock)
            {
                outsideCheckFlag = true;
            }
            if (stageArray[x, 8] != MapChipEnum.Block && stageArray[x, 8] != MapChipEnum.CanKillBlock)
            {
                outsideCheckFlag = true;
            }
        }
        for (int y = 1; y <= 7; y++)
        {
            if (stageArray[0, y] != MapChipEnum.Block && stageArray[0, y] != MapChipEnum.CanKillBlock)
            {
                outsideCheckFlag = true;
            }
            if (stageArray[15, y] != MapChipEnum.Block && stageArray[15, y] != MapChipEnum.CanKillBlock)
            {
                outsideCheckFlag = true;
            }
        }
        if (outsideCheckFlag)
        {
            return false;
        }
        return true;
    }

    //マウスがクリックされ、ブロックを設置する時
    public void PutBlock(int x,int y,BackCollider collider,bool isClick)
    {
        if (nowChipEnum == MapChipEnum.NotBrush) return;

        Sprite image;
        MapChipEnum mapChipEnum = MapChipEnum.Transparent;
        if (nowChipEnum == MapChipEnum.Eraser)
        {
            image = BlockImage.Transparent;
        }
        else
        {
            image = ChooseMapChipImage(nowChipEnum);
            mapChipEnum = nowChipEnum;
        }

        if (Input.GetMouseButton(0)||isClick)
        {
            if (stageArray[x, y] != mapChipEnum)
            {
                stageArray[x, y] = mapChipEnum;
                collider.SetAlpha(1.0f);
                collider.SetSprite(image);
                MapMemory.instance.canClear = false;
                uploadButtonNode.CannotClick();
            }
        }
        else
        {
            if (nowChipEnum != MapChipEnum.Eraser)
            {
                collider.SetSprite(image);
                collider.SetAlpha(0.6f);
            }
            else
            {
                collider.SetAlpha(0.2f);
            }
        }
    }

    //マウスがマス目から離れた時
    public void MouseExit(int x,int y,BackCollider collider)
    {
        if (nowChipEnum == MapChipEnum.Transparent) return;
        MapChipEnum mapChipEnum = MapChipEnum.Transparent;
        if (nowChipEnum != MapChipEnum.Eraser)
        {
            mapChipEnum = nowChipEnum;
        }
        if (stageArray[x, y] != mapChipEnum)
        {
            collider.SetSprite(ChooseMapChipImage(stageArray[x,y]));
        }
        collider.SetAlpha(1.0f);
    }

    //MapChipNodeがクリックされた時
    public void ChangeNowChipEnum(MapChipEnum mapChipEnum)
    {
        nowChipEnum = mapChipEnum;
        foreach(var mapChipNode in mapChipNodes)
        {
            if (!mapChipNode.GetMapChipEnum().Equals(mapChipEnum))
            {
                mapChipNode.SetIsSelect(false);
                mapChipNode.OnMouseExit();
            }
        }
    }

    private Sprite ChooseMapChipImage(MapChipEnum mapChip)
    {
        switch (mapChip)
        {
            case MapChipEnum.Transparent:
                return BlockImage.Transparent;
            case MapChipEnum.NotBrush:
                return BlockImage.NotBrush;
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
            case MapChipEnum.Eraser:
                return BlockImage.Eraser;
            default:
                Debug.Log(mapChip + "がない");
                return null;
        }
    }

    //公開に関する警告文を表示する
    public void DisplayUploadAlart()
    {
        DisplayAlart("先に一回プレイして\nクリアできる事を確認しましょう");
    }


    //ステージ公開用パネル表示
    public void DisplayUploadPanel()
    {
        isDisplayUploadPanel = true;
        UploadPanel.SetActive(true);
    }


    //ステージ公開中止
    public void AbondonUpload()
    {
        isDisplayUploadPanel = false;
        UploadPanel.SetActive(false);
        yesNode.OnMouseExit();
        noNode.OnMouseExit();
    }

    //ステージ公開
    public void UploadStage()
    {
        //名前チェック
        string name = stageNameInputField.text;
        if (name.Trim().Length == 0)
        {
            Debug.Log("名前が0文字");
            return;
        }

        //ステージ公開処理
        //マップの情報をint型に直す
        int[,] mapData = new int[16, 9];
        for(int x = 0; x <= 15; x++)
        {
            for(int y=0;y<=8; y++)
            {
                mapData[x, y] = (int)stageArray[x, y];
            }
        }
        MyNCMBManager.PushMapInfo(name, mapData);

        UploadPanel.SetActive(false);
        UploadFinishPanel.SetActive(true);
        yesNode.OnMouseExit();
        noNode.OnMouseExit();
    }

    //ステージ制作を続ける
    public void GoOnCreateStage()
    {
        UploadFinishPanel.SetActive(false);
        isDisplayUploadPanel = false;
        yesNode2.OnMouseExit();
        noNode2.OnMouseExit();
    }

    //ステージ制作をやめる
    public void GoBack()
    {
        SceneChangeManager.SceneChange(SceneEnum.MapCreate, SceneEnum.ExtraMapSelect);
    }
}
