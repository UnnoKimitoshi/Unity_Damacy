using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock_UI : MonoBehaviour
{
    // 時間経過に連れて消えていく画像
    [SerializeField] Image _fillImage;
    // 残り時間を表示するテキスト
    [SerializeField] TextMeshProUGUI _clockText;

    void Update()
    {
        UpdateImage();
        UpdataText();
    }

    void UpdateImage()
    {
        var rate = MainScene.Instance.gameTimerRemaining / (MainScene.Instance.gameTimerLength * 60);
        _fillImage.fillAmount = Mathf.Lerp(0, 1, rate);
    }

    void UpdataText()
    {
        // 「XX:XX」形式で表示
        float minutes, seconds;
        SecondsToMMSS(MainScene.Instance.gameTimerRemaining, out minutes, out seconds);
        var minutesText = minutes.ToString("00");
        var secondsText = string.Format("{0:00}", seconds);
        _clockText.text = $"{minutesText}:{secondsText}";
    }

    // 秒数を分と秒に変換
    void SecondsToMMSS(float remainingSeconds, out float minutes, out float seconds)
    {
        minutes = Mathf.FloorToInt(remainingSeconds / 60);
        seconds = Mathf.FloorToInt(remainingSeconds - ((float)minutes * 60));
    }
}
