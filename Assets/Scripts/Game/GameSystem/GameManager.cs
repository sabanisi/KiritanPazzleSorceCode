using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;
using static SceneMapStock;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] [Header("マップ生成機構")] private StageGenerator _stageGenarator;
    [SerializeField] [Header("カメラ制御")] private CameraManager _cameraManager;
    [SerializeField] private CinemachineImpulseSource DieImpulseSource;
    [SerializeField] private CinemachineImpulseSource TackleImpulseSource;

    [SerializeField] private Camera MainCamera;

    [SerializeField] private GameObject BlackBack;

    [SerializeField][Header("クリア時の文字")] private GameObject C, L, E, A, R;
    [SerializeField] private Text TimeCount;
    [SerializeField] private Text DeadCount;
    [SerializeField] private Text TimeText;
    [SerializeField] private Text DeadText;
    [SerializeField] private Text PressZ;

    [SerializeField] [Header("ステージ情報倉庫")] private SceneMapStock _sceneMapStock;
    private List<GameObject> charas = new List<GameObject>();

    private SceneEnum nowScene;
    private GameObject StageNodes;

    private Kiritan _player;
    public Kiritan GetPlayer()
    {
        return _player;
    }

    //ポーズ画面用フィールド
    [SerializeField] private Transform MainCameraTF;
    [SerializeField] private GameObject PauseObj;
    [SerializeField] private GameObject GoOnObj;
    [SerializeField] private GameObject RestartObj;
    [SerializeField] private GameObject GoBackObj;
    [SerializeField] private GameObject ExplainObj;
    [SerializeField] private Text GoOnText;
    [SerializeField] private Text RestartText;
    [SerializeField] private Text GoBackText;
    [SerializeField] private Text ExplainText;
    private bool isPause;//ポーズを管理するフラグ
    private bool isPausePrepare;//ポーズ準備・片付けも含めて管理するフラグ
    private int pauseStateNum;
    private bool isGameChange;
    public bool IsPause()
    {
        return isPause;
    }

    private bool isStop;
    public bool IsStop()
    {
        return isStop;
    }

    private float dieCount;
    private float time;

    private int dieNum;//死んだ数

    private bool isClear;
    public bool IsClear()
    {
        return isClear;
    }

    private bool isGoBackFrag;

    private Collider2D CameraLimitInstance;

    public CameraManager GetCameraManager()
    {
        return _cameraManager;
    }


    private void Start()
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

    private void Update()
    {
        if (!isPause)
        {
            if (Input.GetButtonDown("Pause")&&dieCount==0)
            {
                if (!isClear)
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.PauseVoice);
                    isPause = true;
                    isPausePrepare = true;
                    BlackBack.SetActive(true);
                    pauseStateNum = 1;
                    GoOnText.color = new Color(1, 1, 1);
                    RestartText.color = new Color(1, 1, 1);
                    GoBackText.color = new Color(1, 1, 1);
                    switch (pauseStateNum)
                    {
                        case 1:
                            GoOnText.color = new Color(1, 0.5f, 0);
                            break;
                        case 2:
                            RestartText.color = new Color(1, 0.5f, 0);
                            break;
                        case 3:
                            GoBackText.color = new Color(1, 0.5f, 0);
                            break;
                        default:
                            Debug.Log(pauseStateNum + "が用意されていない");
                            break;
                    }

                    Hashtable PausemoveHash = new Hashtable();
                    PausemoveHash.Add("position", new Vector3(-500, 350, 0));
                    PausemoveHash.Add("time", 0.15f);
                    PausemoveHash.Add("delay", 0f);
                    PausemoveHash.Add("easeType", "easeOutQuart");
                    PausemoveHash.Add("oncomplete", "PauseMoveFinish1");
                    PausemoveHash.Add("oncompletetarget", gameObject);
                    PausemoveHash.Add("isLocal", true);
                    iTween.MoveTo(PauseObj, PausemoveHash);
                }
            }
        }
        else
        {
            if (!isPausePrepare)
            {
                PauseDeal();
            }

            if (Input.GetButtonDown("Pause"))
            {
                PauseDealOfGoOn();
            }
        }

        if (isGoBackFrag)
        {
            if (Input.GetButtonDown("Enter"))
            {
                if (nowScene != SceneEnum.StageMapCreate)
                {
                    if (nowScene != SceneEnum.StageExtraMapPlay)
                    {
                        ClearFragManager.SetClearFrag(nowScene, true);
                    }
                }
                else
                {
                    MapMemory.instance.canClear = true;
                }

                GoStageSelect();
                SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
            }
        }

        if (dieCount > 0)
        {
            dieCount -= Time.deltaTime;
            if (dieCount <= 0)
            {
                dieCount = 0;

            }
        }
        time += Time.deltaTime;
    }

    public void Initialize(SceneEnum _nowScene)
    {
        MapStruct mapStruct = _sceneMapStock.GetMapStruct(_nowScene);
        nowScene = _nowScene;
        CameraLimitInstance = Instantiate(mapStruct.collider);
        _cameraManager.SetLimit(CameraLimitInstance);
        if (_nowScene.Equals(SceneEnum.StageMapCreate)||_nowScene.Equals(SceneEnum.StageExtraMapPlay))
        {
            CameraLimitInstance.transform.localPosition = new Vector3(-2.5f, -1.5f, 0);
        }

        dieNum = 0;
        time = 0;
        RestartDeal();
        TimeCount.enabled = false;
        DeadCount.enabled = false;
        TimeText.enabled = false;
        DeadText.enabled = false;
        PressZ.enabled = false;
        isGoBackFrag = false;
    }

    private void PauseDeal()
    {
        GoOnText.color = new Color(1, 1, 1);
        RestartText.color = new Color(1, 1, 1);
        GoBackText.color = new Color(1, 1, 1);
        switch (pauseStateNum)
        {
            case 1:
                GoOnText.color = new Color(1, 0.5f, 0);
                break;
            case 2:
                RestartText.color = new Color(1, 0.5f, 0);
                break;
            case 3:
                GoBackText.color = new Color(1, 0.5f, 0);
                break;
            default:
                Debug.Log(pauseStateNum + "が用意されていない");
                break;
        }
        if (Input.GetButtonDown("Up"))
        {
            pauseStateNum--;
            if (pauseStateNum < 1)
            {
                pauseStateNum = 3;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.CursleMouse);
        }
        if (Input.GetButtonDown("Down"))
        {
            pauseStateNum++;
            if (pauseStateNum > 3)
            {
                pauseStateNum = 1;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.CursleMouse);
        }
        if (Input.GetButtonDown("Enter") && !isGameChange)
        {
            switch (pauseStateNum)
            {
                case 1:
                    //続ける
                    PauseDealOfGoOn();
                    break;
                case 2:
                    //最初から
                    PauseDealOfRestart();
                    break;
                case 3:
                    //セレクト画面へ
                    PauseDealOfGoBack();
                    break;
                default:
                    Debug.Log(pauseStateNum + "がない");
                    break;
            }
        }
    }

    private void PauseDealOfGoOn()
    {
        isPausePrepare = true;
        CreateBackHashTable(1, PauseObj);
        CreateBackHashTable(2, GoOnObj);
        CreateBackHashTable(3, RestartObj);
        CreateBackHashTable(4, GoBackObj);
        CreateBackHashTable(5, ExplainObj);
        GoOnText.color = new Color(1, 1, 1);
        RestartText.color = new Color(1, 1, 1);
        GoBackText.color = new Color(1, 1, 1);
        SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
    }

    private void PauseDealOfRestart()
    {
        SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
        isGameChange = true;
        StartCoroutine(RestartEnumerator2());
    }

    private void PauseDealOfGoBack()
    {

        if (nowScene != SceneEnum.StageSelect1 && nowScene != SceneEnum.StageSelect2 && nowScene != SceneEnum.StageSelect3
            && nowScene != SceneEnum.StageSelect4 && nowScene != SceneEnum.StageSelect5 && nowScene != SceneEnum.StageSelect6
            &&nowScene != SceneEnum.ChutorialSelect && nowScene != SceneEnum.StageCredit && nowScene != SceneEnum.StageFinish
            && nowScene != SceneEnum.ExtraMapSelect)
        {
            isGameChange = true;
            GoStageSelect();
            SoundManager.StopBGM();
            SoundManager.PlaySE(SoundManager.SE_Type.GoBackVoice);
            SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
        }
        else
        {
            SoundManager.PlaySE(SoundManager.SE_Type.Cancel);
        }

    }

    private void PauseMoveFinish1()
    {
        CreateTextHashTable(0, GoOnObj);
        CreateTextHashTable(1, RestartObj);
        CreateTextHashTable(2, GoBackObj);
        CreateTextHashTable(3, ExplainObj);
    }

    private void CreateTextHashTable(int i, GameObject target)
    {
        Hashtable PausemoveHash = new Hashtable();
        if (i != 3)
        {
            PausemoveHash.Add("position", MainCameraTF.position + new Vector3(550, -i * 150, 0));
        }
        else
        {
            PausemoveHash.Add("position", MainCameraTF.position + new Vector3(-400, 150, 0));
        }
        
        PausemoveHash.Add("isLocal", true);
        PausemoveHash.Add("time", 0.15f);
        PausemoveHash.Add("delay", 0f);
        PausemoveHash.Add("easeType", "easeOutQuart");
        if (i == 2)
        {
            PausemoveHash.Add("oncomplete", "PauseMoveFinish2");
            PausemoveHash.Add("oncompletetarget", gameObject);
        }
        iTween.MoveTo(target, PausemoveHash);
    }

    private void PauseMoveFinish2()
    {
        isPausePrepare = false;
    }

    private void CreateBackHashTable(int i, GameObject Target)
    {
        Hashtable PausemoveHash = new Hashtable();
        float x = 1050;
        if (i == 1) { x = -1050; }
        if (i == 5) { x = -1200; }
        float y = 0;
        if (i == 1) { y = 200; }
        if (i == 5) { y = 600; }
        PausemoveHash.Add("position",MainCameraTF.position+ new Vector3(x, y- (i - 2) *150, 0));
        PausemoveHash.Add("time", 0.15f);
        PausemoveHash.Add("delay", 0f);
        PausemoveHash.Add("easeType", "easeOutQuart");
        if (i == 1)
        {
            PausemoveHash.Add("oncomplete", "PauseBackFinish");
            PausemoveHash.Add("oncompletetarget", gameObject);
        }
        PausemoveHash.Add("isLocal", true);
        iTween.MoveTo(Target, PausemoveHash);
    }

    private void PauseBackFinish()
    {
        isPause = false;
        isPausePrepare = false;
        BlackBack.SetActive(false);
    }

    private void GoStageSelect()
    {
        if (nowScene.Equals(SceneEnum.Stage1) || nowScene.Equals(SceneEnum.Stage8) ||
                    nowScene.Equals(SceneEnum.Stage10) || nowScene.Equals(SceneEnum.Stage15) ||
                    nowScene.Equals(SceneEnum.Stage9))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect1);
        }
        if (nowScene.Equals(SceneEnum.Stage7) || nowScene.Equals(SceneEnum.Stage5) ||
            nowScene.Equals(SceneEnum.Stage25) || nowScene.Equals(SceneEnum.Stage3) ||
            nowScene.Equals(SceneEnum.Stage11))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect2);
        }
        if (nowScene.Equals(SceneEnum.Stage16) || nowScene.Equals(SceneEnum.Stage19) ||
                   nowScene.Equals(SceneEnum.Stage22) || nowScene.Equals(SceneEnum.Stage14) ||
                   nowScene.Equals(SceneEnum.Stage30))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect3);
        }
        if (nowScene.Equals(SceneEnum.Stage12) || nowScene.Equals(SceneEnum.Stage28) ||
                   nowScene.Equals(SceneEnum.Stage4) || nowScene.Equals(SceneEnum.Stage20) ||
                   nowScene.Equals(SceneEnum.Stage26))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect4);
        }
        if (nowScene.Equals(SceneEnum.Stage13) || nowScene.Equals(SceneEnum.Stage2) ||
                  nowScene.Equals(SceneEnum.Stage29) || nowScene.Equals(SceneEnum.Stage21) ||
                  nowScene.Equals(SceneEnum.Stage6))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect5);
        }
        if (nowScene.Equals(SceneEnum.Stage27) || nowScene.Equals(SceneEnum.Stage24) ||
                 nowScene.Equals(SceneEnum.Stage17) || nowScene.Equals(SceneEnum.Stage18) ||
                 nowScene.Equals(SceneEnum.Stage23))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.StageSelect6);
        }
        if (nowScene.Equals(SceneEnum.Chutorial1)|| nowScene.Equals(SceneEnum.Chutorial2)
            || nowScene.Equals(SceneEnum.Chutorial3)||nowScene.Equals(SceneEnum.ExtraMapSelect))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.ChutorialSelect);
        }
        if (nowScene.Equals(SceneEnum.StageMapCreate))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.MapCreate);
        }
        if (nowScene.Equals(SceneEnum.StageExtraMapPlay))
        {
            SceneChangeManager.SceneChange(nowScene, SceneEnum.ExtraMapPlay);
        }
    }


    private void RestartDeal()
    {
        MapStruct mapStruct = _sceneMapStock.GetMapStruct(nowScene);

        if (StageNodes != null)
        {
            Destroy(StageNodes);
        }

        if (!nowScene.Equals(SceneEnum.StageMapCreate))
        {
            if (mapStruct.StageNodes != null)
            {
                StageNodes = Instantiate(mapStruct.StageNodes);
            }
        }
       
        if (_player != null)
        {
            Destroy(_player.gameObject);
        }

        if (!nowScene.Equals(SceneEnum.StageMapCreate)&&!nowScene.Equals(SceneEnum.StageExtraMapPlay))
        {
            _player = _stageGenarator.Initialize(mapStruct._tileMap);
            _player.Initialize();
            _cameraManager.Initialize(_player, mapStruct.canCameraMove, mapStruct.cameraPos, nowScene);
        }
        else
        {
            //ステージ作成とプレイヤー取得を同時に行う
            _player = _stageGenarator.InitializeByMapMemory();
            _player.Initialize();
            _cameraManager.Initialize(_player, false, new Vector2(-2.5f, -1.5f), nowScene);
        }
    
        dieCount = 0;
        isPause = false;
        isClear = false;
        isGameChange = false;
        dieNum++;

        PauseObj.transform.localPosition = MainCameraTF.position+new Vector3(-1050,350f, 0);
        GoOnObj.transform.localPosition = MainCameraTF.position + new Vector3(1050, 0, 0);
        RestartObj.transform.localPosition = MainCameraTF.position + new Vector3(1050,-150, 0);
        GoBackObj.transform.localPosition = MainCameraTF.position + new Vector3(1050, -300, 0);
        ExplainObj.transform.localPosition = MainCameraTF.position + new Vector3(-1200, 150, 0);
        BlackBack.SetActive(false);

        if (dieNum == 1)
        {
            if (nowScene != SceneEnum.StageSelect1 && nowScene != SceneEnum.StageSelect2
                && nowScene != SceneEnum.StageSelect3 && nowScene != SceneEnum.StageSelect4
                &&nowScene!=SceneEnum.StageSelect5&&nowScene!=SceneEnum.StageSelect6
                && nowScene != SceneEnum.ChutorialSelect&&nowScene!=SceneEnum.StageCredit
                &&nowScene!=SceneEnum.StageFinish && nowScene != SceneEnum.ExtraMapSelect)
            {
                SoundManager.PlaySE(SoundManager.SE_Type.Hue);
                SoundManager.PlayBGM(SoundManager.BGM_Type.Game);
            }
            else
            {
                SoundManager.PlayBGM(SoundManager.BGM_Type.StageSelect);
            }

        }
        Invoke("VoiceOfStart", 0.3f);
    }

    private void VoiceOfStart()
    {
        if (dieNum != 1)
        {
            SoundManager.PlaySE(SoundManager.SE_Type.ResponeVoice);
        }
    }

    public static void SetCameraTarget(Transform TF)
    {
        instance._cameraManager.SetTarget(TF);
    }

    public static void DieDeal()
    {
        instance.dieCount = 0.6f;
        instance.DieImpulseSource.GenerateImpulse();
        Restart();
    }

    public static void TackleShake()
    {
        instance.TackleImpulseSource.GenerateImpulse();
    }

    public static void Restart()
    {
        instance.StartCoroutine(instance.RestartEnumerator());
    }

    private IEnumerator RestartEnumerator()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RestartEnumerator2());
    }

    private IEnumerator RestartEnumerator2()
    {
        SceneChangeManager.CloseCurtain2();

        GameScene.DestroyStocks();
        RestartDeal();
        yield return new WaitForSeconds(0.05f);
        yield return SceneChangeManager.OpenCurtain2();
    }

    public void ClearDeal(Vector3 clearPos)
    {
        SoundManager.StopBGM();
        SoundManager.PlaySE(SoundManager.SE_Type.Clear);
        _cameraManager.StartClearZoom();
        Invoke("ZoomComplete", 0.5f);
        _player.ClearDeal(clearPos);
        isClear = true;
    }

    private void ZoomComplete()
    {
        Invoke("CreateC", 1.3f);
        Invoke("CreateL", 1.35f);
        Invoke("CreateE", 1.40f);
        Invoke("CreateA", 1.45f);
        Invoke("CreateR", 1.50f);

        Invoke("ShowText", 2.0f);
        Invoke("ShowTime", 2.5f);
        Invoke("ShowDead", 3.0f);
        Invoke("ShowPressZ", 3.6f);
    }

    private void ShowText()
    {
        TimeText.enabled = true;
        DeadText.enabled = true;
    }

    private void ShowTime()
    {
        string timeText = ((int)time / 60) + "分" + ((int)time % 60) + "秒";
        TimeCount.text = timeText;
        TimeCount.enabled = true;
        SoundManager.PlaySE(SoundManager.SE_Type.Pa1);
    }

    private void ShowDead()
    {
        DeadCount.text = dieNum + "回";
        DeadCount.enabled = true;
        SoundManager.PlaySE(SoundManager.SE_Type.Pa1);
    }

    private void ShowPressZ()
    {
       PressZ.enabled = true;
       isGoBackFrag = true;
       SoundManager.PlaySE(SoundManager.SE_Type.Pa2);
    }

    private void CreateC()
    {
        BlackBack.SetActive(true);
        GameObject obj = Instantiate(C, MainCamera.transform);
        obj.transform.localPosition = new Vector3(0, -3f, 10);
        charas.Add(obj);
    }

    private void CreateL()
    {
        GameObject obj = Instantiate(L, MainCamera.transform);
        obj.transform.localPosition = new Vector3(0, -3f, 10);
        charas.Add(obj);
    }

    private void CreateE()
    {
        GameObject obj = Instantiate(E, MainCamera.transform);
        obj.transform.localPosition = new Vector3(0, -3f, 10);
        charas.Add(obj);
    }

    private void CreateA()
    {
        GameObject obj = Instantiate(A, MainCamera.transform);
        obj.transform.localPosition = new Vector3(0, -3f, 10);
        charas.Add(obj);
    }

    private void CreateR()
    {
        GameObject obj = Instantiate(R, MainCamera.transform);
        obj.transform.localPosition = new Vector3(0, -3f, 10);
        charas.Add(obj);
    }

    public void CashDeal()
    {
        foreach (var obj in charas)
        {
            Destroy(obj);
        }
        charas.Clear();
        if (_player != null)
        {
            Destroy(_player.gameObject);
        }
        if (StageNodes != null)
        {
            Destroy(StageNodes);
        }
        BlackBack.SetActive(false);

        if (CameraLimitInstance != null)
        {
            Destroy(CameraLimitInstance.gameObject);
        }
    }
}
