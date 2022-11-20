using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Mainシーンの進行、処理を管理するクラス
public class MainScene : Singleton<MainScene>
{
    protected override bool DontDestroy => false;
    // プレイヤーの目標となるサイズ
    public int targetSize = 500;
    Fade _fade;
    SoundManager _soundManager;

    // 開始時のカウントダウン用タイマー
    [SerializeField] TextMeshProUGUI _startTimerText;
    // 初期値（秒）
    // ...2->1->Startと表示したいので表示したい最大値＋1の値を設定
    int _startTimerLength = 4;
    // 残り時間（秒）
    float _startTimerRemaining;

    // ゲームの制限時間のタイマー
    // 初期値（分）
    [Range(1, 5)] public int gameTimerLength = 3;
    // 残り時間（秒）
    public float gameTimerRemaining;

    // フラグ
    // スタートタイマーカウントの可否
    bool _countStartTimer = false;
    // プレイ中の可否
    public bool isPlaying = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        _startTimerRemaining = _startTimerLength;
        gameTimerRemaining = gameTimerLength * 60;
        _fade = Fade.Instance;
        _soundManager = SoundManager.Instance;
        // フェードアウトしたらスタートタイマーカウント開始を登録
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
                _soundManager.PlaySE(SE.Countdown);
                _startTimerText.text = ((int)_startTimerRemaining).ToString();
            }
        }
        else if (_startTimerRemaining > 0)
        {
            _soundManager.PlayBgm(Bgm.Main);
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
        // 目標達成した場合Endシーンを読み込む
        if (Player.Instance.size >= targetSize)
        {
            isPlaying = false;
            Score.Instance.score = (int)gameTimerRemaining;
            _startTimerText.text = "目標達成";
            StartCoroutine(LoadScene("End"));
        }
        // タイムアップの場合Titleシーンに戻る
        if (gameTimerRemaining <= 0)
        {
            isPlaying = false;
            gameTimerRemaining = 0;
            _startTimerText.text = "時間切れ";
            StartCoroutine(LoadScene("Title"));
        }
    }

    IEnumerator LoadScene(string seneName)
    {
        _soundManager.StopBgm();
        _soundManager.PlaySE(SE.GameOver, true);
        isPlaying = false;
        yield return new WaitForSeconds(2);
        Fade.Instance.FadeIn(() => SceneManager.LoadScene(seneName));
    }

    public void Restart()
    {
        _soundManager.StopBgm();
        isPlaying = false;
        Fade.Instance.FadeIn(() => SceneManager.LoadScene("Main"));
    }
}
