using System;
using TMPro;
using UnityEngine;

// 球のサイズに関するUIを制御するクラス
public class Size_UI : MonoBehaviour
{
    // 球のサイズを表示するテキスト
    [SerializeField] TextMeshProUGUI _sizeText;
    // 画像の最大スケール
    float _maxImageScale;
    // 最大スケールの基準となるオブジェクトのtランスフォーム
    [SerializeField] Transform _targetScaleImage;
    // 球のサイズに比例して拡大する内側の画像
    [SerializeField] Transform _insideImage;
    // 内側の画像の回転速度
    [SerializeField] float _rotateSpeedInside = -1;
    // 球のサイズに比例して拡大する外の画像
    [SerializeField] Transform _outsideImage;
    // 外側の画像の回転速度
    [SerializeField] float _rotateSpeedOutside = -1;
    // 目標のサイズを表示するテキスト
    [SerializeField] TextMeshProUGUI _targetSize;
    float PlayerSize => Player.Instance.size;
    [SerializeField] float _scalingSpeed = 100;
    [SerializeField] float _scalingRate = 0.05f;
    float _angle = 0;
    Vector3 _imageScale;
    Vector3 ImageScale
    {
        get
        {
            var scale = Mathf.Lerp(1, _maxImageScale, Player.Instance.size / MainScene.Instance.targetSize);
            return _imageScale * scale;
        }
    }

    private void Start()
    {
        _imageScale = _insideImage.localScale;
        _targetSize.text = (MainScene.Instance.targetSize / 100).ToString() + "m";
        _maxImageScale = _targetScaleImage.localScale.x;
    }
    private void Update()
    {
        RotateAndScallingImage();
        UpdatePlayerSizeText();
    }

    private void RotateAndScallingImage()
    {
        // 画像を回転
        _insideImage.Rotate(0, 0, _rotateSpeedInside);
        _outsideImage.Rotate(0, 0, _rotateSpeedOutside);

        // 画像を拡縮
        _angle += Time.deltaTime * _scalingSpeed;
        _angle = Mathf.Repeat(_angle, 360);
        var rate = (float)Math.Sin(_angle * (Math.PI / 180)) * 0.05f + 1;
        _insideImage.localScale = ImageScale * rate;
        _outsideImage.localScale = ImageScale * rate;
    }

    private void UpdatePlayerSizeText()
    {
        // 1m未満 「XXcmXmm」
        if (PlayerSize < 100)
        {
            _sizeText.text = "<size=50>" + Math.Floor(PlayerSize).ToString("00") + "</size>cm"
                             + "<size=50>" + Math.Floor((PlayerSize % 1) * 10).ToString() + "</size>mm";
        }
        // 1m以上 「XXXmXXcm」
        else
        {
            _sizeText.text = "<size=50>" + Math.Floor(PlayerSize / 100).ToString("000") + "</size>m"
                            + "<size=50>" + Math.Floor(PlayerSize % 100).ToString("00") + "</size>cm";
        }
    }
}
