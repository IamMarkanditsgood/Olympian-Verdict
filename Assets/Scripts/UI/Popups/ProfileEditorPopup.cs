using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileEditorPopup : BasePopup
{
    [SerializeField] private AvatarManager _avatarManager;

    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _close;
    [SerializeField] private Button _save;
    [SerializeField] private Button _avatar;

    [SerializeField] private TMP_Text _nameProfile;
    [SerializeField] private TMP_Text _nameHome;
    [SerializeField] private TMP_InputField _editorName;

    [SerializeField] private Sprite _offSounds;
    [SerializeField] private Sprite _onSounds;
    [SerializeField] private GameObject _music;
    [SerializeField] private AudioSource _sounds;

    private void Start()
    {
        _close.onClick.AddListener(Close);
        _save.onClick.AddListener(Save);
        _avatar.onClick.AddListener(Avatar);
        _soundButton.onClick.AddListener(SoundButton);
    }
    private void OnDestroy()
    {
        _close.onClick.RemoveListener(Close);
        _save.onClick.RemoveListener(Save);
        _avatar.onClick.RemoveListener(Avatar);
        _soundButton.onClick.RemoveListener(SoundButton);
    }
    public override void Show()
    {
        SetPopup();
        base.Show();
    }

    private void Close()
    {
        _avatarManager.SetSavedPicture();
        Hide();
    }
    private void Save()
    {
        PlayerPrefs.SetString("Name", _editorName.text);

        _nameProfile.text = _editorName.text;
        _nameHome.text = _editorName.text;

        _avatarManager.SavePictures();
    }

    private void SetPopup()
    {
        if(PlayerPrefs.GetInt("Audio") == 1)
        {
            _soundButton.gameObject.GetComponent<Image>().sprite = _onSounds;
        }
        else
        {
            _soundButton.gameObject.GetComponent<Image>().sprite = _offSounds;
        }
    }

    private void Avatar()
    {
        _avatarManager.TakePicture();
    }
    private void SoundButton()
    {
        if(PlayerPrefs.GetInt("Audio") == 1)
        {
            Debug.Log("Off");
            PlayerPrefs.SetInt("Audio", 0);
            _soundButton.gameObject.GetComponent<Image>().sprite = _offSounds;
            _music.SetActive(false);
            _sounds.Stop();
        }
        else
        {
            Debug.Log("On");
            PlayerPrefs.SetInt("Audio", 1);
            _soundButton.gameObject.GetComponent<Image>().sprite = _onSounds;
            _music.SetActive(true);
        }
       
    }
}
