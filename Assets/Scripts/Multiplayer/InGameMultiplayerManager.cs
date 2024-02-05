using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

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

    public void SetSpawnPoints(GameObject l_player)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            l_player.transform.position = spawnPositions[i].transform.position;
            PlayerInfo l_pInfo = l_player.GetComponent<PlayerInfo>();
            l_pInfo.playerName = PhotonNetwork.NickName;
            l_pInfo.id = i + 1;
            l_pInfo.spawnpoint = spawnPositions[i].gameObject;
            l_pInfo.playerVisuals[i].SetActive(true);
        }
    }
}
