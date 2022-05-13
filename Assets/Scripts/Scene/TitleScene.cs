using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour
{
    public static TitleScene instance;
    private bool isAlive;
    private bool isPentagon;
    [SerializeField] private SpriteRenderer Kiritan;
    [SerializeField] private SpriteRenderer Sentence1;
    [SerializeField] private SpriteRenderer Sentence2;

    [SerializeField] private PentagonCreater _pentagonCreater;
    private float flashCount;
    private float pentagonCount;

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

    public void Initialize()
    {
        Kiritan.gameObject.transform.position = new Vector3(10, 0, 0);
        Sentence1.gameObject.transform.position = new Vector3(0, 8, 0);
        Sentence2.gameObject.SetActive(false);
        Sentence2.color = new Color(1, 1, 1, 0);
        flashCount = 0;
        pentagonCount = 0;
        isAlive = false;
        isPentagon = true;

        Invoke("StartDeal", 0.7f);
    }

    private void StartDeal()
    {
        StartCoroutine(StartDealCoroutine());
    }

    private void CreateHashtable(GameObject target,float delay)
    {
        Hashtable move = new Hashtable();
        move.Add("position", new Vector3(0, 0, 0));
        move.Add("time", 0.5f);
        move.Add("delay", delay);
        move.Add("easeType", "easeOutSine");
        iTween.MoveTo(target, move);
    }

    private IEnumerator StartDealCoroutine()
    {
        CreateHashtable(Kiritan.gameObject, 0);
        CreateHashtable(Sentence1.gameObject, 0.4f);
        yield return new WaitForSeconds(1.1f);
        isAlive = true;
        Sentence2.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isAlive)
        {
            if (Input.GetButtonDown("Enter"))
            {
                SoundManager.PlaySE(SoundManager.SE_Type.PressEnter1);
                SoundManager.PlaySE(SoundManager.SE_Type.TitleVoice);
                SceneChangeManager.SceneChange(SceneEnum.Title, SceneEnum.ChutorialSelect);
                isAlive = false;
                isPentagon = false;
                _pentagonCreater.Cash();
                SoundManager.PlayBGM(SoundManager.BGM_Type.StageSelect);
            }
            float a = 0;
            if (flashCount <= 2.0f)
            {
                if (flashCount <= 1.0f)
                {
                    a = flashCount;
                }
                else
                {
                    a = 2 - flashCount;
                }
                flashCount += Time.deltaTime;
            }
            else
            {
                flashCount = 0;
            }
            Sentence2.color = new Color(1, 1, 1, a);
        }
        if (isPentagon)
        {
            if (pentagonCount >= 0.7f)
            {
                _pentagonCreater.CreatePentagon(false);
                pentagonCount = 0;
            }
            else
            {
                pentagonCount += Time.deltaTime;
            }
        }
    }
}
