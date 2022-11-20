using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UnitychanController : MonoBehaviour
{
    Animator _animator;
    Transform _player;
    [SerializeField] float _heighetOffset = 0.5f;
    [SerializeField] float _distance;
    [SerializeField] private Transform _cameraParent;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = Player.Instance.transform;
    }

    void Update()
    {
        var inputVertical = Input.GetAxisRaw("Vertical");
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (inputVertical != 0)
        {
            _animator.SetInteger("input", (int)inputVertical);
        }
        else if (inputHorizontal != 0)
        {
            _animator.SetInteger("input", 1);
        }
        else
        {
            _animator.SetInteger("input", 0);
        }

    }
    void LateUpdate()
    {

        // X-Z平面上で「球->カメラ」を正規化
        var playerToCamera = Vector3.Scale(
            Camera.main.transform.position - _player.position,
            new Vector3(1, 0, 1)
            ).normalized;
        var heightVec = new Vector3(0, _heighetOffset, 0);
        var offset = playerToCamera * _distance + heightVec;
        transform.position = _player.position + offset * Player.Instance.size;
        transform.localEulerAngles = new Vector3(0, _cameraParent.localEulerAngles.y, 0);
    }

}
