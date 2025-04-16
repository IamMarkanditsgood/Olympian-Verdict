using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private ProfileButtonsManager _profileButtonsManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private PlayerAchievements _playerAchievements;

    [SerializeField] private Home _home;
    [SerializeField] private Info _infoScreen;
    [SerializeField] private BasePopup _profileEditor;

    [SerializeField] private Button[] _achievemntButtons;
    [SerializeField] private Sprite[] _achievementSprites;
    [SerializeField] private GameObject _viewPopup;
    [SerializeField] private Image _achieve;
    [SerializeField] private Button[] _closePopup;

    [SerializeField] private GameObject _view;
    public void Start()
    {
        _profileButtonsManager.Init(this, _infoScreen,_home, _profileEditor);
        _profileButtonsManager.Subscribe();

        foreach (var button in _closePopup)
        {
            button.onClick.AddListener(ClosePopup);
        }

        for (int i = 0; i < _achievemntButtons.Length; i++)
        {
            int index = i;
            _achievemntButtons[index].onClick.AddListener(() => OpenPopup(index));
        }

    }
    private void OnDestroy()
    {
        _profileButtonsManager.Unsubscribe();

        foreach (var button in _closePopup)
        {
            button.onClick.RemoveListener(ClosePopup);
        }

        for (int i = 0; i < _achievemntButtons.Length; i++)
        {
            int index = i;
            _achievemntButtons[index].onClick.RemoveListener(() => OpenPopup(index));
        }
    }

    public void Show()
    {
        _playerManager.SetPlayer();
        _playerAchievements.SetAchievements();
        _view.SetActive(true);
    }
    public void Hide()
    {
        _view.SetActive(false);
    }

    private void OpenPopup(int index)
    {
        _viewPopup.SetActive(true);
        _achieve.sprite = _achievementSprites[index];
    }
    private void ClosePopup()
    {
        _viewPopup.SetActive(false);
    }
}
