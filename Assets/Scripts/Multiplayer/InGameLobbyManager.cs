using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InGameLobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby Information")]
    [Tooltip("If this scene is the lobby scene")]
    [SerializeField] bool isLobby = false;

    [Tooltip("The prefab of the player that will be spawned when the players go to the maze")]
    [SerializeField] private GameObject playerPrefab;

    [Tooltip("The list of spawnpoints where the players can spawn")]
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();

    [Header("User Interface")]
    [SerializeField] private TMP_Text _lobbyName;
    [SerializeField] private TMP_Text _userNames;
    [SerializeField] private Button _startGameButton;


    private void Start()
    {
        SetSpawnPoints();
        SetNamesOfLobbyAndConnectedUsers();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetSpawnPoints()
    {
        if (!isLobby)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, Quaternion.identity, 0);
            }
        }
    }

    private void SetNamesOfLobbyAndConnectedUsers()
    {
        if (isLobby)
        {
            _lobbyName.text = PhotonNetwork.PlayerList[0].NickName + "'s Lobby!";

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
                string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": Host" : "");
                _userNames.text = "- " + PhotonNetwork.PlayerList[i].NickName + isMasterClient;
            }
        }

    }

    public override void OnLeftRoom()
    {
        //We have left the Room, return back to the GameLobby
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                _startGameButton.interactable = true;
            }
        }
    }

    public void StartGameButton()
    {
        PhotonNetwork.LoadLevel("Maze Coop");
    }
}
