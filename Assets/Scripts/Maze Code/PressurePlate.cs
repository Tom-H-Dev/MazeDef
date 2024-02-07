using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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

    [Tooltip("The Audio Source for the pressure plate.")]
    private AudioSource _audioSource;

    [Tooltip("The clip that will be played when the player stands on the pressure plate.")]
    [SerializeField] private AudioClip _sfx;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// If the player is standind on the pressure plate
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInfo l_player))
        {
            //Play sound effect
            _audioSource.clip = _sfx;
            _audioSource.Play();

            //Open Gate animations
            for (int i = 0; i < _animators.Count; i++)
            {
                OpenGate(true, _animators[i]);
                _animators[i].gameObject.GetComponent<GateSounds>().OnPlayAudio();
            }
        }
    }
    /// <summary>
    /// If the player moves off the pressure plate the gate will be closed
    /// </summary>
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

    //The function to sync the local player with the other players on the network
    //Is part of the IPunObservable interface
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
    /// <summary>
    /// Sets the bool for the animator
    /// </summary>
    /// <param name="l_isOpen"></ The bool if the gate is open or closed>
    /// <param name="l_animator"></The specific animator that neds to be open>
    public void OpenGate(bool l_isOpen, Animator l_animator)
    {
        l_animator.SetBool("Open", l_isOpen);
    }
}
