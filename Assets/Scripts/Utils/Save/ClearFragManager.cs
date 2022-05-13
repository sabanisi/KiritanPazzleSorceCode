using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class ClearFragManager
{
    private static ClearFragManager instance=new ClearFragManager();
    public static ClearFragManager Instance => instance;
    private static bool clear1, clear2, clear3, clear4, clear5, clear6, clear7, clear8, clear9, clear10,
        clear11, clear12, clear13, clear14, clear15, clear16, clear17, clear18, clear19, clear20,
        clear21, clear22, clear23, clear24, clear25, clear26, clear27, clear28, clear29, clear30,
        clearChutorial1, clearChutorial2, clearChutorial3;

    private List<ClearFrag> dictionary = new List<ClearFrag>()
    {
        new ClearFrag(SceneEnum.Chutorial1,clearChutorial1 ),
        new ClearFrag(SceneEnum.Chutorial2,clearChutorial2 ),
        new ClearFrag(SceneEnum.Chutorial3,clearChutorial3 ),

        new ClearFrag(SceneEnum.Stage1,clear1 ),
        new ClearFrag(SceneEnum.Stage2,clear2 ),
        new ClearFrag(SceneEnum.Stage3,clear3 ),
        new ClearFrag(SceneEnum.Stage4,clear4 ),
        new ClearFrag(SceneEnum.Stage5,clear5 ),
        new ClearFrag(SceneEnum.Stage6,clear6 ),
        new ClearFrag(SceneEnum.Stage7,clear7 ),
        new ClearFrag(SceneEnum.Stage8,clear8 ),
        new ClearFrag(SceneEnum.Stage9,clear9 ),
        new ClearFrag(SceneEnum.Stage10,clear10 ),
        new ClearFrag(SceneEnum.Stage11,clear11 ),
        new ClearFrag(SceneEnum.Stage12,clear12 ),
        new ClearFrag(SceneEnum.Stage13,clear13 ),
        new ClearFrag(SceneEnum.Stage14,clear14 ),
        new ClearFrag(SceneEnum.Stage15,clear15 ),
        new ClearFrag(SceneEnum.Stage16,clear16 ),
        new ClearFrag(SceneEnum.Stage17,clear17 ),
        new ClearFrag(SceneEnum.Stage18,clear18 ),
        new ClearFrag(SceneEnum.Stage19,clear19 ),
        new ClearFrag(SceneEnum.Stage20,clear20 ),
        new ClearFrag(SceneEnum.Stage21,clear21 ),
        new ClearFrag(SceneEnum.Stage22,clear22 ),
        new ClearFrag(SceneEnum.Stage23,clear23 ),
        new ClearFrag(SceneEnum.Stage24,clear24 ),
        new ClearFrag(SceneEnum.Stage25,clear25 ),
        new ClearFrag(SceneEnum.Stage26,clear26 ),
        new ClearFrag(SceneEnum.Stage27,clear27 ),
        new ClearFrag(SceneEnum.Stage28,clear28 ),
        new ClearFrag(SceneEnum.Stage29,clear29 ),
        new ClearFrag(SceneEnum.Stage30,clear30 )
    };

    public List<ClearFrag> GetDictionary()
    {
        return dictionary;
    }

    public static void SetClearFrag(SceneEnum sceneEnum,bool isClear)
    {
        foreach(var value in instance.dictionary)
        {
            if (value.sceneEnum == sceneEnum)
            {
                value.isClear = isClear;
                break;
            }
        }
        SaveSystem.Instance.Save();
    }
    public static bool GetClearFrag(SceneEnum sceneEnum)
    {
        foreach(var value in instance.dictionary)
        {
            if (value.sceneEnum == sceneEnum)
            {
                return value.isClear;
            }
        }
        Debug.Log("GetClearFragで" + sceneEnum + "がない");
        return false;
    }

    public static bool IsAllClear()
    {
        foreach(var value in instance.dictionary)
        {
            if (value.sceneEnum != SceneEnum.Chutorial1 && value.sceneEnum != SceneEnum.Chutorial2 && value.sceneEnum != SceneEnum.Chutorial3)
            {
                if (!value.isClear)
                {
                    return false;
                }
            }
        }
        return true;
    }    
}
