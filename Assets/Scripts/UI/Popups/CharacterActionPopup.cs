using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPopup : BasePopup
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _positiveButton;
    [SerializeField] private Button _negativeButton;

    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private TMP_Text _storyName;
    [SerializeField] private TMP_Text _storyText;

    [SerializeField] private GemMover gemMover;



    private CharacterConfig _characterConfig;
    private int _currentStory = 0;
    private bool _lastStory;

    private bool _canPress = true;

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
        _positiveButton.onClick.AddListener(Positive);
        _negativeButton.onClick.AddListener(Negative);  
    }
    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Close);
        _positiveButton.onClick.RemoveListener(Positive);
        _negativeButton.onClick.RemoveListener(Negative);
    }
    public void Show(CharacterConfig characterConfig)
    {
        if(_characterConfig == null || characterConfig.CharacterName != _characterConfig.CharacterName)
        {
            _currentStory = 0;
            _lastStory = false;
        }
        _characterConfig = characterConfig;
        SetStory();

        base.Show();
    }
    private void NextStory()
    {
        _currentStory++;

    }
    private void SetStory()
    {
        if (_currentStory < _characterConfig.CharacterStories.Length)
        {
            _characterName.text = _characterConfig.name;
            _storyName.text = _characterConfig.CharacterStories[_currentStory].StoryName;
            _storyText.text = _characterConfig.CharacterStories[_currentStory].Story;
        }
        if(_currentStory == _characterConfig.CharacterStories.Length -1)
        {
            _characterName.text = _characterConfig.name;
            _storyName.text = _characterConfig.CharacterStories[_currentStory].StoryName;
            _storyText.text = _characterConfig.CharacterStories[_currentStory].Story;
            _lastStory = true;
        }
    }

    private void Close()
    {
        if (_canPress)
        {
            Hide();
        }
    }
    private void Positive()
    {
        if (_canPress)
        {
            _canPress = false;
            gemMover.MoveToRight();
        }
    }
    private async void Negative()
    {
        if (_canPress)
        {
            _canPress = false;
            gemMover.MoveToLeft();
        }
    }
    public void PositiveDone()
    {
        _canPress = true;
        if (_lastStory)
        {
            GameEvents.PositivePressed();
            Close();
            return;
        }
        GameEvents.PositivePressed();
        _currentStory++;
        SetStory();
        
    }
    public void NegativeDone()
    {
        _canPress = true;
        if (_lastStory)
        {
            GameEvents.NegativePressed();
            Close();
           return;
        }
        GameEvents.NegativePressed();
        _currentStory++;
        SetStory(); 
    }
}
