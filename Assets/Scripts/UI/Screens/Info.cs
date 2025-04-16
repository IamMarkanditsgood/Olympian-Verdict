using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] private Profile _profile;
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _closeButton;
    private void Start()
    {
        _closeButton.onClick.AddListener(Hide);
    }
    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Hide);
    }
    public void Show()
    {
        _view.SetActive(true);
    }
    public void Hide()
    {
        _profile.Show();
        _view.SetActive(false);
    }
}
