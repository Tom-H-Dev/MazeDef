using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSounds : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _clip;
    public void OnPlayAudio()
    {
        _source.clip = _clip;
        _source.Play();
    }
}