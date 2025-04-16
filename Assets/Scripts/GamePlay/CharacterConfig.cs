
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    [SerializeField] private GodAdvice[] advice;
    [SerializeField] private CharacterStoryData[] characterStories;
    [SerializeField] private string _characterName;
    [SerializeField] private Sprite _characterSprite;

    public GodAdvice[] Advice => advice;
    public CharacterStoryData[] CharacterStories => characterStories;
    public string CharacterName => _characterName;
    public Sprite CharacterSprite => _characterSprite;
}
