using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : BasePopup
{
    [SerializeField] private Button _close;
    [SerializeField] private Button _thanks;

    private void Start()
    {
        _close.onClick.AddListener(Hide);
        _thanks.onClick.AddListener(Hide);
    }
    private void OnDestroy()
    {
        _close.onClick.RemoveListener(Hide);
        _thanks.onClick.RemoveListener(Hide);
    }
    public override void Show()
    {
        ResourceManager.Instance.UpdateScore(50);
        base.Show();
    }

}
