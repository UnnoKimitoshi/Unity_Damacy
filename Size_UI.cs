using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Size_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _sizeText;

    private void Update()
    {
        var size = Player.Instance.size;
        // 1m未満 XXcmXmm
        if (size < 100)
        {
            _sizeText.text = "<size=50>" + Math.Floor(size).ToString("00") + "</size>cm"
                             + "<size=50>" + Math.Floor((size % 1) * 10).ToString() + "</size>mm";
        }
        // 以上 XXXmXXcm
        else
        {
            _sizeText.text = "<size=50>" + Math.Floor(size / 100).ToString("000") + "</size>m"
                            + "<size=50>" + Math.Floor(size % 100).ToString("00") + "</size>cm";
        }
    }
}
