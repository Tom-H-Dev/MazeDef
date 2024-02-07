using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    [Tooltip("The scripts that should only be active on the local player and will be disabled on other players.")]
    public MonoBehaviour[] localScripts;

    [Tooltip("The gameObjects that should only be active on the local player and will be disabled on other players.")]
    public GameObject[] localObjects;

    [Tooltip("The latest position of the other players.")]
    Vector3 latestPos;

    [Tooltip("The latest rotation of the other players.")]
    Quaternion latestRot;

    [Tooltip("The gameObject for the pause menu")]
    [SerializeField] private GameObject _pauseMenu;

    // Use this for initialization
    void Start()
    {
        if (photonView.IsMine)
        {
            //Pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_pauseMenu.active)
                    _pauseMenu.SetActive(true);
                else _pauseMenu.SetActive(false);
            }
        }
        else
        {
            //Player is Remote, deactivate the scripts and object that should only be enabled for the local player
            for (int i = 0; i < localScripts.Length; i++)
            {
                localScripts[i].enabled = false;
            }
            for (int i = 0; i < localObjects.Length; i++)
            {
                localObjects[i].SetActive(false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
        }
    }
}
