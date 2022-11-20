using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球にくっつくオブジェクトクラス
public class AttachableObject : MonoBehaviour
{
    Player _player;
    [SerializeField] float size;
    [SerializeField] SE _se;
    Collider _collider;
    Vector3 _defaultScale;
    // 球にくっついたかどうかのフラグ
    bool _isAttaced = false;

    private void Start()
    {
        _player = Player.Instance;
        _collider = GetComponent<Collider>();
        _defaultScale = transform.lossyScale;
    }
    private void Update()
    {
        // くっついた場合球のスケールに影響されるので補正をスケールを維持
        if (_isAttaced)
        {
            transform.localScale = _defaultScale / _player.Scale.x;
        }
        else
        {
            // 球のサイズが自身のサイズ以上になったらくっつく準備
            if (_player.size < size) return;
            _collider.isTrigger = true;
        }
    }

    // 球が接触したときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (!MainScene.Instance.isPlaying) return;
        // 球が接触したとき条件を満たしていれば球の子になる
        if (other.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlaySE(_se);
            _isAttaced = true;
            _collider.enabled = false;
            // そのまま足すと大きくなりすぎるので補正値をかける
            _player.size += size * 0.05f;
            gameObject.transform.parent = other.transform;
        }
    }
}
