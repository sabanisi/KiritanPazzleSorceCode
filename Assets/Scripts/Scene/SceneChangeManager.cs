using UnityEngine;
using System.Collections;

public class SceneChangeManager: MonoBehaviour
{
    public static SceneChangeManager instance { get; private set; }

    [SerializeField] private TitleScene _titleScene;
    [SerializeField] private GameScene _gameScene;
    [SerializeField] private MapCreateScene _mapCreateScene;
    [SerializeField] private ExtraMapPlayScene _extraMapPlayScene;

    [SerializeField] private Transform CurtainTF;
    [SerializeField] private GameObject Curtain;

    [SerializeField] private GameObject Curtain2;
    [SerializeField] private SpriteRenderer Curtain2Sprite;

    private SceneEnum nowScene;

    private bool isChange;

    void Start()
    {
        SaveSystem.Instance.Load();
        SoundManager.PlayBGM(SoundManager.BGM_Type.Title);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SceneChange(SceneEnum.None, SceneEnum.Title);
    }

    public static void SceneChange(SceneEnum fromSceneEnum, SceneEnum toSceneEnum)
    {
        if (instance.isChange) return;
        instance.StartCoroutine(instance.SceneChangeDeal(fromSceneEnum, toSceneEnum));
    }

    private IEnumerator SceneChangeDeal(SceneEnum fromSceneEnum, SceneEnum toSceneEnum)
    {
        isChange = true;
        if (fromSceneEnum != SceneEnum.None)
        {
            CurtainMove(0, 0.5f, 0.2f, "easeInCubic");
            yield return new WaitForSeconds(0.55f);

            if (!fromSceneEnum.Equals(SceneEnum.Title)&&!fromSceneEnum.Equals(SceneEnum.MapCreate))
            {
                _gameScene.CashDeal();
            }
        }

        _titleScene.gameObject.SetActive(false);
        _gameScene.gameObject.SetActive(false);
        _mapCreateScene.gameObject.SetActive(false);
        _extraMapPlayScene.gameObject.SetActive(false);

        if (toSceneEnum.Equals(SceneEnum.Title))
        {
            _titleScene.gameObject.SetActive(true);
            _titleScene.Initialize();
            SoundManager.PlayBGM(SoundManager.BGM_Type.Title);
        }
        else if (toSceneEnum.Equals(SceneEnum.MapCreate))
        {
            _mapCreateScene.gameObject.SetActive(true);
            _mapCreateScene.Initialize(fromSceneEnum);
        }
        else if (toSceneEnum.Equals(SceneEnum.ExtraMapPlay))
        {
            _extraMapPlayScene.gameObject.SetActive(true);
            _extraMapPlayScene.Initialize(fromSceneEnum);
        }
        else
        {
            _gameScene.gameObject.SetActive(true);
            _gameScene.Initialize(toSceneEnum);
        }
        isChange = false;
        Curtain2Sprite.color = new Color(0.7f, 0.7f, 0.7f, 0);

        if (!fromSceneEnum.Equals(SceneEnum.None))
        {
           
            yield return new WaitForSeconds(0.4f);
            if(fromSceneEnum.Equals(SceneEnum.ExtraMapPlay))
            {
               StartCoroutine(WaitExtraMapInitialize());
                yield return new WaitForSeconds(0.1f);
            }
        }
        CurtainMove(10f, 0.5f, 0f, "easeOutCubic");

        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator WaitExtraMapInitialize()
    {
        while (!_extraMapPlayScene.IsReady())
        {
            yield return null;
        }
    }

    private void CurtainMove(float toY, float time, float delay, string easeType)
    {
        Hashtable moveTable = new Hashtable();
        moveTable.Add("position", new Vector3(0, toY, 1));
        moveTable.Add("time", time);
        moveTable.Add("delay", delay);
        moveTable.Add("easyType", easeType);
        iTween.MoveTo(Curtain, moveTable);
    }

    public static IEnumerator CloseCurtain()
    {
        instance.CurtainMove(0, 0.5f, 0f, "easeInCubic");
        yield return new WaitForSeconds(0.8f);
    }

    public static IEnumerator OpenCurtain()
    {
        instance.CurtainMove(10f, 0.5f, 0f, "easeInCubic");
        yield return new WaitForSeconds(0.5f);
    }

    public static void CloseCurtain2()
    {
        instance.Curtain2Sprite.color = new Color(0.7f, 0.7f, 0.7f, 1);
    }

    public static IEnumerator OpenCurtain2()
    {
        float a = 1f;
        while (true)
        {
            a -= Time.deltaTime*1.5f;
            if (a <= 0)
            {
                break;
            }
            else
            {
                instance.Curtain2Sprite.color = new Color(0.7f, 0.7f, 0.7f, a);
                yield return null;
            }
        }
    }
}
