using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bgm
{
    Title, Main, End
}

public enum SE
{
    normal, StartButton, Countdown, GameOver,
    Cow, Pig, Sheep, Chicken, Duck,

}

public class SoundManager : Singleton<SoundManager>
{
    // 楽曲データの基礎クラス
    public class SoundData
    {
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
    }

    [Serializable]
    public class BgmData : SoundData
    {
        public Bgm bgm;
    }

    [Serializable]
    public class SEData : SoundData
    {
        public SE se;
        // 最後に再生した時間
        public float playdTime;
    }

    protected override bool DontDestroy => true;
    [SerializeField] List<BgmData> _bgmData;
    [SerializeField] List<SEData> _seDatas;
    // Bgm用のAudioSource
    AudioSource _bgmAudioSource;
    // SE用のAudioSource 同時に鳴らしたいSEの種類の数だけ用意
    AudioSource[] _seAudioSources = new AudioSource[5];
    // 上のサブ
    AudioSource _subSEAudioSource = new AudioSource();
    [SerializeField] float _bgmMasterVolume = 0.5f;
    [SerializeField] float _seMasterVolume = 0.5f;
    // 同じSEを再び再生できるようになるまでの間隔
    [SerializeField] float _playTimeDistance = 0.01f;

    new void Awake()
    {
        base.Awake();
        // Bgm用のAudioSourceを作成
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        // SE用のAudioSourceを作成
        for (var i = 0; i < _seAudioSources.Length; ++i)
        {
            _seAudioSources[i] = gameObject.AddComponent<AudioSource>();
        }
        _subSEAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBgm(Bgm bgm)
    {
        var data = _bgmData.Find(bgmData => bgmData.bgm == bgm);
        if (data == null)
        {
            Debug.Log($"{bgm}は登録されていません");
            return;
        }
        _bgmAudioSource.clip = data.audioClip;
        _bgmAudioSource.volume = data.volume * _bgmMasterVolume;
        _bgmAudioSource.Play();
    }

    public void StopBgm()
    {
        _bgmAudioSource.Stop();
    }

    public void PlaySE(SE se = SE.normal, bool sub = false)
    {
        // 未使用のAudioSourceを取得
        var audioSource = GetUnusedSEAudioSource();
        if (sub) audioSource = _subSEAudioSource;
        if (audioSource == null) return; //再生できませんでした
        var data = _seDatas.Find(seData => seData.se == se);
        if (data == null)
        {
            Debug.Log($"{se}は登録されていません");
            return;
        }
        // 前回再生した時間から十分経過しているか検証
        if (Time.realtimeSinceStartup - data.playdTime < _playTimeDistance) return;
        audioSource.clip = data.audioClip;
        audioSource.volume = data.volume * _seMasterVolume;
        audioSource.Play();
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedSEAudioSource()
    {
        for (var i = 0; i < _seAudioSources.Length; ++i)
        {
            if (_seAudioSources[i].isPlaying == false) return _seAudioSources[i];
        }
        return null;
    }

    public void ChangeBgmMasterVolume(float value)
    {
        _bgmMasterVolume = value;
        var clip = _bgmAudioSource.clip;
        var bgmVolume = _bgmData.Find(bgmData => bgmData.audioClip == _bgmAudioSource.clip).volume;
        _bgmAudioSource.volume = bgmVolume * _bgmMasterVolume;
    }

    public void ChangeSEMasterVolume(float value)
    {
        _seMasterVolume = value;
    }
}
