using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiritan : MonoBehaviour
{

    [SerializeField][Header("歩く速さ")] private float speed;
    [SerializeField] [Header("ジャンプ力")] private float jumpHeight;
    [SerializeField] [Header("地面との接触判定")] private GroundCheck _groundCheck;

    [SerializeField] [Header("飛行モード時の炎")] private GameObject Fire;
    [SerializeField] [Header("炎の位置")] private FirePos _FirePos;
    [SerializeField] [Header("包丁投げ用のプレハブ")] private Shot ShotPrefab;
    
    [SerializeField][Header("きりたん画像")]private KiritanImage KiriImage;

    [SerializeField] [Header("ヒットエフェクト")] private GameObject HitEffect;

    [System.Serializable][SerializeField]
    private struct KiritanImage
    {
        public Sprite flyWait,flyVertical,flyHorizontal,Shot1,Shot2,Shot3,Shot4,Damage;
    }
    [System.Serializable][SerializeField]
    private struct FirePos
    {
        public Vector3 Up, Left, Right,Down;
    }

    private Animator _animator;
    private Transform _transform;
    private Transform _parentTransform;
    private Rigidbody2D rb;
    private SpriteRenderer _sprite;

    private bool isInWarpBlock;//ワープブロックに触れているかどうか
    public void SetIsInWarpBlock(bool _isInkWarpBlock)
    {
        isInWarpBlock = _isInkWarpBlock;
    }

    //きりたん砲関連
    private bool isFly;
    public bool IsFly()
    {
        return isFly;
    }
    private bool isFlyPrepare;
    private FlyEnum _flyEnum;
    public FlyEnum GetFlyEnum()
    {
        return _flyEnum;
    }
    private Vector3 flySpeed;
    public Vector3 GetFlySpeed()
    {
        return flySpeed;
    }
    private bool isSpeedMax;
    private float flySpeedMaxCount;

    //包丁投げ関連
    private bool isShot;
    private bool isShotPrepare;

    //ダメージ関連
    private bool isDamage;
    public bool IsDamage()
    {
        return isDamage;
    }

    //操作不能
    private bool canPlay=true;
    public void SetCanPlay(bool _canPlay)
    {
        canPlay = _canPlay;
    }


    public void Initialize()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        Fire.SetActive(false);
        isDamage = false;
        if (rb != null)
        {
            rb.simulated = true;
        }
        isInWarpBlock = false;
        canPlay = true;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = transform;
        _parentTransform = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        Initialize();
    }

    private void Update()
    {
        if (!GameManager.instance.IsPause())
        {
            if (!GameManager.instance.IsClear())
            {
                rb.simulated = true;
            }
            else
            {
                rb.simulated = false;
            }
            
            _animator.speed = 1;

            if (isDamage) return;
            if (GameManager.instance.IsClear()) return;
            if (!isShot && !isShotPrepare) { FlyDeal(); }
            if (!isFly && !isFlyPrepare) { ShotDeal(); }

            if (!isFly && !isFlyPrepare && !isShot && !isShotPrepare)
            {
                MoveDeal();
                AnimationDeal();
            }
        }
        else
        {
            rb.simulated = false;
            _animator.speed = 0;
        }
    }

    private void ShotDeal()
    {
        if (!isShot)
        {
            if (!isShotPrepare)
            {
                if (Input.GetButtonDown("Shot")&&!isInWarpBlock&&canPlay)
                {
                    _animator.Play("ShotPrepare");
                    isShotPrepare = true;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                bool isStart = false;
                Vector3 shotSpeed = Vector3.zero;
                Vector3 rotate = Vector3.zero;
                Vector3 pos = _transform.position;
                Sprite sprite = null;
                if (Input.GetButtonDown("Up")&&canPlay)
                {
                    isStart = true;
                    shotSpeed = new Vector3(0, 12, 0);
                    sprite = KiriImage.Shot3;
                    rotate = new Vector3(0, 0, 90);
                    pos += new Vector3(0.153f*_transform.localScale.x, 0.341f, 0);
                }else if (Input.GetButtonDown("Down")&&canPlay)
                {
                    isStart = true;
                    shotSpeed = new Vector3(0, -12, 0);
                    sprite = KiriImage.Shot4;
                    rotate = new Vector3(0, 0, -90);
                    pos += new Vector3(0.049f * _transform.localScale.x,-0.366f, 0);
                }
                else if (Input.GetButtonDown("Right")&&canPlay)
                {
                    isStart = true;
                    shotSpeed = new Vector3(12, 0, 0);
                    if (_transform.localScale.x == 1)
                    {
                        sprite = KiriImage.Shot1;
                        pos += new Vector3(0.364f, -0.079f, 0);
                    }
                    else
                    {
                        sprite = KiriImage.Shot2;
                        pos += new Vector3(0.17f, -0.07f, 0);
                    }
                    rotate = new Vector3(0, 0, 0);
                }
                else if (Input.GetButtonDown("Left")&&canPlay)
                {
                    isStart = true;
                    shotSpeed = new Vector3(-12, 0, 0);
                    if (_transform.localScale.x == 1)
                    {
                        sprite = KiriImage.Shot2;
                        pos += new Vector3(-0.17f, -0.07f, 0);
                    }
                    else
                    {
                        sprite = KiriImage.Shot1;
                        pos += new Vector3(-0.346f,-0.079f,0);
                    }
                    rotate = new Vector3(180, 0, 180);
                }
                else if (Input.GetButtonDown("Shot")&&canPlay)
                {
                    isShotPrepare = false;
                    FinishShot();
                }
                if (isStart)
                {
                    _animator.enabled = false;
                    _sprite.sprite = sprite;
                    isShot = true;
                    isShotPrepare = false;

                    Shot shot = Instantiate(ShotPrefab,GameScene.instance.StockOfTransform);
                    shot.Initialize(this,shotSpeed);
                    shot.transform.position = pos;
                    shot.transform.rotation = Quaternion.Euler(rotate);
                    SoundManager.PlaySE(SoundManager.SE_Type.OKeyVoice);
                }
            }
        }
        else
        {
            _animator.enabled = false;
        }
    }

    private void FlyDeal()
    {
        if (isFly)
        {
            if (isSpeedMax)
            {
                rb.velocity = flySpeed;
            }
            else
            {
                rb.velocity = flySpeed / 5;
                flySpeedMaxCount += Time.deltaTime;
                if (flySpeedMaxCount >= 0.1f)
                {
                    isSpeedMax = true;
                }
            }
        }
        else
        {
            if (!isFlyPrepare)
            {
                if (Input.GetButtonDown("Fly") && !isInWarpBlock&&canPlay)
                {
                    _animator.enabled = false;
                    isFlyPrepare = true;
                    _sprite.sprite = KiriImage.flyWait;

                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                bool isStart = false;
                if (Input.GetButtonDown("Up")&&canPlay)
                {
                    isStart = true;
                    flySpeed = new Vector3(0, 9, 0);
                    _sprite.sprite = KiriImage.flyVertical;
                    Fire.transform.localPosition = _FirePos.Up;
                    Fire.transform.rotation = Quaternion.Euler(180, 0, 0);
                    _flyEnum = FlyEnum.Vertical;
                }
                else if (Input.GetButtonDown("Down")&&canPlay)
                {
                    isStart = true;
                    flySpeed = new Vector3(0, -9, 0);
                    _sprite.sprite = KiriImage.flyVertical;
                    Fire.transform.localPosition = _FirePos.Down;
                    Fire.transform.rotation = Quaternion.Euler(0, 0, 0);
                    _flyEnum = FlyEnum.Vertical;
                    _transform.localScale = new Vector3(_transform.localScale.x, -1, 1);
                }
                else if (Input.GetButtonDown("Right")&&canPlay)
                {
                    isStart = true;
                    flySpeed = new Vector3(9, 0, 0);
                    _sprite.sprite = KiriImage.flyHorizontal;
                    _transform.localScale = new Vector3(-1, 1, 1);
                    Fire.transform.localPosition = _FirePos.Left;
                    Fire.transform.rotation = Quaternion.Euler(180, 0, 90);
                    _flyEnum = FlyEnum.Horizontal;
                }
                else if (Input.GetButtonDown("Left")&&canPlay)
                {
                    isStart = true;
                    flySpeed = new Vector3(-9, 0, 0);
                    _sprite.sprite = KiriImage.flyHorizontal;
                    _transform.localScale = new Vector3(1, 1, 1);
                    Fire.transform.localPosition = _FirePos.Right;
                    Fire.transform.rotation = Quaternion.Euler(180, 0, -90);
                    _flyEnum = FlyEnum.Horizontal;
                }else if (Input.GetButtonDown("Fly")&&canPlay)
                {
                    isFlyPrepare = false;
                    FinishFly();
                }

                if (isStart)
                {
                    isFly = true;
                    isFlyPrepare = false;
                    rb.gravityScale = 0;
                    transform.position += new Vector3(0, 0.05f, 0);
                    Fire.SetActive(true);
                    isSpeedMax = false;
                    flySpeedMaxCount = 0;
                    SoundManager.PlaySE(SoundManager.SE_Type.PKeyVoice);
                }
            }
        }
    }

    private void MoveDeal()
    {
        float vertical = Input.GetAxis("Horizontal");
        if (!canPlay) vertical = 0;
        Vector3 speedVector;
        if (vertical > 0)
        {
            //右移動
            speedVector = new Vector3(speed,rb.velocity.y, 0);
        }
        else if(vertical < 0)
        {
            //左移動
            speedVector = new Vector3(-speed,rb.velocity.y, 0);
        }
        else
        {
            speedVector = new Vector3(0, rb.velocity.y, 0);
        }

        if (_groundCheck.IsGround())
        {
            if (Input.GetButtonDown("Jump")&&canPlay)
            {
                speedVector = new Vector3(speedVector.x, jumpHeight, 0);
                SoundManager.PlaySE(SoundManager.SE_Type.JumpVoice1);
            }
        }
        rb.velocity = speedVector;
    }

    private void AnimationDeal()
    {
        if (rb.velocity.x != 0)
        {
            _animator.SetBool("IsWalk", false);
            if (rb.velocity.x > 0)
            {
                _transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                _transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            _animator.SetBool("IsWalk", true);
        }

        _animator.SetFloat("ySpeed", rb.velocity.y);
        _animator.SetBool("IsAir", !_groundCheck.IsGround());
    }

    public void FinishFly()
    {
        isFly = false;
        rb.gravityScale = 1;
        flySpeed = Vector3.zero;
        _animator.enabled = true;
        Fire.SetActive(false);
        _transform.localScale = new Vector3(_transform.localScale.x, 1, 1);
    }

    public void FinishShot()
    {
        isShot = false;
        _animator.enabled = true;
        _animator.Play("WaitAnimation");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            BlockChip block = collision.gameObject.GetComponent<BlockChip>();
            if (block!=null&&block.IsCanKill())
            {
                if (isDamage) return;
                isDamage = true;
                _animator.enabled = false;
                _sprite.sprite = KiriImage.Damage;
                GameManager.DieDeal();
                GameScene.DestroyStocks();
                float r = Random.value;
                if (r < 0.5f)
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.DamageVoice1);
                }
                else
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.DamageVoice2);
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Damage);
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    Instantiate(HitEffect,GameScene.instance.StockOfTransform).transform.position =contact.point;
                    break;
                }
            }
        }
    }

    public void ClearDeal(Vector3 clearPos)
    {
        if (clearPos.x - _transform.position.x>0)
        {
            _transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _transform.localScale = new Vector3(1, 1, 1);
        }
        _animator.Play("WalkAnimation");
        Hashtable move = new Hashtable();
        move.Add("position", clearPos + new Vector3(-0.18f,0, 0));
        move.Add("time", 0.4f);
        move.Add("easeType", "easeinCubic");
        move.Add("oncomplete", "ClearAnimationStart");
        move.Add("oncompletetrget",gameObject);
        iTween.MoveTo(gameObject,move);
        rb.simulated = false;
    }

    public void ClearAnimationStart()
    {
        int r = Random.Range(0, 2);
        if (r ==0)
        {
            SoundManager.PlaySE(SoundManager.SE_Type.ClearVoice1);
        }
        else
        {
            SoundManager.PlaySE(SoundManager.SE_Type.ClearVoice2);
        }
        StartCoroutine(ClearAnimationEnumerator());
    }

    private IEnumerator ClearAnimationEnumerator()
    {
        _animator.enabled = true;
        _transform.localScale = new Vector3(-1, 1, 1);
        _animator.Play("ClearAnimation");
        yield return new WaitForSeconds(2.0f);
    }
}
