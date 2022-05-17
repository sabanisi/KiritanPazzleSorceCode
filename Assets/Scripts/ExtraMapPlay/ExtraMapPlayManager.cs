using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public class ExtraMapPlayManager : MonoBehaviour
{
    [SerializeField] private List<SelectMapNode> selectMapNodes = new List<SelectMapNode>();

    private List<NCMBObject> mapDatas;
    [SerializeField]private Text numberText;

    private int indexNum;

    public bool UpIndex()
    {
        if (indexNum > (mapDatas.Count - 1) / 9)
        {
            return false;
        }
        indexNum++;
        UpdateView();
        return true;
    }

    public bool DownIndex()
    {
        if (indexNum < 1)
        {
            return false;
        }
        indexNum --;
        UpdateView();
        return true;
    }

    public void Initialize()
    {
        indexNum = 1;
        StartCoroutine(StartDeal());
    }

    private IEnumerator StartDeal()
    {
        MyNCMBManager.FetchList();

        while (!MyNCMBManager.instance.isLoadFinish)
        {
            yield return null;
        }
        mapDatas = MyNCMBManager.instance.GetLoadObjs();
        SortByDate();
        UpdateView();
        ExtraMapPlayScene.instance.SetIsReady(true);

        yield return null;
    }

    //表示するマップを更新する
    private void UpdateView()
    {
        int index = (indexNum-1) * 9;
        int newIndex = 0;

        foreach(var obj in selectMapNodes)
        {
            obj.gameObject.SetActive(false);
        }

        for(int i = 0; i <= 8; i++)
        {
            newIndex = index + i;
            if (newIndex >= mapDatas.Count)
            {
                newIndex--;
                break;
            }
            NCMBObject mapData = mapDatas[newIndex];
            Debug.Log(mapData["Name"]);
            Debug.Log(mapData["MapData"]);
            string name = (string)mapData["Name"];
            int[] map = new int[144];
            ArrayList list = (ArrayList)mapData["MapData"];
            for (int j = 0; j < list.Count; j++)
            {
                if (j < 144)
                {
                    map[j] = Convert.ToInt32(list[j]);
                }
            }

            selectMapNodes[i].gameObject.SetActive(true);
           
            selectMapNodes[i].Initialize(name,map);
        }
        int startIndex = index + 1;
        int endIndex = newIndex + 1;
        numberText.text =startIndex + "-" + endIndex + "件目表示中(全" + mapDatas.Count + "件)";
    }

    //ゲーム開始
    public void StartGame(int index)
    {
        MapMemory.instance.stageArray = selectMapNodes[index].GetMapData();
        SceneChangeManager.SceneChange(SceneEnum.ExtraMapPlay, SceneEnum.StageExtraMapPlay);
    }


    //日付(降順)で並べ替え
    private void SortByDate()
    {
        mapDatas.Sort(SortByDateAuxiliary);
    }

    private int SortByDateAuxiliary(NCMBObject a,NCMBObject b)
    {
        if (a["CreateTime"] == null)
        {
            return 1;
        }
        if (b["CreateTime"] == null)
        {
            return - 1;
        }

        return ((DateTime)b["CreateTime"]).CompareTo((DateTime)a["CreateTime"]);
    }

    private void SortByPopularity()
    {

    }

    private void SortByDifficulty()
    {

    }

    //終わる
    public void GoBack()
    {
        SceneChangeManager.SceneChange(SceneEnum.ExtraMapPlay, SceneEnum.ExtraMapSelect);
    }
}
