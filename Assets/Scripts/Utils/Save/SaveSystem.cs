using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections.Specialized;

public class SaveSystem
{
    private static SaveSystem instance=new SaveSystem();

    public static SaveSystem Instance => instance;

    public void Load()
    {
        string text = PlayerPrefs.GetString("Data","んんんんん");
        if (text != "んんんんん")
        {
            List<ClearFrag> dictionary = ClearFragManager.Instance.GetDictionary();
            List<bool> clearFlags = CreateBoolList(text, dictionary);
            for(int i=0;i<clearFlags.Count;i++)
            {
                dictionary[i].isClear = clearFlags[i];
            }
        }
        else
        {
            foreach(var value in ClearFragManager.Instance.GetDictionary())
            {
                value.isClear = false;
            }
        }
    }

    private List<bool> CreateBoolList(string text, List<ClearFrag> dictionary)
    {
        List<bool> clearFlags = new List<bool>();
        int index = 0;
        foreach (char chara in text)
        {
            int num = Conversion.CharToInt(chara);
            if (num != -1)
            {
                int subScriptNum = 4;
                while (true)
                {
                    index++;
                    if (index > dictionary.Count)
                    {
                        return clearFlags;
                    }
                    if (num >= Mathf.Pow(2, subScriptNum))
                    {
                        clearFlags.Add(true);
                        num -= (int)Mathf.Pow(2, subScriptNum);
                    }
                    else
                    {
                        clearFlags.Add(false);
                    }
                    subScriptNum--;
                    if (subScriptNum < 0)
                    {
                        break;
                    }
                }
            }
        }
        return clearFlags;
    }

    public void Save()
    {
        ClearFragManager flagManager = ClearFragManager.Instance;
        string text = "";
        int subScriptNum=4;
        int num = 0;
        foreach(var value in flagManager.GetDictionary())
        {
            if (value.isClear)
            {
                num += (int)Mathf.Pow(2, subScriptNum);
            }
            subScriptNum--;
            if (subScriptNum <0)
            {
                char chara = Conversion.IntToChar(num);
                num = 0;
                subScriptNum = 4;
                text += chara;
            }
        }
        if (subScriptNum !=4)
        {
            char chara = Conversion.IntToChar(num);
            text += chara;
        }
        PlayerPrefs.SetString("Data", text);
        PlayerPrefs.Save();
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }
}
