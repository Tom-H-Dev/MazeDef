using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Class not used
public class PlayerGateSystem : MonoBehaviourPun
{
    private bool isOpen = false;
    private Animator _animator;
    public void OpenGate(bool l_isOpen, Animator l_animator)
    {
        if (photonView.IsMine)
        {
            // If this is the local player's gate, trigger the animation locally.
            l_animator.SetBool("Open", l_isOpen);
            isOpen = l_isOpen;
            _animator = l_animator;
            // Now, synchronize the trigger state across the network.
            photonView.RPC("TriggerGateAnimation", RpcTarget.All);
        }
    }

    [PunRPC]
    private void TriggerGateAnimation()
    {
        // Trigger the gate animation on all clients except the one that initiated the RPC.
        _animator.SetBool("Open", isOpen);
    }
}
