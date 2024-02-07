using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMultiplayerManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab of the player that will be spawned when the players go to the maze.")]
    [SerializeField] private GameObject playerPrefab;

    [Tooltip("The list of spawnpoints where the players can spawn.")]
    [SerializeField] private List<GameObject> spawnPositions = new List<GameObject>();


    private void Start()
    {
        GameObject l_player = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity, 0);
        SetSpawnPoints(l_player);
    }

    /// <summary>
    /// Sets the correct spawn points and some further data for each of the players
    /// </summary>
    /// <param name="l_player"></The gameobject for the player>
    public void SetSpawnPoints(GameObject l_player)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Spawnpoints set
            if (PhotonNetwork.IsMasterClient)
                l_player.transform.position = spawnPositions[0].transform.position;
            else l_player.transform.position = spawnPositions[1].transform.position;

            //Further data set
            PlayerInfo l_pInfo = l_player.GetComponent<PlayerInfo>();
            l_pInfo.playerName = PhotonNetwork.NickName;
            l_pInfo.id = i;
            l_pInfo.spawnpoint = spawnPositions[i].gameObject;
        }
    }

    /// <summary>
    /// Leave the room when there are not enough players in the room
    /// </summary>
    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length != 2)
        {
            LeaveRoom();
        }
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene("Main Menu");
        PhotonNetwork.LeaveRoom();
    }
}
