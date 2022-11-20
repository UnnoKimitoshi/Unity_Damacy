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
    Vector3 _defaultScale;
    // サイズに比例する重力
    Vector3 _defaultGravity = new Vector3(0, -9.8f, 0);
    Vector3 _gravity;
    //　ゲーム上での球のサイズ
    public float size = 1;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        transform.localScale = _defaultScale * size;
        _gravity = _defaultGravity * size;
    }

    private void FixedUpdate()
    {
        // X-Z平面上でカメラの前方向を正規化
        var cameraForward = Vector3.Scale(
            Camera.main.transform.forward,
            new Vector3(1, 0, 1)
            ).normalized;
        var moveDir = cameraForward * _vertical
            + Camera.main.transform.right * _horizontal;
        _rb.AddForce(moveDir * _speed + _gravity);
    }
}
