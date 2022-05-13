using UnityEngine;
using System.Collections;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;
    public Transform StockOfTransform;

    public static GameScene instance { get; private set; }
    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(SceneEnum sceneEnum)
    {
        _GameManager.Initialize(sceneEnum);
    }

    public static void DestroyStocks()
    {
        foreach(Transform _transform in instance.StockOfTransform)
        {
            if (_transform != null)
            {
                Destroy(_transform.gameObject);
            }
        }
    }

    public void CashDeal()
    {
        _GameManager.CashDeal();
        DestroyStocks();
    }
}
