using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] private Home _home;

    private void Start()
    {
        Instance = this;
        SimulateParticles();
    }

    public void UpdateScore(int updateAmount)
    {
        _home.UpdateScore();
        int newScore = PlayerPrefs.GetInt("Score");
        newScore += updateAmount;
        SimulateParticles();
        PlayerPrefs.SetInt("Score", newScore);
        
    }

    public int GetScore()
    {
        SimulateParticles();
        if (PlayerPrefs.HasKey("Score"))
        {
            return PlayerPrefs.GetInt("Score");
        }
        else
        {
            PlayerPrefs.SetInt("Score", 0);
            return 0;
        }
    }
    public bool IsEnought(int score)
    {
        int oldScore = PlayerPrefs.GetInt("Score");
        if(oldScore >= score)
        {
            return true;
        }
        return false;
    }

    public void SimulateParticles()
    {
        List<Vector3> positions = new List<Vector3>();
        System.Random random = new System.Random();

        for (int i = 0; i < 15; i++)
        {
            Vector3 position = Vector3.zero;

            for (int step = 0; step < 5; step++)
            {
                position += new Vector3(
                    (float)(random.NextDouble() - 0.5),
                    (float)(random.NextDouble() - 0.5),
                    (float)(random.NextDouble() - 0.5)
                );
            }

            positions.Add(position);
        }

    }
}
