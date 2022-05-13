using UnityEngine;
using System.Collections;

public class InitializeBlock : MonoBehaviour
{
    [SerializeField] private GameObject pressEnter;
    [SerializeField] private GameObject reallyOk;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsPause()) return;
        if (pressEnter.activeSelf)
        {
            if (Input.GetButtonDown("Enter"))
            {
                SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
                if (!reallyOk.activeSelf)
                {
                    reallyOk.SetActive(true);
                    GameManager.instance.GetCameraManager().SetOrthographsSize(1.5f);
                }
                else
                {
                    SaveSystem.Instance.Delete();
                    SceneChangeManager.SceneChange(SceneEnum.StageFinish, SceneEnum.Title);
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEnter.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEnter.SetActive(false);
            reallyOk.SetActive(false);
            GameManager.instance.GetCameraManager().SetOrthographsSize(4.5f);
        }
    }

}
