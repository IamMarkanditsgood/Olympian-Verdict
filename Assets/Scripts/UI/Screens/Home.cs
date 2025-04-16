using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _profileButton;
    [SerializeField] private Profile _profile;
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Button _dailyTaskButton;
    [SerializeField] private DailyTasksPopup _dailyTaskPopup;

    [SerializeField] private AvatarManager _avatarManager;
    [SerializeField] private TMP_Text _nameHome;
    [SerializeField] private TMP_Text _leaderPos;
    [SerializeField] private TMP_Text _score;



    private PlayerList playerList;

    private void Start()
    {
        _profileButton.onClick.AddListener(Profile);
        _dailyTaskButton.onClick.AddListener(DailyTask);
        SetHome();
        _gameManager.StartGame();
        _view.SetActive(true);

    }
    private void OnDestroy()
    {
        _dailyTaskButton.onClick.RemoveListener(DailyTask);
        _profileButton.onClick.RemoveListener(Profile);
    }
    public void Show()
    {
        SetHome();
        _gameManager.StartGame();
        _view.SetActive(true);
        SetLeaderPos();
    }
    public void Hide()
    {
        _view.SetActive(false);
    }

    private void SetHome()
    {
        _avatarManager.SetSavedPicture();
        SetName();
        UpdateScore();
    }

    public void UpdateScore()
    {
        _score.text = ResourceManager.Instance.GetScore().ToString();
    }
    private void Profile()
    {
        _profile.Show();
    }
    private void SetName()
    {
        string name = "UserName";
        if (PlayerPrefs.HasKey("Name"))
        {
            name = PlayerPrefs.GetString("Name");
        }
        _nameHome.text = name;
    }


    public async System.Threading.Tasks.Task SetLeaderPos()
    {
        SortPlayersByScore(playerList);
        SetPlayerPos();
    }

    private void SortPlayersByScore(PlayerList playerList)
    {
        if (playerList != null && playerList.players != null)
        {
            playerList.players.Sort((x, y) => y.score.CompareTo(x.score));
        }
    }
    private void SetPlayerPos()
    {
        int playerPos;
        for (int i = 0; i < playerList.players.Count; i++)
        {
            if (playerList.players[i].id == PlayerPrefs.GetInt("PlayerId"))
            {
                playerPos = i + 1;
                _leaderPos.text = "You in rating: " + playerPos + "th";

            }
        }
    }

    private void DailyTask()
    {
        _dailyTaskPopup.Show(); 
    }
}
