using UnityEngine;

// カメラの回転の軸となる親を作りそれにアタッチ
[ExecuteInEditMode]
public class TrackingCamera : MonoBehaviour
{
    Transform _trackinTarget;
    [SerializeField] float _trackingSpeed;
    [SerializeField] Vector3 _angle = new Vector3();
    [SerializeField] float _mouseSensivity = 4;
    [SerializeField] Transform _camera;
    [SerializeField] Vector3 _defaultCameraOffset = new Vector3(0, 0, 0);
    Vector3 _cameraOffset => _defaultCameraOffset * Player.Instance.size;

    void Awake()
    {
        _camera = Camera.main.transform;
        _trackinTarget = Player.Instance.transform;
    }

    void Update()
    {
        // マウスで回転(カメラが移動)
        var inputAngle = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * _mouseSensivity;
        _angle += inputAngle;
        // X軸方向の回転に制限をかける
        _angle.x = Mathf.Clamp(_angle.x, 0, 70);
    }

    private void LateUpdate()
    {
        // カメラは遅れて移動する
        var position = Vector3.Lerp(
            transform.position,
            _trackinTarget.position,
            _trackingSpeed * Time.deltaTime
        );

        // 算出した値を適応
        transform.position = position;
        transform.eulerAngles = _angle;

        // 子ども（カメラ）の場所を設定
        var cameraPos = _camera.localPosition;
        cameraPos.y = _cameraOffset.y;
        cameraPos.z = _cameraOffset.z;
        _camera.localPosition = cameraPos;
    }
}
