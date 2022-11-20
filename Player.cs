using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    protected override bool DontDestroy => false;
    Rigidbody _rb;
    // 移動の入力値
    float _horizontal, _vertical;
    [SerializeField] float _speed;
    float Speed
    {
        get
        {
            // 大きくなるに連れて遅くなる
            var rate = Mathf.Lerp(1, 0.5f, size / MainScene.Instance.targetSize);
            return _speed * size * rate;
        }
    }
    Vector3 _scale;
    public Vector3 Scale => _scale * size;
    Vector3 _gravity = new Vector3(0, -9.8f, 0);
    Vector3 gravity => _gravity * size;
    //　ゲーム上での球のサイズ
    public float size = 1;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _scale = transform.localScale;
    }

    private void Update()
    {
        if (MainScene.Instance.isPlaying)
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
        }
        transform.localScale = Scale;
    }

    private void FixedUpdate()
    {
        // カメラの前方＝プレイヤーの前方とした移動処理
        // X-Z平面上でカメラの前方向を正規化
        var cameraForward = Vector3.Scale(
            Camera.main.transform.forward,
            new Vector3(1, 0, 1)
            ).normalized;
        var moveDir = cameraForward * _vertical
            + Camera.main.transform.right * _horizontal;
        _rb.AddForce(moveDir * Speed);
        _rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
