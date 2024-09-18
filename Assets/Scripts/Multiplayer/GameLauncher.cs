using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviourPunCallbacks
{
    [Header("User Interface")]
    /// <summary>
    /// The input field where the player enters their name.
    /// </summary>
    [Tooltip("The input field where the player enters their name.")]
    [SerializeField] private TMP_InputField _usernameChoice;

    [Tooltip("The button where the player can join a lobby.")]
    [SerializeField] private Button _joinGameButton;

    [Tooltip("The GameObject which all the loading ui is under.")]
    [SerializeField] private GameObject _loadingMenu;


    [Header("Game/Lobby Info")]
    [Tooltip("The version of the game wich the player joins so the player can only join players with the correct game version and not older or newer versions.")]
    private string gameVersion = "1.0";

    [Tooltip("The check if the player is already joining a room or not.")]
    private bool joiningRoom = false;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
        _loadingMenu.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        //After we connected to Master server, join the Lobby
        if (joiningRoom)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()
    {

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // #Critical
            // Load the Room Level. 
            if (_usernameChoice.text == "" || _usernameChoice.text == null) PhotonNetwork.NickName = "No Name Selected.";
            PhotonNetwork.LoadLevel("Lobby");

        }
    }

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        joiningRoom = true;
        _loadingMenu.SetActive(true);
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    /// <summary>
    /// This function is called by the Input Field for the username input.
    /// </summary>
    public void ChangeNickName()
    {
        //Sets the button interactable and the nickname when nickname is made.
        if (_usernameChoice.text != null || _usernameChoice.text != "")
        {
            PhotonNetwork.NickName = _usernameChoice.text;
            _joinGameButton.interactable = true;    
        }
        else _joinGameButton.interactable = false;
    }
}
