using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        FetchData();
    }
    public void Achieve(TypesOfAchievements newAchieve)
    {
        FetchData();
        List<TypesOfAchievements> achievements = LoadAchievementList("Achievements");
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i] == newAchieve)
            {
                return;
            }
        }
        achievements.Add(newAchieve);
        SaveAchievementList("Achievements", achievements);
    }

    public List<TypesOfAchievements> GetReceivedAchievements()
    {
        FetchData();
        List<TypesOfAchievements> achievements = LoadAchievementList("Achievements");
        return achievements;
    }
    private void SaveAchievementList(string key, List<TypesOfAchievements> list)
    {
        string serializedList = string.Join(",", list.Select(a => a.ToString()).ToArray());
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save();
        Debug.Log(key);
    }

    private List<TypesOfAchievements> LoadAchievementList(string key)
    {
        string serializedList = PlayerPrefs.GetString(key, string.Empty);
        if (string.IsNullOrEmpty(serializedList))
        {
            return new List<TypesOfAchievements>();
        }

        return serializedList.Split(',')
                             .Select(s => (TypesOfAchievements)System.Enum.Parse(typeof(TypesOfAchievements), s))
                             .ToList();
    }
    public async void FetchData()
    {
        await Task.Delay(500); 
        Debug.Log("Data fetched from API simulator");
        await Task.Delay(500);
        int a = 90 + 66;
        await Task.Delay(a);
    }

}
