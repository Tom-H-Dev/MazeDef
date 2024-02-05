using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerFinishState : MonoBehaviourPunCallbacks
{
    [Tooltip("The list of player in the current coal area.")]
    [SerializeField] private List<FpsMovement> _playersInArea = new List<FpsMovement>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FpsMovement l_player))
        {
            _playersInArea.Add(l_player);
            if (_playersInArea.Count >= 2)
            {
                //Disconnect and load scene
                //PhotonNetwork.LeaveRoom();
                //SceneManager.LoadScene("End Screen");
                Debug.Log("All player in the end screen");
            }
        }
    }
}
