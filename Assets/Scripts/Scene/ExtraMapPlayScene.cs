using UnityEngine;
using System.Collections;

public class ExtraMapPlayScene : MonoBehaviour
{
    public static ExtraMapPlayScene instance;
    [SerializeField] private ExtraMapPlayManager manager;

    private bool isReady;
    public bool IsReady()
    {
        return isReady;
    }
    public void SetIsReady(bool _isReady)
    {
        isReady = _isReady;
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

    public void Initialize(SceneEnum fromScene)
    {
        isReady = false;
        if (!fromScene.Equals(SceneEnum.StageExtraMapPlay))
        {
            manager.Initialize();
        }
    }
}
