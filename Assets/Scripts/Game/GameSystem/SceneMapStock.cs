using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class SceneMapStock : MonoBehaviour
{
    [SerializeField] public MapStruct Stage1;
    [SerializeField] public MapStruct Stage2;
    [SerializeField] public MapStruct Stage3;
    [SerializeField] public MapStruct Stage4;
    [SerializeField] public MapStruct Stage5;
    [SerializeField] public MapStruct Stage6;
    [SerializeField] public MapStruct Stage7;
    [SerializeField] public MapStruct Stage8;
    [SerializeField] public MapStruct Stage9;
    [SerializeField] public MapStruct Stage10;
    [SerializeField] public MapStruct Stage11;
    [SerializeField] public MapStruct Stage12;
    [SerializeField] public MapStruct Stage13;
    [SerializeField] public MapStruct Stage14;
    [SerializeField] public MapStruct Stage15;
    [SerializeField] public MapStruct Stage16;
    [SerializeField] public MapStruct Stage17;
    [SerializeField] public MapStruct Stage18;
    [SerializeField] public MapStruct Stage19;
    [SerializeField] public MapStruct Stage20;
    [SerializeField] public MapStruct Stage21;
    [SerializeField] public MapStruct Stage22;
    [SerializeField] public MapStruct Stage23;
    [SerializeField] public MapStruct Stage24;
    [SerializeField] public MapStruct Stage25;
    [SerializeField] public MapStruct Stage26;
    [SerializeField] public MapStruct Stage27;
    [SerializeField] public MapStruct Stage28;
    [SerializeField] public MapStruct Stage29;
    [SerializeField] public MapStruct Stage30;
    [SerializeField] public MapStruct StageSelect1;
    [SerializeField] public MapStruct StageSelect2;
    [SerializeField] public MapStruct StageSelect3;
    [SerializeField] public MapStruct StageSelect4;
    [SerializeField] public MapStruct StageSelect5;
    [SerializeField] public MapStruct StageSelect6;
    [SerializeField] public MapStruct StageChutorial1;
    [SerializeField] public MapStruct StageChutorial2;
    [SerializeField] public MapStruct StageChutorial3;
    [SerializeField] public MapStruct StageChutorialSelect;
    [SerializeField] public MapStruct StageCredit;
    [SerializeField] public MapStruct StageFinish;
    [SerializeField] public MapStruct ExtraMapSelect;

    [System.Serializable]
    [SerializeField]
    public struct MapStruct
    {
        public Tilemap _tileMap;
        public Collider2D collider;
        public bool canCameraMove;
        public Vector2 cameraPos;
        public GameObject StageNodes;
    }

    public MapStruct GetMapStruct(SceneEnum sceneEnum)
    {
        switch (sceneEnum)
        {
            case SceneEnum.Stage1:
                return Stage1;
            case SceneEnum.Stage2:
                return Stage2;
            case SceneEnum.Stage3:
                return Stage3;
            case SceneEnum.Stage4:
                return Stage4;
            case SceneEnum.Stage5:
                return Stage5;
            case SceneEnum.Stage6:
                return Stage6;
            case SceneEnum.Stage7:
                return Stage7;
            case SceneEnum.Stage8:
                return Stage8;
            case SceneEnum.Stage9:
                return Stage9;
            case SceneEnum.Stage10:
                return Stage10;
            case SceneEnum.Stage11:
                return Stage11;
            case SceneEnum.Stage12:
                return Stage12;
            case SceneEnum.Stage13:
                return Stage13;
            case SceneEnum.Stage14:
                return Stage14;
            case SceneEnum.Stage15:
                return Stage15;
            case SceneEnum.Stage16:
                return Stage16;
            case SceneEnum.Stage17:
                return Stage17;
            case SceneEnum.Stage18:
                return Stage18;
            case SceneEnum.Stage19:
                return Stage19;
            case SceneEnum.Stage20:
                return Stage20;
            case SceneEnum.Stage21:
                return Stage21;
            case SceneEnum.Stage22:
                return Stage22;
            case SceneEnum.Stage23:
                return Stage23;
            case SceneEnum.Stage24:
                return Stage24;
            case SceneEnum.Stage25:
                return Stage25;
            case SceneEnum.Stage26:
                return Stage26;
            case SceneEnum.Stage27:
                return Stage27;
            case SceneEnum.Stage28:
                return Stage28;
            case SceneEnum.Stage29:
                return Stage29;
            case SceneEnum.Stage30:
                return Stage30;
            case SceneEnum.StageSelect1:
                return StageSelect1;
            case SceneEnum.StageSelect2:
                return StageSelect2;
            case SceneEnum.StageSelect3:
                return StageSelect3;
            case SceneEnum.StageSelect4:
                return StageSelect4;
            case SceneEnum.StageSelect5:
                return StageSelect5;
            case SceneEnum.StageSelect6:
                return StageSelect6;
            case SceneEnum.Chutorial1:
                return StageChutorial1;
            case SceneEnum.Chutorial2:
                return StageChutorial2;
            case SceneEnum.Chutorial3:
                return StageChutorial3;
            case SceneEnum.ChutorialSelect:
                return StageChutorialSelect;
            case SceneEnum.StageCredit:
                return StageCredit;
            case SceneEnum.ExtraMapSelect:
                return ExtraMapSelect;


            case SceneEnum.StageMapCreate:
                return Stage1;
            case SceneEnum.StageExtraMapPlay:
                return Stage1;

            case SceneEnum.StageFinish:
                Debug.Log(sceneEnum + " がない");
                break;
        }
        return Stage1;
    }
}
