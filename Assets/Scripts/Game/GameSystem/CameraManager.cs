using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _Camera;
    [SerializeField] private CinemachineConfiner _CameraConfiner;
    [SerializeField] private CinemachineVirtualCameraBase _cinemachineVirtualCameraBase;
    [SerializeField][Header("クリア時の深度")] private float clearOrthgraphSize=2.5f;
    private bool isFollow;
    private Kiritan _player;

    

    private float clearCount;
    public void StartClearZoom()
    {
        clearCount = 0.5f;
        beforeSize = _Camera.m_Lens.OrthographicSize;
        _Camera.Follow = _player.transform;
    }
    private float beforeSize;


    public void Initialize(Kiritan player,bool _isFollow,Vector2 pos,SceneEnum nowSceneEnum)
    {
        isFollow = _isFollow;
        _player = player;
        if (isFollow)
        {
            _Camera.Follow = player.transform;
            _cinemachineVirtualCameraBase.PreviousStateIsValid = false;
            _Camera.transform.position = player.transform.position;
        }
        else
        {
            _cinemachineVirtualCameraBase.PreviousStateIsValid = false;
            _Camera.transform.position = new Vector3(pos.x,pos.y,-10);
        }
        if (nowSceneEnum == SceneEnum.Chutorial1 || nowSceneEnum == SceneEnum.Chutorial2 || nowSceneEnum == SceneEnum.Chutorial3||nowSceneEnum==SceneEnum.StageCredit)
        {
            _Camera.m_Lens.OrthographicSize = 3.5f;
        }
        else
        {
            _Camera.m_Lens.OrthographicSize = 4.5f;
        }
        clearCount = 0;
    }

    public void SetLimit(Collider2D limitCollider)
    {
        //カメラ制限区域を設定
        _CameraConfiner.m_BoundingShape2D = limitCollider;
    }

    // Update is called once per frame
    void Update()
    {
        if (clearCount > 0)
        {
            clearCount -= Time.deltaTime;
            if (clearCount > 0)
            {
                _Camera.m_Lens.OrthographicSize = clearOrthgraphSize + (beforeSize - clearOrthgraphSize) * clearCount;
            }
        }
    }

    public void SetTarget(Transform TF)
    {
        if (isFollow)
        {
            _Camera.Follow = TF;
        }
    }

    public void SetOrthographsSize(float size)
    {
        _Camera.m_Lens.OrthographicSize = size;
    }
}
