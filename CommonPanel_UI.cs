using UnityEngine;

// 共通UIに関する処理
public class CommonPanel_UI : MonoBehaviour
{
    [SerializeField] GameObject _settingPanel;

    public void OpeanSettingPanel()
    {
        _settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        _settingPanel.SetActive(false);
    }

}
