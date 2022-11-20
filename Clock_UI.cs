using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock_UI : MonoBehaviour
{
    [SerializeField] Image _fillImage;
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
