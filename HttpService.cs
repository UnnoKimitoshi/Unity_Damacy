using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpService
{
    // サーバーへGETリクエストを送信
    public static IEnumerator Get<T>(string url, IDictionary<string, string> requestParams = null)
    {
        string requestUrl = url;
        // リクエストパラメーターがある場合はURLに結合
        if (requestParams != null)
        {
            requestUrl += "?";
            foreach (var requestParam in requestParams)
            {
                requestUrl += $"{requestParam.Key}={requestParam.Value}&";
            }
            // 最後の＆を削除
            requestUrl = requestUrl.Substring(0, requestUrl.Length - 1);
        }
        var request = UnityWebRequest.Get(requestUrl);
        // リクエスト送信
        yield return request.SendWebRequest();
        // エラー判定
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"通信に失敗しました。（{request.error}）");
            yield break;
        }
        yield return JsonUtility.FromJson<T>(request.downloadHandler.text);
    }

    // サーバーへPOSTリクエストを送信
    public static IEnumerator Post<T>(string url, IDictionary<string, string> requestParams = null)
    {
        var request = UnityWebRequest.Post(url, (Dictionary<string, string>)requestParams);
        // リクエスト送信
        yield return request.SendWebRequest();
        // エラー判定
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"通信に失敗しました。（{request.error}）");
            yield break;
        }
        yield return JsonUtility.FromJson<T>(request.downloadHandler.text);
    }
}
