using System;
using UnityEngine;

[Serializable] 
public class CharacterStoryData
{
    [SerializeField] private string _storyName;
    [SerializeField] private string _story;

    public string StoryName => _storyName;
    public string Story => _story;
}