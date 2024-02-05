using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerEndScreen : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.LeaveRoom();
    }
}
