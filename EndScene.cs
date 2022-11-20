using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    class ResponseCheckRankin
    {
        public bool rankin;
    }

    class ResponseRegistRecord
    {
        public bool result;
    }

    class ResponseRanking
    {
        List<Record> ranking;
    }

    class Record
    {
        public string name;
        public string score;
    }

    const string BASE_URL = "http://127.0.0.1:8000";
    [SerializeField] GameObject _registPanel;
    [SerializeField] GameObject _rankingPanel;


    void Start()
    {
        GetCheckRankin();
    }

    IEnumerator GetCheckRankin()
    {
        // ランクインしているか判定
        var httpget = HttpService.Get<ResponseCheckRankin>(
            BASE_URL + "/api/check_rankin",
            new Dictionary<string, string>(){
                {"score", Score.Instance.score.ToString()}
            }
        );
        yield return StartCoroutine(httpget);
        var responseCheckRankin = httpget.Current as ResponseCheckRankin;
        // 通信エラーの場合タイトルシーンを読み込む
        if (responseCheckRankin == null) SceneManager.LoadScene("Title");
        // ランクインしていない場合処理を戻す
        if (!responseCheckRankin.rankin) yield break;
        // ランクインしている場合
        _registPanel.SetActive(true);
        yield break;
    }

    IEnumerator RegistRecord()
    {
        var httpPost = HttpService.Get<ResponseRegistRecord>(
                   BASE_URL + "/api/regist",
                   new Dictionary<string, string>(){
                    {"name", "aaa"},
                     {"score", Score.Instance.score.ToString()}
                   }
               );
        yield return StartCoroutine(httpPost);
        var ResponseRegistRecord = httpPost.Current as ResponseRegistRecord;
        // 通信エラーまたは登録失敗時
        // 登録成功時
        // ランキング取得
        // ランキングパネル表示
    }

    IEnumerator ShowRanking()
    {
        _rankingPanel.SetActive(true);
        // 取得中のテキスト
        // ランキング取得
        var httpGet = HttpService.Get<ResponseRanking>(BASE_URL + "/api/get");
        yield return StartCoroutine(httpGet);
        var responseRanking = httpGet.Current as ResponseRanking;
        // エラー判定
        // ランキング情報をパネルに設定
    }



}
