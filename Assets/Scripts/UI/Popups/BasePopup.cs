using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    public virtual void Show()
    {
        _view.SetActive(true);
    }
    public virtual void Hide()
    {
        _view.SetActive(false);
    }
}
