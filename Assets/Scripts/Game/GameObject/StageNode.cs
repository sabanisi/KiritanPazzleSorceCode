using UnityEngine;
using System.Collections;

public class StageNode : MonoBehaviour
{
    [SerializeField] private GameObject pressEnter;
    [SerializeField] private SceneEnum sceneEnum;
    [SerializeField] private SpriteRenderer numberSprite;
    [SerializeField] private GameObject ClearHukidashi;
    [SerializeField] private bool isBack;

    void Start()
    {
        if (numberSprite != null)
        {
            if (ClearFragManager.GetClearFrag(sceneEnum))
            {
                numberSprite.color = new Color(1, 1, 1);
            }
            else
            {
                numberSprite.color = new Color(0, 0, 0);
            }
        }
        if (ClearHukidashi != null)
        {
            if (ClearFragManager.GetClearFrag(sceneEnum))
            {
                ClearHukidashi.SetActive(true);
            }
            else
            {
                ClearHukidashi.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (GameManager.instance.IsPause()) return;
        if (pressEnter.activeSelf)
        {
            if (Input.GetButtonDown("Enter"))
            {
                if (sceneEnum != SceneEnum.StageFinish)
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.PressEnter2);
                }
                GoStage();
            }
        }
    }

    private void GoStage()
    {
        if (sceneEnum == SceneEnum.StageFinish)
        {
            if (ClearFragManager.IsAllClear())
            {
                SoundManager.PlaySE(SoundManager.SE_Type.ClearVoice3);
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.StageFinish);
            }
            else
            {
                SoundManager.PlaySE(SoundManager.SE_Type.Cancel);
            }
            return;
        }

        if (sceneEnum != SceneEnum.ChutorialSelect && sceneEnum != SceneEnum.StageSelect1 &&
            sceneEnum != SceneEnum.StageSelect2 && sceneEnum != SceneEnum.StageSelect3 &&
            sceneEnum != SceneEnum.StageSelect4&&sceneEnum!=SceneEnum.StageSelect5 &&
            sceneEnum!=SceneEnum.StageSelect6&&
            sceneEnum!=SceneEnum.Title&&sceneEnum!=SceneEnum.StageCredit)
        {
            SoundManager.StopBGM();
            SoundManager.PlaySE(SoundManager.SE_Type.StartVoice);
        }
        else if(sceneEnum==SceneEnum.Title)
        {
            SoundManager.StopBGM();
            SoundManager.PlaySE(SoundManager.SE_Type.GoTitleVoice);
        }
        else
        {
            if (sceneEnum != SceneEnum.StageCredit)
            {
                if (isBack)
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.GoBackStageVoice);
                }
                else
                {
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        SoundManager.PlaySE(SoundManager.SE_Type.StageChangeVoice);
                    }
                    else
                    {
                        SoundManager.PlaySE(SoundManager.SE_Type.StageChangeVoice2);
                    }
                }
            }
        }
        switch (sceneEnum)
        {
            case SceneEnum.Stage1:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Stage1);
                break;
            case SceneEnum.Stage2:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Stage2);
                break;
            case SceneEnum.Stage3:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Stage3);
                break;
            case SceneEnum.Stage4:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Stage4);
                break;
            case SceneEnum.Stage5:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Stage5);
                break;
            case SceneEnum.Stage6:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage6);
                break;
            case SceneEnum.Stage7:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage7);
                break;
            case SceneEnum.Stage8:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage8);
                break;
            case SceneEnum.Stage9:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage9);
                break;
            case SceneEnum.Stage10:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage10);
                break;
            case SceneEnum.Stage11:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.Stage11);
                break;
            case SceneEnum.Stage12:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.Stage12);
                break;
            case SceneEnum.Stage13:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.Stage13);
                break;
            case SceneEnum.Stage14:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.Stage14);
                break;
            case SceneEnum.Stage15:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.Stage15);
                break;
            case SceneEnum.Stage16:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect4, SceneEnum.Stage16);
                break;
            case SceneEnum.Stage17:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage17);
                break;
            case SceneEnum.Stage18:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage18);
                break;
            case SceneEnum.Stage19:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage19);
                break;
            case SceneEnum.Stage20:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.Stage20);
                break;
            case SceneEnum.Stage21:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.Stage21);
                break;
            case SceneEnum.Stage22:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.Stage22);
                break;
            case SceneEnum.Stage23:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.Stage23);
                break;
            case SceneEnum.Stage24:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.Stage24);
                break;
            case SceneEnum.Stage25:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.Stage25);
                break;
            case SceneEnum.Stage26:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect6, SceneEnum.Stage26);
                break;
            case SceneEnum.Stage27:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect6, SceneEnum.Stage27);
                break;
            case SceneEnum.Stage28:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect6, SceneEnum.Stage28);
                break;
            case SceneEnum.Stage29:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect6, SceneEnum.Stage29);
                break;
            case SceneEnum.Stage30:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect6, SceneEnum.Stage30);
                break;

            case SceneEnum.StageSelect1:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.StageSelect1);
                break;
            case SceneEnum.StageSelect2:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.StageSelect2);
                break;
            case SceneEnum.StageSelect3:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect2, SceneEnum.StageSelect3);
                break;
            case SceneEnum.StageSelect4:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect3, SceneEnum.StageSelect4);
                break;
            case SceneEnum.StageSelect5:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect4, SceneEnum.StageSelect5);
                break;
            case SceneEnum.StageSelect6:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect5, SceneEnum.StageSelect6);
                break;

            case SceneEnum.Chutorial1:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Chutorial1);
                break;
            case SceneEnum.Chutorial2:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Chutorial2);
                break;
            case SceneEnum.Chutorial3:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.Chutorial3);
                break;
            case SceneEnum.ChutorialSelect:
                SceneChangeManager.SceneChange(SceneEnum.StageSelect1, SceneEnum.ChutorialSelect);
                break;

            case SceneEnum.StageCredit:
                SceneChangeManager.SceneChange(SceneEnum.ChutorialSelect, SceneEnum.StageCredit);
                break;

            case SceneEnum.Title:
                SceneChangeManager.SceneChange(SceneEnum.ChutorialSelect, SceneEnum.Title);
                break;

            case SceneEnum.ExtraMapSelect:
                SceneChangeManager.SceneChange(SceneEnum.ChutorialSelect, SceneEnum.ExtraMapSelect);
                break;

            default:
                Debug.Log("SelectNodeのGostageでバグ");
                break;
        }
        pressEnter.SetActive(false);
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

}
