using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using System;

public class MyNCMBManager : MonoBehaviour
{
    public static MyNCMBManager instance;

    private List<NCMBObject> loadObjs;
    public List<NCMBObject> GetLoadObjs()
    {
        return loadObjs;
    }
    public bool isLoadFinish { get; private set; }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PushMapInfo(string name,int[,] mapData)
    {
        int[] newMapData = new int[144];
        for (int x = 0; x <= 15; x++)
        {
            for(int y = 0; y <= 8; y++)
            {
                newMapData[x + y * 15] = mapData[x, y];
            }
        }

        NCMBObject obj = new NCMBObject("Map");

        obj["Name"] = name;
        obj["MapData"] = newMapData;
        obj["PlayNum"] = 0;
        obj["CreateTime"]=DateTime.Now;

        obj.Save((NCMBException e) =>
        {
            if (e == null)
            {
                //成功時
                Debug.Log("セーブ成功");
            }
            else
            {
                //エラー時
                Debug.Log("セーブ失敗");
            }
        });
    }

    public static void FetchList()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Map");
        if (instance.loadObjs == null)
        {
            instance.loadObjs = new List<NCMBObject>();
        }
        instance.loadObjs.Clear();
        instance.isLoadFinish = false;

        query.FindAsync((List<NCMBObject> _objList, NCMBException e) =>
        {
            if (e == null)
            {
                foreach (var obj in _objList)
                {
                    instance.loadObjs.Add(obj);
                }
                instance.isLoadFinish = true;
            }
            else
            {
                Debug.Log("ロード失敗");
            }
        });
        
    }
}
