using UnityEngine;
using UnityEngine.SceneManagement;

// Titleシーンの進行、処理を管理するクラス
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
        SoundManager.Instance.PlaySE(SE.StartButton);
        _fade.FadeIn(() =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}
