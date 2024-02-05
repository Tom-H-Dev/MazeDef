using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InGameLobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby Information")]
    [Tooltip("If this scene is the lobby scene")]
    [SerializeField] bool isLobby = false;
    
    [Tooltip("The prefab of the player that will be spawned when the players go to the maze")]
    [SerializeField] private GameObject playerPrefab;

    [Tooltip("The list of spawnpoints where the players can spawn")]
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();


    private void Start()
    {
        SetSpawnPoints();
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
}
