using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerAchievements
{
    [SerializeField] private AchievementsManager _achievementsManager;

    [SerializeField] private List<TypesOfAchievements> _achievementsList;
    [SerializeField] private Image[] _achievementsImage;
    [SerializeField] private Sprite[] _achievementSprites;

    private List<TypesOfAchievements> _receivedAchievements = new List<TypesOfAchievements>();

    public void SetAchievements()
    {
        _receivedAchievements = _achievementsManager.GetReceivedAchievements();

        for(int i = 0; i < _achievementsList.Count; i++)
        {
            for(int j = 0; j < _receivedAchievements.Count; j++)
            {
                if (_achievementsList[i] == _receivedAchievements[j])
                {
                    _achievementsImage[i].sprite = _achievementSprites[i];
                }
            }
        }
    }
}
