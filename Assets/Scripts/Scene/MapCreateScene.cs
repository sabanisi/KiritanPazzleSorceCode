using UnityEngine;
using System.Collections;

public class MapCreateScene : MonoBehaviour
{
    public static MapCreateScene instance;
    [SerializeField] private MapCreateManager mapCreateManager;
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

    public void Initialize(SceneEnum fromSceneEnum)
    {
        if (!fromSceneEnum.Equals(SceneEnum.StageMapCreate))
        {
            mapCreateManager.Initialize();
        }
        else
        {
            mapCreateManager.Restart();
        }
    }
}
