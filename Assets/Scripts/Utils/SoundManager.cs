using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum BGM_Type
    {
        Title,StageSelect,Game
    }

    public enum SE_Type
    {
        CursleMouse,PressEnter1,PressEnter2,Clear,Damage,Explosion,KeyOpen,KnifeHit,KnifeNotHit,Explosion2,Warp,
        ClearVoice1,ClearVoice2,DamageVoice1,DamageVoice2,JumpVoice1,JumpVoice2,OKeyVoice,PKeyVoice,PauseVoice,ResponeVoice,
        RetryVoice,StartVoice,TitleVoice,Hue,Pa1,Pa2,GoBackVoice,StageChangeVoice,GoTitleVoice,Cancel,ClearVoice3,GoBackStageVoice,
        StageChangeVoice2
    }

    //BGM用フィールド
    [SerializeField] private float BGM_Volume = 0.1f;
    [SerializeField] private AudioClip[] IntroBGM_Clips;
    [SerializeField] private AudioClip[] LoopBGM_Clips;
    private AudioSource IntroBGM_Source;
    private AudioSource LoopBGM_Source;
    private int CurrentBGMIndex;


    //SE用フィールド
    [SerializeField] private float SE_Volume = 0.1f;
    [SerializeField] private AudioClip[] SE_Clips;
    //再生を遅延させるフレーム数
    [SerializeField] private int delayFrameCount = 2;
    //再生を予約できる最大数
    [SerializeField] private int maxQuendItemCount = 8;
    [SerializeField] private AudioSource defaultSource = default;

    public class _Info
    {
        public SE_Type SeType;
        public bool IsDone;//再生済みかどうかのフラグ
        public int FrameCount;//再生候補になってからの経過フレーム
        public AudioClip Clip;
    }
    private readonly Dictionary<SE_Type, Queue<_Info>> table = new Dictionary<SE_Type, Queue<_Info>>();
    public AudioMixerGroup AudioMixerGroup { set => this.defaultSource.outputAudioMixerGroup = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //ここあってる？
        IntroBGM_Source = gameObject.AddComponent<AudioSource>();
        LoopBGM_Source = gameObject.AddComponent<AudioSource>();
        IntroBGM_Source.loop = false;
        LoopBGM_Source.loop = true;

        if (!this.defaultSource)
        {
            var go = new GameObject("SEManager");
            go.transform.parent = this.transform;
            this.defaultSource = go.AddComponent<AudioSource>();
        }
    }

    public void Update()
    {
        //BGM
        if (IntroBGM_Source.clip != null)
        {
            if (!IntroBGM_Source.isPlaying)
            {
                LoopBGM_Source.Play();
                IntroBGM_Source.clip = null;
            }
        }

        //SE
        foreach (var q in this.table.Values)
        {
            if (q.Count == 0)
            {
                continue;
            }
            while (true)
            {
                if (q.Count == 0) break;
                if (q.Peek().IsDone)
                {
                    q.Dequeue();//
                }
                else
                {
                    break;
                }
            }
            if (q.Count == 0)
            {
                continue;
            }

            var info = q.Peek();
            info.FrameCount++;
            if (info.FrameCount > this.delayFrameCount)
            {
                defaultSource.volume = SE_Volume;
                this.defaultSource.PlayOneShot(info.Clip);
                q.Dequeue();
            }
        }

        if (this.count == 0)
        {
            this.table.Clear();
        }
    }

    public static void PlayBGM(BGM_Type bgmType)
    {
        instance.InstancePlayBGM(bgmType);
    }

    private void InstancePlayBGM(BGM_Type bgmType)
    {
        int index = (int)bgmType;
        CurrentBGMIndex = index;
        if (index < 0 || index >= IntroBGM_Clips.Length) return;
        if (IntroBGM_Source.clip != null && IntroBGM_Source.clip.Equals(IntroBGM_Clips[index])) return;
        if (LoopBGM_Source.clip != null && LoopBGM_Source.clip.Equals(LoopBGM_Clips[index])) return;

        IntroBGM_Source.clip = IntroBGM_Clips[index];
        LoopBGM_Source.clip = LoopBGM_Clips[index];
       
        IntroBGM_Source.volume = BGM_Volume;
        LoopBGM_Source.volume = BGM_Volume;
        IntroBGM_Source.Play();
    }

    public static void StopBGM()
    {
        instance.InstanceStopBGM();
    }

    private void InstanceStopBGM()
    {
        IntroBGM_Source.Stop();
        IntroBGM_Source.clip = null;
        LoopBGM_Source.Stop();
        LoopBGM_Source.clip = null;
    }

    public static void PlaySE(SE_Type seType)
    {
        instance.InstancePlaySE(seType);
    }

    private void InstancePlaySE(SE_Type seType)
    {
        int index = (int)seType;
        if (index < 0 || index >= SE_Clips.Length) return;
        AudioClip clip = SE_Clips[index];
        var info = new _Info() { FrameCount = 0, Clip = clip, };
        if (!this.table.ContainsKey(seType))
        {
            this.defaultSource.PlayOneShot(clip);
            info.IsDone = true;

            var q = new Queue<_Info>();
            q.Enqueue(info);
            this.table[seType] = q;
        }
        else
        {
            var list = this.table[seType];
            if (list.Count <= this.maxQuendItemCount)
            {
                this.table[seType].Enqueue(info);
            }
            else
            {
                Debug.Log($"効果音の最大登録数超過name={seType}");
            }
        }
    }

    public static void StopSE()
    {
        instance.InstanceStopSE();
    }

    private void InstanceStopSE()
    {
        defaultSource.Stop();
        table.Clear();
    }

    //有効な要素数の取得
    private int count
    {
        get
        {
            int num = 0;
            foreach (var list in this.table.Values)
            {
                num += list.Count;
            }
            return num;
        }
    }
}
