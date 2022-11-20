using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    Fade _fade;
    void Awake()
    {
        _fade = Fade.Instance;
        _fade.FadeOut();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm(Bgm.Title);
    }

    public void OnStartButtonPushed()
    {
        _fade.FadeIn(() =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}
