using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StageEnum;

public class SoundManager : SequenceController {
    public static SoundManager Instance = null;

    class OverlapTime
    {
        public int soundID;
        public float curTime;
    }

    Dictionary<int, AudioClip> kEffectClips = new Dictionary<int, AudioClip>();
    Dictionary<int, AudioClip> kVoiceClips = new Dictionary<int, AudioClip>();
    Dictionary<int, AudioClip> kBGMClips = new Dictionary<int, AudioClip>();
    
    AudioSource mVoiceSource;
    AudioSource mBGMSource;
    AudioSource mEffectSource;

    List<AudioSource> mAudioSourcePoolList = new List<AudioSource>();
    
    [HideInInspector]
    public bool kIsSystemOn = true;

    List<OverlapTime> mOverlapTimeList = new List<OverlapTime>();

    void Awake()
    {
        Instance = this;

        mVoiceSource = AddSource("Voice");
        mVoiceSource.gameObject.AddComponent<AudioHighPassFilter>().cutoffFrequency = 500.0f;

        mBGMSource = AddSource("BGM");        
        mEffectSource = AddSource("Effect");

        BGMVolume(EditDef.SOUND_BGM_DEFAULT_VOLUME);
        EffectVolume(EditDef.SOUND_EFFECT_DEFAULT_VOLUME);

        for (int i = 0; i < 10; i++)
        {
            AudioSource audioSource = AddSource("AudioSource");
            mAudioSourcePoolList.Add(audioSource);
        }

        DontDestroyOnLoad(gameObject);
    }
        
    // Use this for initialization
    public override void OnStart () {
       
    }

    public void BGMVolume(float _volume)
    {
        mBGMSource.volume = _volume;
    }

    public void VoiceVolume(float _volume)
    {
        mVoiceSource.volume = _volume;
    }

    public void EffectVolume(float _volume)
    {
        mEffectSource.volume = _volume;
    }

    string ToSoundFileName(int _tableID)
    {        
        DT_SoundData_Info info = CDT_SoundData_Manager.Instance.GetInfo(_tableID);
        if (info == null)
            return "";
        
        return info.Name;
    }

    public void PlayBGM(int _tableID)
    {
        if (kIsSystemOn == false)
            return;
    }


    public void PlayBGM(string _name)
    {
        if (kIsSystemOn == false)
            return;

        if (mBGMSource.isPlaying == true && mBGMSource.clip.name.CompareTo(_name) == 0)
            return;
        
        AudioClip bgm = Resources.Load(StrDef.PATH_SOUND_BGM + _name) as AudioClip;
        mBGMSource.loop = true;
        mBGMSource.clip = bgm;
        mBGMSource.Play();

        /*
        AudioClip clip = null;
        if (kBGMClips.ContainsKey(_tableID) == true)
        {
            clip = kBGMClips[_tableID];
        }
        else
        {
            string name = ToSoundFileName(_tableID);
            Object obj = AssetManager.Instance.GetObject(StrDef.PATH_SOUND_BGM + name, ".mp3");
            if (obj == null)
                return;
            clip = obj as AudioClip;
            kBGMClips.Add(_tableID, clip);
        }
        
        mBGMSource.clip = clip;
        mBGMSource.loop = true;
        mBGMSource.Play();
        */
    }

    public void StopBGM()
    {
        mBGMSource.Stop();
    }

    public void PlayVoice(string _name)
    {
        AudioClip voice = Resources.Load(StrDef.PATH_SOUND_VOICE + _name) as AudioClip;
        mVoiceSource.PlayOneShot(voice);
    }

    public void PlayVoice(int _tableID)
    {
        if (kIsSystemOn == false)
            return;
        /*
        AudioClip clip = null;
        if (kVoiceClips.ContainsKey(_tableID) == true)
        {
            clip = kVoiceClips[_tableID];
        }
        else
        {
            string name = ToSoundFileName(_tableID);
            Object obj = AssetManager.Instance.GetObject(StrDef.PATH_SOUND_VOICE + name, ".mp3");
            if (obj == null)
                return;
            clip = obj as AudioClip;
            kVoiceClips.Add(_tableID, clip);
        }

        mVoiceSource.PlayOneShot(clip);
        */
    }
    
