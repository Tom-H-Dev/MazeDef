using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoAnimator: MonoBehaviour
{
    [SerializeField] Animator _anim;

    public void StartWalkingAnimation()
    {
        _anim.SetBool("isMoving", true);
    }
}
