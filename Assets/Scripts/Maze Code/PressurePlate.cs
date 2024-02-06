using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum PlayerIDs
{
    playerBlue = 1,
    playerRed = 2

}

public class PressurePlate : MonoBehaviourPun, IPunObservable
{
    [Tooltip("To what player does this pressure plate belong.")]
    [SerializeField] PlayerIDs _whatPlayerPressurePlate;

    [Tooltip("The animator for the door that the corresponding pressure polate should open.")]
    [SerializeField] private List<Animator> _animators;

    private PlayerGateSystem _gateSystem;
    private bool isOpen = false;

    private void OnCollisionEnter(Collision collision)
    {
        print("1");
        if (collision.gameObject.TryGetComponent(out PlayerInfo l_player))
        {
            print("2");
            for (int i = 0; i < _animators.Count; i++)
            {
                print("3");
                OpenGate(true, _animators[i]);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInfo l_player))
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                OpenGate(false, _animators[i]);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                bool isGateOpen = _animators[i].GetBool("Open");
                stream.SendNext(isGateOpen);
            }
        }
        else
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                bool isGateOpen = (bool)stream.ReceiveNext();
                _animators[i].SetBool("Open", isGateOpen);
            }

        }
    }

    public void OpenGate(bool l_isOpen, Animator l_animator)
    {
        l_animator.SetBool("Open", l_isOpen);
        isOpen = l_isOpen;
    }
}
