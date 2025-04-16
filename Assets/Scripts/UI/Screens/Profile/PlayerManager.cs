using System;
using TMPro;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    [SerializeField] private AvatarManager _avatarManager;
    [SerializeField] private ApiManager _apiManager;

    [SerializeField] private TMP_Text _nameProfile;
    [SerializeField] private TMP_Text _nameHome;
    [SerializeField] private TMP_InputField _editorName;

    [SerializeField] private TMP_Text _leaderPos;

    private PlayerList playerList;
    public void SetPlayer()
    {
        _avatarManager.SetSavedPicture();
        SetName();
        SetLeaderPos();
    }
    private void SetName()
    {
        string name = "UserName";
        if (PlayerPrefs.HasKey("Name"))
        {
            name = PlayerPrefs.GetString("Name");
        }
        _nameProfile.text = name; 
        _nameHome.text = name;
        _editorName.text = name;
    }

    
    private async System.Threading.Tasks.Task SetLeaderPos()
    {
        playerList = await _apiManager.GetPlayerList();
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
}
