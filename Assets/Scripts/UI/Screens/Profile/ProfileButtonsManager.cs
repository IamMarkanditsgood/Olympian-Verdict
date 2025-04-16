using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ProfileButtonsManager
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _playerEditorButton;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _rateButton;

    private Profile _profileScreen;
    private Home _home;
    private Info _infoScreen;
    private BasePopup _profileEditor;

    public void Init(Profile profile, Info info, Home home, BasePopup profileEditor)
    {
        _profileScreen = profile;
        _infoScreen = info;
        _home = home;
        _profileEditor = profileEditor;
    }

    public void Subscribe()
    {
        _closeButton.onClick.AddListener(Close);
        _playerEditorButton.onClick.AddListener(PlayerEditorOpen);
        _infoButton.onClick.AddListener(InfoOpen);
        _rateButton.onClick.AddListener(RateUs);
    }
    public void Unsubscribe() 
    {
        _closeButton.onClick.RemoveListener(Close);
        _playerEditorButton.onClick.RemoveListener(PlayerEditorOpen);
        _infoButton.onClick.RemoveListener(InfoOpen);
        _rateButton.onClick.RemoveListener(RateUs);
    }

    private void Close()
    {
        _profileScreen.Hide();
    }
    private void PlayerEditorOpen()
    {
        _profileEditor.Show();
    }
    private void InfoOpen()
    {
        _infoScreen.Show();
        _profileScreen.Hide();
    }
    public void RateUs()
    {
#if UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
#else
            Debug.Log("Rate Us functionality is only supported on iOS.");
#endif
    }
}
