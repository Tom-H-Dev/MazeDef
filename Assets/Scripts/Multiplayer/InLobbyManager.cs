using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class InLobbyManager : MonoBehaviourPunCallbacks
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
        _lobbyName.text = PhotonNetwork.PlayerList[0].NickName + "'s Lobby!";
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
    public override void OnJoinedRoom()
    {
        Debug.Log("You joined the room.");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log(PhotonNetwork.IsMasterClient);

        SetNamesOfLobbyAndConnectedUsers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Other players joined the room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + "/2 Starting game...");

            SetNamesOfLobbyAndConnectedUsers();
        }
    }

    private void SetNamesOfLobbyAndConnectedUsers()
    {
        if (isLobby)
        {
            _userNames.text = string.Empty;

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
                string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": Host" : "");
                _userNames.text = _userNames.text + "- " + PhotonNetwork.PlayerList[i].NickName + isMasterClient + "\n";
            }
        }

    }

    private void OnPlayerLeftRoom(Player otherPlayer) 
    {
        Debug.Log("A player left the room");
        SetNamesOfLobbyAndConnectedUsers();
    }



    public override void OnLeftRoom()
    {
        //We have left the Room, return back to the GameLobby
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    private void Update()
    {
        if (isLobby)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList.Length == 2)
                {
                    _startGameButton.interactable = true;
                }
            }
            //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            //{
            //    //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
            //    string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": Host" : "");
            //    _userNames.text = _userNames.text + "- " + PhotonNetwork.PlayerList[i].NickName + isMasterClient + "\n";
            //}
        }


    }

    public void StartGameButton()
    {
        PhotonNetwork.LoadLevel("Maze Coop");
    }
}