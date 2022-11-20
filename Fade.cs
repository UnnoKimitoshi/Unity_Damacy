using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

enum fadeStatus
{
    none,
    fadeIn,
    fadeOut,
}

public class Fade : Singleton<Fade>
{
    protected override bool DontDestroy => true;
    const int ALPHA_MAX = 1;
    const int ALPHA_MIN = 0;
    [SerializeField] Image _fadeImage;
    fadeStatus _fadeStatus = fadeStatus.none;
    float _alpha = 255;
    [SerializeField] float _fadeSpeed = 100f;
    UnityAction OnFaded;

    void Update()
    {
        switch (_fadeStatus)
        {
            case fadeStatus.fadeIn:
                _alpha += Time.deltaTime * _fadeSpeed;
                if (_alpha >= ALPHA_MAX)
                {
                    _alpha = ALPHA_MAX;
                    ReturnFadeStatus();
                }
                _fadeImage.color = new Color(0f, 0f, 0f, _alpha);
                break;
            case fadeStatus.fadeOut:
                _alpha -= Time.deltaTime * _fadeSpeed;
                if (_alpha <= ALPHA_MIN)
                {
                    _alpha = ALPHA_MIN;
                    _fadeImage.gameObject.SetActive(false);
                    ReturnFadeStatus();
                }
                _fadeImage.color = new Color(0f, 0f, 0f, _alpha);
                break;
        }
    }

    public void ReturnFadeStatus()
    {
        _fadeStatus = fadeStatus.none;
        OnFaded?.Invoke();
        OnFaded = null;
    }

    public void FadeIn(UnityAction action = null)
    {
        OnFaded += action;
        SoundManager.Instance.StopBgm();
        _fadeImage.gameObject.SetActive(true);
        _alpha = ALPHA_MIN;
        _fadeStatus = fadeStatus.fadeIn;
    }

    public void FadeOut(UnityAction action = null)
    {
        OnFaded += action;
        _fadeImage.gameObject.SetActive(true);
        _alpha = ALPHA_MAX;
        _fadeStatus = fadeStatus.fadeOut;
    }

}
