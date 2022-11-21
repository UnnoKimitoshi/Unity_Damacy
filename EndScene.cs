using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Endシーンの進行、処理を管理するクラス
public class EndScene : MonoBehaviour
{
    class ResponseCheckRankIn
    {
        public bool rankIn;
        public int rank;
    }

    class ResponseRegistRecord
    {
        public bool result;
    }
    [Serializable]
    class ResponseRanking
    {
        [Serializable]
        public class Record
        {
            public string name;
            public string score;
        }
        public List<Record> records;
    }



    const string BASE_URL = "http://www.kimitoshi.com";
    const string CONNECTIONG_TEXT = "サーバーと通信中です";
    const string CONNECTING_ERROR = "エラーが発生しました。<br>タイトルへ戻ります。";
    // スコアを表示するテキスト
    [SerializeField] TextMeshProUGUI _scoreText;
    // ランクイン取得中の情報を表示するテキスト
    [SerializeField] TextMeshProUGUI _getRankinInfoText;
    // 名前の入力欄
    [SerializeField] TMP_InputField _inputName;
    // レコード登録中の情報を表示するテキスト
    [SerializeField] TextMeshProUGUI _registInfoText;
    // レコード登録関係を表示するパネル
    [SerializeField] GameObject _registPanel;
    // ランキングを表示するパネル
    [SerializeField] GameObject _rankingPanel;
    // レコードの親
    [SerializeField] Transform _recordsParent;
    // レコード表示の雛形
    [SerializeField] GameObject _recordPanel;
    // ランキング取得中の情報を表示するテキスト
    [SerializeField] TextMeshProUGUI _getRankingInfoText;

    void Start()
    {
        Fade.Instance.FadeOut();
        SoundManager.Instance.PlayBgm(Bgm.End);
        _scoreText.text = Score.Instance.score.ToString();
        StartCoroutine(GetCheckRankin());
    }

    IEnumerator GetCheckRankin()
    {
        _getRankinInfoText.text = CONNECTIONG_TEXT;
        // ランクインしているか判定
        var httpget = HttpService.Get<ResponseCheckRankIn>(
            BASE_URL + "/api/check_rankin",
            new Dictionary<string, string>(){
                {"score", Score.Instance.score.ToString()}
            }
        );
        yield return StartCoroutine(httpget);
        var responseCheckRankIn = httpget.Current as ResponseCheckRankIn;
        // 通信エラーの場合タイトルシーンを読み込む
        if (responseCheckRankIn == null)
        {
            _getRankinInfoText.text = CONNECTING_ERROR;
            StartCoroutine(LoadTitle());
            yield break;
        }
        // ランクインしていないタイトルシーンを読み込む
        if (!responseCheckRankIn.rankIn)
        {
            _getRankinInfoText.text = "ランク圏外です。<br>タイトルへ戻ります。";
            LoadTitle();
            StartCoroutine(LoadTitle());
            yield break;

        }
        // ランクインしている場合
        else
        {
            _getRankinInfoText.text = $"第{responseCheckRankIn.rank}位にランクインしました。";
            _registPanel.SetActive(true);
            _registInfoText.text = "";
            yield break;
        }

    }

    public void Regist(Button button)
    {
        Debug.Log("ボタンが押されました");

        button.interactable = false;
        // 名前のバリデーションチェック (半角英数字ハイフンアンダースコアのみか)
        if (!Regex.IsMatch(_inputName.text, "^[a-zA-Z0-9-_]*$"))
        {
            _registInfoText.text = "名前は半角英数字のみ使用できます";
            button.enabled = true;
            return;
        }
        if (_inputName.text == null) _inputName.text = "guest";
        Debug.Log("登録開始");
        StartCoroutine(RegistRecord());
    }

    IEnumerator RegistRecord()
    {
        _registInfoText.text = CONNECTIONG_TEXT;
        var httpPost = HttpService.Post<ResponseRegistRecord>(
                   BASE_URL + "/api/regist",
                   new Dictionary<string, string>(){
                    {"name", _inputName.text},
                     {"score", Score.Instance.score.ToString()}
                   }
               );
        yield return StartCoroutine(httpPost);
        var ResponseRegistRecord = httpPost.Current as ResponseRegistRecord;
        // 通信エラーまたは登録失敗時はTitleシーンを読み込む
        if (ResponseRegistRecord == null || !ResponseRegistRecord.result)
        {
            _getRankinInfoText.text = CONNECTING_ERROR;
            StartCoroutine(LoadTitle());
            yield break;
        }
        // 登録成功時はランキングパネル表示
        else
        {
            _registInfoText.text = "登録が完了しました。";
            yield return new WaitForSeconds(2);
            StartCoroutine(ShowRanking());
        }

    }

    IEnumerator ShowRanking()
    {
        _rankingPanel.SetActive(true);
        // 取得中のテキスト
        _getRankingInfoText.text = CONNECTIONG_TEXT;
        // ランキング取得
        var httpGet = HttpService.Get<ResponseRanking>(BASE_URL + "/api/get");
        yield return StartCoroutine(httpGet);
        var responseRanking = httpGet.Current as ResponseRanking;

        // エラー判定
        if (responseRanking == null)
        {
            _getRankingInfoText.text = CONNECTING_ERROR;
            StartCoroutine(LoadTitle());
            yield break;
        }
        _getRankingInfoText.text = "";
        // レコード情報をパネルに設定
        var rank = 1;
        foreach (var record in responseRanking.records)
        {
            GameObject recordPanel = Instantiate(_recordPanel, Vector3.zero, Quaternion.identity, _recordsParent);
            recordPanel.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rank.ToString();
            recordPanel.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = record.name;
            recordPanel.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = record.score;
            rank++;
        }
    }

    public void OnPushedTitleButton(Button button)
    {
        button.interactable = false;
        StartCoroutine(LoadTitle());
    }

    public IEnumerator LoadTitle()
    {
        yield return new WaitForSeconds(2);
        SoundManager.Instance.StopBgm();
        Fade.Instance.FadeIn(() => SceneManager.LoadScene("Title"));
    }
}
