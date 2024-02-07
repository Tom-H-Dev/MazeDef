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
    [Tooltip("If this scene is the lobby scene.")]
    [SerializeField] bool isLobby = false;


    [Header("User Interface")]
    [Tooltip("The TextMeshPro text element where the name of lobby is in.")]
    [SerializeField] private TMP_Text _lobbyName;

    [Tooltip("The TextMeshPro text element where all the names of the players are set in a row to see what players are in the list.")]
    [SerializeField] private TMP_Text _userNames;

    [Tooltip("The button that is in the screen to start the game.")]
    [SerializeField] private Button _startGameButton;


    private void Start()
    {
        _lobbyName.text = PhotonNetwork.PlayerList[0].NickName + "'s Lobby!";
        SetNamesOfLobbyAndConnectedUsers();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.IsMasterClient)
        {
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
                if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[i].NickName)
                {
                    _userNames.text = _userNames.text + "- " + PhotonNetwork.PlayerList[i].NickName + isMasterClient + " (You)" + "\n";
                }
                else
                {
                    _userNames.text = _userNames.text + "- " + PhotonNetwork.PlayerList[i].NickName + isMasterClient + "\n";
                }
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
        if (isLobby)
        {
            //The lobby will be closed then there are enough players in the lobby
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList.Length == 2)
                {
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    _startGameButton.interactable = true;
                }
                else
                {
                    PhotonNetwork.CurrentRoom.IsVisible = true;
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    _startGameButton.interactable = false;
                }
            }
            //Will update the list of players in the lobby
            SetNamesOfLobbyAndConnectedUsers();
        }
    }

    public void StartGameButton()
    {
        PhotonNetwork.LoadLevel("Maze Coop");
    }
}