    public void PlayEffect(int _tableID)
    {
        if (kIsSystemOn == false)
            return;
        
        AudioClip clip = null;
        
        if (kEffectClips.ContainsKey(_tableID) == true)
        {
            for(int i = 0; i < mOverlapTimeList.Count; i++)
                if (mOverlapTimeList[i].soundID == _tableID)
                    return;

            clip = kEffectClips[_tableID];            
        }
        else
        {
            string name = ToSoundFileName(_tableID);
            Object obj = Resources.Load(StrDef.PATH_SOUND_EFFECT + name); //ObjectPoolManager.Instance.GetGameObejct(StrDef.PATH_SOUND_EFFECT + name);
            if (obj == null )
                return;
            clip = obj as AudioClip;
            kEffectClips.Add(_tableID, clip);
        }

        OverlapTime info = new OverlapTime();
        info.soundID = _tableID;
        info.curTime = 0.0f;
        mOverlapTimeList.Add(info);

        mEffectSource.PlayOneShot(clip);        
    }
    
    public override void OnUpdate()
    {
        EffectOverlapTimeUpdate();
    }

    void EffectOverlapTimeUpdate()
    {
        for (int i = 0; i < mOverlapTimeList.Count; i++)
        {
            mOverlapTimeList[i].curTime += Time.deltaTime;
            if (mOverlapTimeList[i].curTime > StageDef.SOUND_EFFECT_MIN_OVERTIME)
            {
                mOverlapTimeList.RemoveAt(i);
                i--;
            }
        }
    }

    public AudioSource AddSource(string _sourceName)
    {
        GameObject obj = new GameObject(_sourceName);
        obj.transform.parent = transform;
        obj.name = _sourceName;
        AudioSource source = obj.AddComponent<AudioSource>();
        return source;
    }
    
    public void Play(AudioSource _audio, int _soundID, bool _loop = false)
    {
        if (kIsSystemOn == false)
            return;
        /*
        if (_audio.isPlaying == false)
        {
            AudioClip clip = null;
            if (kEffectClips.ContainsKey(_soundID) == true)
                clip = kEffectClips[_soundID];
            else
            {
                string name = ToSoundFileName(_soundID);
                Object obj = AssetManager.Instance.GetObject(StrDef.PATH_SOUND_EFFECT + name, ".mp3");
                if (obj == null)
                    return;

                clip = obj as AudioClip;
                kEffectClips.Add(_soundID, clip);
            }

            if (_loop == false)
            {
                _audio.PlayOneShot(clip);
            }
            else
            {
                _audio.clip = clip;
                _audio.loop = true;
                _audio.Play();
            }
        }
        */
    }

    public AudioSource GetAudioSourcePool()
    {   
        if (mAudioSourcePoolList.Count == 0)
        {
            AudioSource addSource = AddSource("AudioSource");
            return addSource;
        }
        else
        {
            AudioSource source = mAudioSourcePoolList[0];
            mAudioSourcePoolList.RemoveAt(0);
            return source;
        }
    }        

    public void SetAudioSourcePool(AudioSource _source)
    {
        _source.Stop();
        mAudioSourcePoolList.Add(_source);
    }
    
    public void Clear()
    {
        for (int i = 0; i < mAudioSourcePoolList.Count; i++)
            mAudioSourcePoolList[i].Stop();

        if (mVoiceSource.isPlaying == true)
            mVoiceSource.Stop();

        if (mBGMSource.isPlaying == true)
            mBGMSource.Stop();

        if (mEffectSource.isPlaying == true)
            mEffectSource.Stop();

        kBGMClips.Clear();
        kVoiceClips.Clear();
        kEffectClips.Clear();
    }

    string[] mEnemyDestroyVoices = { "enemy down", "target down", "your target destroyed"};
    string[] mEnemyFindVoices = { "enemies incoming", "enemy detected", "engaging the enemy" };
    string[] mOrderVoices = {};
    string[] mPlayerDestroy = { "we are under attack", "engine damaged", "shields down", "your ship destroyed"};

    public void BattleVoice(BattleSign _sign)
    {
        if (mVoiceSource.isPlaying == true)
            return;

        string name = "";
        switch(_sign)
        {
            case BattleSign.EnemyDestroy:
                {
                    int selectIndex = Random.Range(0, mEnemyDestroyVoices.Length);
                    name = mEnemyDestroyVoices[selectIndex];
                }
                break;
            case BattleSign.EnemyFind:
                {
                    int selectIndex = Random.Range(0, mEnemyFindVoices.Length);
                    name = mEnemyFindVoices[selectIndex];
                }
                break;
            case BattleSign.Order:
                {

                }
                break;
            case BattleSign.PlayerDestory:
                {
                    int selectIndex = Random.Range(0, mPlayerDestroy.Length);
                    name = mPlayerDestroy[selectIndex];
                }
                break;
        }

        PlayVoice(name);
    }
}