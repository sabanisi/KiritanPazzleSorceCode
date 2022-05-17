using UnityEngine;
using System.Collections;

public class ExtraMapManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMesh text;
    [SerializeField] private GameObject BlackBack;
    [SerializeField] private GameObject pressEnter;
    [SerializeField] private GameObject Hukidashi;
    private bool isAlive;
    // Use this for initialization
    void Start()
    {
        meshRenderer.sortingLayerName = "UI2";
        meshRenderer.sortingOrder = 1;
        isAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetButtonDown("Fly"))
                {
                    SceneChangeManager.SceneChange(SceneEnum.ExtraMapSelect, SceneEnum.MapCreate);
                    return;
                }
                if (Input.GetButtonDown("Shot"))
                {
                    SceneChangeManager.SceneChange(SceneEnum.ExtraMapSelect, SceneEnum.ExtraMapPlay);
                    return;
                }

                isAlive = false;
                GameManager.instance.GetPlayer().SetCanPlay(true);
                BlackBack.SetActive(false);
                Hukidashi.SetActive(false);
            }
        }
        else
        {
            if (pressEnter.activeSelf)
            {
                if (Input.GetButtonDown("Enter"))
                {
                    PressEnter();
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
        }
    }

    private void PressEnter()
    {
        BlackBack.SetActive(true);
        GameManager.instance.GetPlayer().SetCanPlay(false);
        isAlive = true;
        Hukidashi.SetActive(true);
    }
}
