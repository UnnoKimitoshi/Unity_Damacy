using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene : Singleton<MainScene>
{
    protected override bool DontDestroy => false;
    Fade _fade;
    SoundManager _soundManager;
    // 開始時のカウントダウン用タイマー
    // ...2->1->Startと表示したいので表示したい最大値＋1の値を設定
    int _startTimerLength = 4;
    float _startTimerRemaining;
    [SerializeField] TextMeshProUGUI _startTimerText;
    // ゲームの制限時間のタイマー
    [Range(1, 5)] public int gameTimerLength = 3;
    public float gameTimerRemaining;
    bool _countStartTimer = false;
    public bool isPlaying = false;

    void Start()
    {
        _startTimerRemaining = _startTimerLength;
        gameTimerRemaining = gameTimerLength;
        _fade = Fade.Instance;
        _soundManager = SoundManager.Instance;
        _fade.FadeOut(() =>
        {
            _countStartTimer = true;
        });
    }

    void Update()
    {
        CountStartTimer();
        CountGameTimer();
    }

    void CountStartTimer()
    {
        if (!_countStartTimer) return;
        _startTimerRemaining -= Time.deltaTime;

        if (_startTimerRemaining > 1)
        {
            var newValue = ((int)_startTimerRemaining).ToString();
            if (_startTimerText.text != newValue)
            {
                _startTimerText.text = ((int)_startTimerRemaining).ToString();
                _soundManager.PlaySE(SE.Countdown);
            }
        }
        else if (_startTimerLength >= 0)
        {
            _startTimerText.text = "スタート";
            isPlaying = true;
        }
        else
        {
            _startTimerText.text = "";
            _countStartTimer = false;
        }
    }

    void CountGameTimer()
    {
        if (!isPlaying) return;
        gameTimerRemaining -= Time.deltaTime;
        if (gameTimerRemaining <= 0)
        {
            gameTimerRemaining = 0;
            isPlaying = false;
        }
    }
}
